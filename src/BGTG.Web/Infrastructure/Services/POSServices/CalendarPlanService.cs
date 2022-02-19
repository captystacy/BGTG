using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using BGTG.POS.CalendarPlanTool;
using BGTG.POS.CalendarPlanTool.Base;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels.CalendarPlanViewModels;
using Microsoft.AspNetCore.Hosting;

namespace BGTG.Web.Infrastructure.Services.POSServices
{
    public class CalendarPlanService : ICalendarPlanService
    {
        private readonly IEstimateService _estimateService;
        private readonly ICalendarPlanCreator _calendarPlanCreator;
        private readonly ICalendarPlanWriter _calendarPlanWriter;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        private const string PreparatoryCalendarPlanTemplateFileName = "Preparatory.docx";

        private const string TemplatesPath = @"AppData\Templates\POSTemplates\CalendarPlanTemplates";
        private const string UserFilesPath = @"AppData\UserFiles\POSFiles\CalendarPlanFiles";

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
            calendarPlanCreateViewModel.CalendarWorkViewModels.Add(new CalendarWorkViewModel
            {
                WorkName = CalendarPlanInfo.MainOtherExpensesWorkName,
                Chapter = CalendarPlanInfo.MainOtherExpensesWorkChapter,
                Percentages = new List<decimal>(),
            });

            return calendarPlanCreateViewModel;
        }

        public IEnumerable<decimal> GetTotalPercentages(CalendarPlanCreateViewModel viewModel)
        {
            var calendarPlan = CalculateCalendarPlan(viewModel);
            return calendarPlan.MainCalendarWorks.First(x => x.EstimateChapter == (int)viewModel.TotalWorkChapter).ConstructionMonths.Select(x => x.PercentPart);
        }

        public CalendarPlan Write(CalendarPlanCreateViewModel viewModel)
        {
            var calendarPlan = CalculateCalendarPlan(viewModel);

            return Write(calendarPlan);
        }

        public CalendarPlan Write(CalendarPlan calendarPlan)
        {
            var preparatoryTemplatePath = GetPreparatoryTemplatePath();
            var mainTemplatePath = GetMainTemplatePath(calendarPlan.ConstructionDurationCeiling);

            var savePath = GetSavePath();

            _calendarPlanWriter.Write(calendarPlan, preparatoryTemplatePath, mainTemplatePath, savePath);

            return calendarPlan;
        }

        private CalendarPlan CalculateCalendarPlan(CalendarPlanCreateViewModel viewModel)
        {
            _estimateService.Read(viewModel.EstimateFiles, viewModel.TotalWorkChapter);

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

            var otherExpensesWork = viewModel.CalendarWorkViewModels.Find(x => x.WorkName == CalendarPlanInfo.MainOtherExpensesWorkName)!;
            viewModel.CalendarWorkViewModels.Remove(otherExpensesWork);

            SetEstimatePercentages(viewModel.CalendarWorkViewModels);

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

        public string GetSavePath()
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, UserFilesPath, $"{IdentityHelper.Instance.User!.Name!.RemoveBackslashes()}.docx");
        }
    }
}
