using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities;
using BGTG.Entities.Core;
using BGTG.POS.CalendarPlanTool;
using BGTG.POS.DurationTools.DurationByLCTool;
using BGTG.POS.EnergyAndWaterTool;
using BGTG.POS.ProjectTool.Base;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace BGTG.Web.Infrastructure.Services.POSServices;

public class ProjectService : IProjectService
{
    private readonly IECPProjectWriter _ecpProjectWriter;
    private readonly IDurationByLCService _durationByLCService;
    private readonly ICalendarPlanService _calendarPlanService;
    private readonly IEnergyAndWaterService _energyAndWaterService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _webHostEnvironment;

    private const string TemplatesPath = @"AppData\Templates\POSTemplates\ProjectTemplates";
    private const string UserFilesPath = @"AppData\UserFiles\POSFiles\ProjectFiles";

    public ProjectService(IECPProjectWriter ecpProjectWriter, IDurationByLCService durationByLCService, ICalendarPlanService calendarPlanService, IEnergyAndWaterService energyAndWaterService, IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
    {
        _ecpProjectWriter = ecpProjectWriter;
        _durationByLCService = durationByLCService;
        _calendarPlanService = calendarPlanService;
        _energyAndWaterService = energyAndWaterService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<OperationResult<ProjectViewModel>> Write(ProjectViewModel viewModel)
    {
        var operation = OperationResult.CreateResult<ProjectViewModel>();
        operation.Result = viewModel;

        var constructionObject = await _unitOfWork.GetRepository<ConstructionObjectEntity>().GetFirstOrDefaultAsync(
            predicate: x => x.Cipher == viewModel.ObjectCipher,
            include: x => x
                .Include(x => x.POS).ThenInclude(x => x!.DurationByLC)
                .Include(x => x.POS).ThenInclude(x => x!.CalendarPlan).ThenInclude(x => x!.MainCalendarWorks).ThenInclude(x => x.ConstructionMonths)
                .Include(x => x.POS).ThenInclude(x => x!.CalendarPlan).ThenInclude(x => x!.PreparatoryCalendarWorks).ThenInclude(x => x.ConstructionMonths)
                .Include(x => x.POS).ThenInclude(x => x!.EnergyAndWater));

        if (constructionObject == null)
        {
            operation.AddError(AppData.ConstructionObjectIsNull);
            return operation;
        }

        if (constructionObject.POS == null)
        {
            operation.AddError(AppData.POSIsNull);
            return operation;
        }

        if (constructionObject.POS.DurationByLC == null)
        {
            operation.AddError(AppData.DurationByLCNotCalculated);
            return operation;
        }

        if (constructionObject.POS.DurationByLC.NumberOfEmployees != 4 &&
            constructionObject.POS.DurationByLC.NumberOfEmployees != 8)
        {
            operation.AddError(AppData.ECPProjectOnly4Or8Employees);
            return operation;
        }

        if (constructionObject.POS.CalendarPlan == null)
        {
            operation.AddError(AppData.CalendarPlanNotCalculated);
            return operation;
        }

        if (constructionObject.POS.EnergyAndWater == null)
        {
            operation.AddError(AppData.EnergyAndWaterNotCalculated);
            return operation;
        }

        var durationByLC = _mapper.Map<DurationByLC>(constructionObject.POS.DurationByLC);
        _durationByLCService.Write(durationByLC);

        var calendarPlan = _mapper.Map<CalendarPlan>(constructionObject.POS.CalendarPlan);
        _calendarPlanService.Write(calendarPlan);

        var energyAndWater = _mapper.Map<EnergyAndWater>(constructionObject.POS.EnergyAndWater);
        _energyAndWaterService.Write(energyAndWater);

        var durationByLCPath = _durationByLCService.GetSavePath();
        var calendarPlanPath = _calendarPlanService.GetSavePath();
        var energyAndWaterPath = _energyAndWaterService.GetSavePath();

        var templatePath = GetTemplatePath(viewModel, durationByLC.NumberOfEmployees);
        var savePath = GetSavePath();

        _ecpProjectWriter.Write(viewModel.ObjectCipher, durationByLC, calendarPlan.ConstructionStartDate, durationByLCPath, calendarPlanPath, energyAndWaterPath, templatePath, savePath);

        return operation;
    }

    private string GetTemplatePath(ProjectViewModel viewModel, int numberOfEmployees)
    {
        var templateFileName = $"HouseholdTown{TemplateHelper.GetPlusOrMinus(viewModel.HouseholdTownIncluded)}.docx";
        var templatePath = Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath,
            viewModel.ProjectTemplate.ToString(), viewModel.ChiefProjectEngineer.ToString(),
            IdentityHelper.Instance.User!.Name!.RemoveBackslashes(), $"Employees{numberOfEmployees}", templateFileName);

        if (!File.Exists(templatePath))
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath,
                viewModel.ProjectTemplate.ToString(), viewModel.ChiefProjectEngineer.ToString(),
                AppData.Unknown, $"Employees{numberOfEmployees}", templateFileName);
        }

        return templatePath;
    }

    public string GetSavePath()
    {
        return Path.Combine(_webHostEnvironment.ContentRootPath, UserFilesPath, $"{IdentityHelper.Instance.User!.Name!.RemoveBackslashes()}.docx");
    }
}