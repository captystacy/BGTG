using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using BGTG.POS.Tools.CalendarPlanTool;
using BGTG.POS.Tools.CalendarPlanTool.Interfaces;
using BGTG.POS.Tools.EstimateTool;
using BGTG.POS.Web.Infrastructure.Helpers;
using BGTG.POS.Web.Infrastructure.Services.Interfaces;
using BGTG.POS.Web.ViewModels.CalendarPlanViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BGTG.POS.Web.Infrastructure.Services
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

        public CalendarPlanViewModel GetCalendarPlanViewModel(IFormFileCollection estimateFiles, TotalWorkChapter totalWorkChapter)
        {
            _estimateService.Read(estimateFiles, totalWorkChapter);

            var calendarPlanViewModel = _mapper.Map<CalendarPlanViewModel>(_estimateService.Estimate);

            calendarPlanViewModel.UserWorks.RemoveAll(x => x.Chapter == (int)totalWorkChapter);
            calendarPlanViewModel.UserWorks.Add(new UserWorkViewModel()
            {
                WorkName = CalendarPlanInfo.MainOtherExpensesWorkName,
                Chapter = CalendarPlanInfo.MainOtherExpensesWorkChapter,
                Percentages = new List<decimal>(),
            });

            return calendarPlanViewModel;
        }

        public IEnumerable<decimal> GetTotalPercentages(IFormFileCollection estimateFiles, CalendarPlanViewModel calendarPlanViewModel)
        {
            var calendarPlan = CalculateCalendarPlan(estimateFiles, calendarPlanViewModel);
            return calendarPlan.MainCalendarWorks.Single(x => x.EstimateChapter == (int)calendarPlanViewModel.TotalWorkChapter).ConstructionMonths.Select(x => x.PercentPart);
        }

        public void Write(IFormFileCollection estimateFiles, CalendarPlanViewModel calendarPlanViewModel, string userFullName)
        {
            var calendarPlan = CalculateCalendarPlan(estimateFiles, calendarPlanViewModel);

            var preparatoryTemplatePath = GetPreparatoryTemplatePath();
            var mainTemplatePath = GetMainTemplatePath();

            var savePath = GetSavePath(userFullName);

            _calendarPlanWriter.Write(calendarPlan, preparatoryTemplatePath, mainTemplatePath, savePath);
        }

        private CalendarPlan CalculateCalendarPlan(IFormFileCollection estimateFiles, CalendarPlanViewModel calendarPlanViewModel)
        {
            _estimateService.Read(estimateFiles, calendarPlanViewModel.TotalWorkChapter);

            if (_estimateService.Estimate.ConstructionStartDate == default)
            {
                _estimateService.Estimate.ConstructionStartDate = calendarPlanViewModel.ConstructionStartDate;
            }

            if (_estimateService.Estimate.ConstructionDurationCeiling == 0)
            {
                _estimateService.Estimate.ConstructionDurationCeiling = calendarPlanViewModel.ConstructionDurationCeiling;
            }

            var otherExpensesWork = calendarPlanViewModel.UserWorks.Find(x => x.WorkName == CalendarPlanInfo.MainOtherExpensesWorkName);
            calendarPlanViewModel.UserWorks.Remove(otherExpensesWork);

            SetEstimatePercentages(calendarPlanViewModel.UserWorks);

            var calendarPlan = _calendarPlanCreator.Create(_estimateService.Estimate, otherExpensesWork.Percentages, calendarPlanViewModel.TotalWorkChapter);

            return calendarPlan;
        }

        private void SetEstimatePercentages(List<UserWorkViewModel> userWorks)
        {
            userWorks.ForEach(userWork =>
            {
                _estimateService.Estimate.MainEstimateWorks.Single(estimateWork => estimateWork.WorkName == userWork.WorkName).Percentages.AddRange(userWork.Percentages);
            });
        }

        private string GetPreparatoryTemplatePath()
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, CalendarPlanTemplatesPath, "PreparatoryCalendarPlanTemplate.docx");
        }

        private string GetMainTemplatePath()
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, CalendarPlanTemplatesPath, $"MainCalendarPlanTemplate{_estimateService.Estimate.ConstructionDurationCeiling}.docx");
        }

        public string GetSavePath(string userFullName)
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, CalendarPlansSavePath, $"CalendarPlan{userFullName.RemoveBackslashes()}.docx");
        }

        public string GetFileName(string objectCipher)
        {
            return $"{objectCipher}КП.docx";
        }
    }
}
