﻿using AutoMapper;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Tools.CalendarPlanTool.Base;
using POS.Infrastructure.Tools.CalendarPlanTool.Models;
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

    private const string TemplatesPath = @"Templates\CalendarPlanTemplates";

    public CalendarPlanService(IEstimateService estimateService, ICalendarPlanCreator calendarPlanCreator,
        ICalendarPlanWriter calendarPlanWriter, IWebHostEnvironment webHostEnvironment, IMapper mapper)
    {
        _estimateService = estimateService;
        _calendarPlanCreator = calendarPlanCreator;
        _calendarPlanWriter = calendarPlanWriter;
        _webHostEnvironment = webHostEnvironment;
        _mapper = mapper;
    }

    public CalendarPlanViewModel GetCalendarPlanViewModel(CalendarPlanCreateViewModel viewModel)
    {
        _estimateService.Read(viewModel.EstimateFiles, viewModel.TotalWorkChapter);

        var calendarPlanViewModel = _mapper.Map<CalendarPlanViewModel>(_estimateService.Estimate);
        calendarPlanViewModel.CalendarWorks.RemoveAll(x => x.Chapter == (int)viewModel.TotalWorkChapter);

        calendarPlanViewModel.CalendarWorks.Add(new CalendarWorkViewModel
        {
            WorkName = AppData.MainOtherExpensesWorkName,
            Chapter = AppData.MainOtherExpensesWorkChapter,
            Percentages = new List<decimal>()
        });

        return calendarPlanViewModel;
    }

    public IEnumerable<decimal> GetTotalPercentages(CalendarPlanViewModel viewModel)
    {
        var calendarPlan = CalculateCalendarPlan(viewModel);
        return calendarPlan.MainCalendarWorks.First(x => x.EstimateChapter == (int)viewModel.TotalWorkChapter).ConstructionMonths.Select(x => x.PercentPart);
    }

    public MemoryStream Write(CalendarPlanViewModel viewModel)
    {
        var calendarPlan = CalculateCalendarPlan(viewModel);
        var preparatoryTemplatePath = GetPreparatoryTemplatePath();
        var mainTemplatePath = GetMainTemplatePath(calendarPlan.ConstructionDurationCeiling);

        return _calendarPlanWriter.Write(calendarPlan, preparatoryTemplatePath, mainTemplatePath);
    }

    private CalendarPlan CalculateCalendarPlan(CalendarPlanViewModel viewModel)
    {
        _estimateService.Read(viewModel.EstimateFiles, viewModel.TotalWorkChapter);

        if (_estimateService.Estimate.ConstructionStartDate == default)
        {
            _estimateService.Estimate.ConstructionStartDate = viewModel.ConstructionStartDate;
        }

        if (_estimateService.Estimate.ConstructionDurationCeiling == 0)
        {
            _estimateService.Estimate.ConstructionDurationCeiling = (int)decimal.Ceiling(viewModel.ConstructionDuration);
        }

        if (_estimateService.Estimate.ConstructionDuration == 0)
        {
            _estimateService.Estimate.ConstructionDuration = viewModel.ConstructionDuration;
        }

        var otherExpensesWork = viewModel.CalendarWorks.Find(x => x.WorkName == AppData.MainOtherExpensesWorkName)!;
        viewModel.CalendarWorks.Remove(otherExpensesWork);

        SetEstimatePercentages(viewModel.CalendarWorks);

        var calendarPlan = _calendarPlanCreator.Create(_estimateService.Estimate, otherExpensesWork.Percentages, viewModel.TotalWorkChapter);

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