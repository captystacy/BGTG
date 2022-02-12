using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Core;
using BGTG.Data.CustomRepositories.Interfaces;
using BGTG.POS.CalendarPlanTool;
using BGTG.POS.DurationTools.DurationByLCTool;
using BGTG.POS.EnergyAndWaterTool;
using BGTG.POS.ProjectTool.Interfaces;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.POSServices.Interfaces;
using BGTG.Web.ViewModels.POSViewModels;
using Calabonga.OperationResults;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace BGTG.Web.Infrastructure.Services.POSServices
{
    public class ProjectService : IProjectService
    {
        private readonly IECPProjectWriter _ecpProjectWriter;
        private readonly IDurationByLCService _durationByLCService;
        private readonly ICalendarPlanService _calendarPlanService;
        private readonly IEnergyAndWaterService _energyAndWaterService;
        private readonly IConstructionObjectRepository _constructionObjectRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private const string TemplatesPath = @"AppData\Templates\ProjectTemplates";
        private const string UserFilesPath = @"AppData\UserFiles\ProjectFiles";

        public ProjectService(IECPProjectWriter ecpProjectWriter, IDurationByLCService durationByLCService, ICalendarPlanService calendarPlanService, IEnergyAndWaterService energyAndWaterService, IConstructionObjectRepository constructionObjectRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _ecpProjectWriter = ecpProjectWriter;
            _durationByLCService = durationByLCService;
            _calendarPlanService = calendarPlanService;
            _energyAndWaterService = energyAndWaterService;
            _constructionObjectRepository = constructionObjectRepository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<OperationResult<string>> Write(ProjectViewModel viewModel, string windowsName)
        {
            var operationResult = OperationResult.CreateResult<string>();

            var constructionObject = await _constructionObjectRepository.GetFirstOrDefaultAsync(x => x.Cipher == viewModel.ObjectCipher, null,
                x => x.Include(x => x.POS).ThenInclude(x => x.DurationByLC)
                    .Include(x => x.POS).ThenInclude(x => x.CalendarPlan).ThenInclude(x => x.MainCalendarWorks).ThenInclude(x => x.ConstructionMonths)
                    .Include(x => x.POS).ThenInclude(x => x.CalendarPlan).ThenInclude(x => x.PreparatoryCalendarWorks).ThenInclude(x => x.ConstructionMonths)
                    .Include(x => x.POS).ThenInclude(x => x.EnergyAndWater));

            if (constructionObject == null)
            {
                operationResult.AddError(AppData.ConstructionObjectIsNull);
                return operationResult;
            }

            if (constructionObject.POS == null)
            {
                operationResult.AddError(AppData.POSIsNull);
                return operationResult;
            }

            if (constructionObject.POS.DurationByLC == null)
            {
                operationResult.AddError(AppData.DurationByLCNotCalculated);
                return operationResult;
            }

            if (constructionObject.POS.DurationByLC.NumberOfEmployees != 4 &&
                constructionObject.POS.DurationByLC.NumberOfEmployees != 8)
            {
                operationResult.AddError(AppData.ECPProjectOnly4Or8Employees);
                return operationResult;
            }

            if (constructionObject.POS.CalendarPlan == null)
            {
                operationResult.AddError(AppData.CalendarPlanNotCalculated);
                return operationResult;
            }

            if (constructionObject.POS.EnergyAndWater == null)
            {
                operationResult.AddError(AppData.EnergyAndWaterNotCalculated);
                return operationResult;
            }

            var durationByLC = _mapper.Map<DurationByLC>(constructionObject.POS.DurationByLC);
            _durationByLCService.Write(durationByLC, windowsName);

            var calendarPlan = _mapper.Map<CalendarPlan>(constructionObject.POS.CalendarPlan);
            _calendarPlanService.Write(calendarPlan, windowsName);

            var energyAndWater = _mapper.Map<EnergyAndWater>(constructionObject.POS.EnergyAndWater);
            _energyAndWaterService.Write(energyAndWater, windowsName);

            var durationByLCPath = _durationByLCService.GetSavePath(windowsName);
            var calendarPlanPath = _calendarPlanService.GetSavePath(windowsName);
            var energyAndWaterPath = _energyAndWaterService.GetSavePath(windowsName);

            var templatePath = GetTemplatePath(viewModel, durationByLC.NumberOfEmployees, windowsName);
            var savePath = GetSavePath(windowsName);

            _ecpProjectWriter.Write(viewModel.ObjectCipher, durationByLC, calendarPlan.ConstructionStartDate, durationByLCPath, calendarPlanPath, energyAndWaterPath, templatePath, savePath);

            operationResult.Result = string.Empty;
            return operationResult;
        }

        private string GetTemplatePath(ProjectViewModel viewModel, int numberOfEmployees, string windowsName)
        {
            var householdTownChar = viewModel.HouseholdTownIncluded ? '+' : '-';
            var templateFileName = $"HouseholdTown{householdTownChar}.docx";
            var templatePath = Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath,
                viewModel.ProjectTemplate.ToString(), viewModel.ChiefProjectEngineer.ToString(),
                windowsName.RemoveBackslashes(), $"Employees{numberOfEmployees}", templateFileName);

            if (!File.Exists(templatePath))
            {
                return Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath,
                    viewModel.ProjectTemplate.ToString(), viewModel.ChiefProjectEngineer.ToString(),
                    AppData.Unknown, $"Employees{numberOfEmployees}", templateFileName);
            }

            return templatePath;
        }

        public string GetSavePath(string windowsName)
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, UserFilesPath, $"{windowsName.RemoveBackslashes()}.docx");
        }
    }
}
