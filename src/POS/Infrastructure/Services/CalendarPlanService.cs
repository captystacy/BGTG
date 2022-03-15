using AutoMapper;
using POS.DomainModels.CalendarPlanDomainModels;
using POS.Infrastructure.Creators.Base;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;
using POS.ViewModels;

namespace POS.Infrastructure.Services;

public class CalendarPlanService : ICalendarPlanService
{
    private readonly IEstimateService _estimateService;
    private readonly ICalendarPlanCreator _calendarPlanCreator;
    private readonly ICalendarPlanWriter _calendarPlanWriter;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IMapper _mapper;

    private const string PreparatoryCalendarPlanTemplateFileName = "Preparatory.docx";

    private const string TemplatesPath = @"Infrastructure\Templates\CalendarPlanTemplates";

    public CalendarPlanService(IEstimateService estimateService, ICalendarPlanCreator calendarPlanCreator,
        ICalendarPlanWriter calendarPlanWriter, IWebHostEnvironment webHostEnvironment, IMapper mapper)
    {
        _estimateService = estimateService;
        _calendarPlanCreator = calendarPlanCreator;
        _calendarPlanWriter = calendarPlanWriter;
        _webHostEnvironment = webHostEnvironment;
        _mapper = mapper;
    }

    public CalendarPlanViewModel GetCalendarPlanViewModel(CalendarPlanCreateViewModel dto)
    {
        _estimateService.Read(dto.EstimateFiles, dto.TotalWorkChapter);

        var calendarPlanViewModel = _mapper.Map<CalendarPlanViewModel>(_estimateService.Estimate);
        calendarPlanViewModel.CalendarWorks.RemoveAll(x => x.Chapter == (int)dto.TotalWorkChapter);

        calendarPlanViewModel.CalendarWorks.Add(new CalendarWorkViewModel
        {
            WorkName = Constants.AppConstants.MainOtherExpensesWorkName,
            Chapter = Constants.AppConstants.MainOtherExpensesWorkChapter,
            Percentages = new List<decimal>()
        });

        return calendarPlanViewModel;
    }

    public IEnumerable<decimal> GetTotalPercentages(CalendarPlanViewModel dto)
    {
        var calendarPlan = CalculateCalendarPlan(dto);
        return calendarPlan.MainCalendarWorks.First(x => x.EstimateChapter == (int)dto.TotalWorkChapter).ConstructionMonths.Select(x => x.PercentPart);
    }

    public MemoryStream Write(CalendarPlanViewModel dto)
    {
        var calendarPlan = CalculateCalendarPlan(dto);
        var preparatoryTemplatePath = GetPreparatoryTemplatePath();
        var mainTemplatePath = GetMainTemplatePath(calendarPlan.ConstructionDurationCeiling);

        return _calendarPlanWriter.Write(calendarPlan, preparatoryTemplatePath, mainTemplatePath);
    }

    private CalendarPlan CalculateCalendarPlan(CalendarPlanViewModel dto)
    {
        _estimateService.Read(dto.EstimateFiles, dto.TotalWorkChapter);

        if (_estimateService.Estimate.ConstructionStartDate == default)
        {
            _estimateService.Estimate.ConstructionStartDate = dto.ConstructionStartDate;
        }

        if (_estimateService.Estimate.ConstructionDurationCeiling == 0)
        {
            _estimateService.Estimate.ConstructionDurationCeiling = (int)decimal.Ceiling(dto.ConstructionDuration);
        }

        if (_estimateService.Estimate.ConstructionDuration == 0)
        {
            _estimateService.Estimate.ConstructionDuration = dto.ConstructionDuration;
        }

        var otherExpensesWork = dto.CalendarWorks.Find(x => x.WorkName == Constants.AppConstants.MainOtherExpensesWorkName)!;
        dto.CalendarWorks.Remove(otherExpensesWork);

        SetEstimatePercentages(dto.CalendarWorks);

        var calendarPlan = _calendarPlanCreator.Create(_estimateService.Estimate, otherExpensesWork.Percentages, dto.TotalWorkChapter);

        return calendarPlan;
    }

    private void SetEstimatePercentages(IEnumerable<CalendarWorkViewModel> viewModels)
    {
        foreach (var viewModel in viewModels)
        {
            _estimateService.Estimate.MainEstimateWorks.First(estimateWork => estimateWork.WorkName == viewModel.WorkName).Percentages.AddRange(viewModel.Percentages);
        }
    }

    private string GetPreparatoryTemplatePath()
    {
        return Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath, PreparatoryCalendarPlanTemplateFileName);
    }

    private string GetMainTemplatePath(int constructionDurationCeiling)
    {
        return Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath, $"Main{constructionDurationCeiling}.docx");
    }
}