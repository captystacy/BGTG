using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using BGTG.POS.CalendarPlanTool;
using BGTG.POS.CalendarPlanTool.Interfaces;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.Interfaces;
using BGTG.Web.ViewModels.POSViewModels.CalendarPlanViewModels;
using BGTG.Web.ViewModels.POSViewModels.CalendarWorkViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BGTG.Web.Infrastructure.Services
{
    public class CalendarPlanService : ICalendarPlanService
    {
        private readonly IEstimateService _estimateService;
        private readonly ICalendarPlanCreator _calendarPlanCreator;
        private readonly ICalendarPlanWriter _calendarPlanWriter;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        private const string CalendarPlansSavePath = @"AppData\UserFiles\CalendarPlanFiles";
        private const string CalendarPlanTemplatesPath = @"AppData\Templates\CalendarPlanTemplates";

        public CalendarPlanService(IEstimateService estimateService, ICalendarPlanCreator calendarPlanCreator,
            ICalendarPlanWriter calendarPlanWriter, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _estimateService = estimateService;
            _calendarPlanCreator = calendarPlanCreator;
            _calendarPlanWriter = calendarPlanWriter;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        public CalendarPlanCreateViewModel GetCalendarPlanCreateViewModel(CalendarPlanPreCreateViewModel viewModel)
        {
            _estimateService.Read(viewModel.EstimateFiles, viewModel.TotalWorkChapter);

            var calendarPlanCreateViewModel = _mapper.Map<CalendarPlanCreateViewModel>(_estimateService.Estimate);

            calendarPlanCreateViewModel.CalendarWorkViewModels.RemoveAll(x => x.Chapter == (int)viewModel.TotalWorkChapter);
            calendarPlanCreateViewModel.CalendarWorkViewModels.Add(new CalendarWorkViewModel()
            {
                WorkName = CalendarPlanInfo.MainOtherExpensesWorkName,
                Chapter = CalendarPlanInfo.MainOtherExpensesWorkChapter,
                Percentages = new List<decimal>(),
            });

            return calendarPlanCreateViewModel;
        }

        public IEnumerable<decimal> GetTotalPercentages(CalendarPlanCreateViewModel viewModel)
        {
            var calendarPlan = CalculateCalendarPlan(viewModel.EstimateFiles, viewModel);
            return calendarPlan.MainCalendarWorks.Single(x => x.EstimateChapter == (int)viewModel.TotalWorkChapter).ConstructionMonths.Select(x => x.PercentPart);
        }

        public CalendarPlan Write(CalendarPlanCreateViewModel viewModel, string windowsName)
        {
            var calendarPlan = CalculateCalendarPlan(viewModel.EstimateFiles, viewModel);

            return Write(calendarPlan, windowsName);
        }

        public CalendarPlan Write(CalendarPlan calendarPlan, string windowsName)
        {
            var preparatoryTemplatePath = GetPreparatoryTemplatePath();
            var mainTemplatePath = GetMainTemplatePath(calendarPlan.ConstructionDurationCeiling);

            var savePath = GetSavePath(windowsName);

            _calendarPlanWriter.Write(calendarPlan, preparatoryTemplatePath, mainTemplatePath, savePath);

            return calendarPlan;
        }

        private CalendarPlan CalculateCalendarPlan(IFormFileCollection estimateFiles, CalendarPlanCreateViewModel viewModel)
        {
            _estimateService.Read(estimateFiles, viewModel.TotalWorkChapter);

            if (_estimateService.Estimate.ConstructionStartDate == default)
            {
                _estimateService.Estimate.ConstructionStartDate = viewModel.ConstructionStartDate;
            }

            if (_estimateService.Estimate.ConstructionDurationCeiling == 0)
            {
                _estimateService.Estimate.ConstructionDurationCeiling = viewModel.ConstructionDurationCeiling;
            }

            if (_estimateService.Estimate.ConstructionDuration == 0)
            {
                _estimateService.Estimate.ConstructionDuration = viewModel.ConstructionDuration;
            }

            var otherExpensesWork = viewModel.CalendarWorkViewModels.Find(x => x.WorkName == CalendarPlanInfo.MainOtherExpensesWorkName);
            viewModel.CalendarWorkViewModels.Remove(otherExpensesWork);

            SetEstimatePercentages(viewModel.CalendarWorkViewModels);

            var calendarPlan = _calendarPlanCreator.Create(_estimateService.Estimate, otherExpensesWork.Percentages, viewModel.TotalWorkChapter);

            return calendarPlan;
        }

        private void SetEstimatePercentages(List<CalendarWorkViewModel> viewModels)
        {
            viewModels.ForEach(calendarWorkViewModel =>
            {
                _estimateService.Estimate.MainEstimateWorks.Single(estimateWork => estimateWork.WorkName == calendarWorkViewModel.WorkName).Percentages.AddRange(calendarWorkViewModel.Percentages);
            });
        }

        private string GetPreparatoryTemplatePath()
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, CalendarPlanTemplatesPath, "PreparatoryCalendarPlanTemplate.docx");
        }

        private string GetMainTemplatePath(int constructionDurationCeiling)
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, CalendarPlanTemplatesPath, $"MainCalendarPlanTemplate{constructionDurationCeiling}.docx");
        }

        public string GetSavePath(string windowsName)
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, CalendarPlansSavePath, $"CalendarPlan{windowsName.RemoveBackslashes()}.docx");
        }
    }
}
