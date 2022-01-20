using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using BGTGWeb.Helpers;
using BGTGWeb.Models;
using BGTGWeb.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using POS.CalendarPlanLogic;
using POS.CalendarPlanLogic.Interfaces;
using POS.EstimateLogic;

namespace BGTGWeb.Services
{
    public class CalendarPlanService : ICalendarPlanService
    {
        private readonly IEstimateService _estimateService;
        private readonly ICalendarPlanCreator _calendarPlanCreator;
        private readonly ICalendarPlanWriter _calendarPlanWriter;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        private const string CalendarPlansSavePath = @"UsersFiles\CalendarPlans";
        private const string CalendarPlanTemplatesPath = @"Templates\CalendarPlanTemplates";

        public CalendarPlanService(IEstimateService estimateService, ICalendarPlanCreator calendarPlanCreator,
            ICalendarPlanWriter calendarPlanWriter, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _estimateService = estimateService;
            _calendarPlanCreator = calendarPlanCreator;
            _calendarPlanWriter = calendarPlanWriter;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        public CalendarPlanVM GetCalendarPlanVM(IEnumerable<IFormFile> estimateFiles, TotalWorkChapter totalWorkChapter)
        {
            _estimateService.Read(estimateFiles, totalWorkChapter);

            var calendarPlanVM = _mapper.Map<CalendarPlanVM>(_estimateService.Estimate);

            calendarPlanVM.UserWorks.RemoveAll(x => x.Chapter == (int)totalWorkChapter);
            calendarPlanVM.UserWorks.Add(new UserWorkVM()
            {
                WorkName = CalendarPlanInfo.MainOtherExpensesWorkName,
                Chapter = CalendarPlanInfo.MainOtherExpensesWorkChapter,
                Percentages = new List<decimal>(),
            });

            return calendarPlanVM;
        }

        public IEnumerable<decimal> GetTotalPercentages(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM)
        {
            var calendarPlan = CalculateCalendarPlan(estimateFiles, calendarPlanVM);
            return calendarPlan.MainCalendarWorks.Single(x => x.EstimateChapter == (int)calendarPlanVM.TotalWorkChapter).ConstructionMonths.Select(x => x.PercentPart);
        }

        public void Write(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM, string userFullName)
        {
            var calendarPlan = CalculateCalendarPlan(estimateFiles, calendarPlanVM);

            var preparatoryTemplatePath = GetPreparatoryTemplatePath();
            var mainTemplatePath = GetMainTemplatePath();

            var savePath = GetSavePath(userFullName);

            _calendarPlanWriter.Write(calendarPlan, preparatoryTemplatePath, mainTemplatePath, savePath);
        }

        private CalendarPlan CalculateCalendarPlan(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM)
        {
            _estimateService.Read(estimateFiles, calendarPlanVM.TotalWorkChapter);

            if (_estimateService.Estimate.ConstructionStartDate == default)
            {
                _estimateService.Estimate.ConstructionStartDate = calendarPlanVM.ConstructionStartDate;
            }

            if (_estimateService.Estimate.ConstructionDurationCeiling == 0)
            {
                _estimateService.Estimate.ConstructionDurationCeiling = calendarPlanVM.ConstructionDurationCeiling;
            }

            var otherExpensesWork = calendarPlanVM.UserWorks.Find(x => x.WorkName == CalendarPlanInfo.MainOtherExpensesWorkName);
            calendarPlanVM.UserWorks.Remove(otherExpensesWork);

            SetEstimatePercentages(calendarPlanVM.UserWorks);

            var calendarPlan = _calendarPlanCreator.Create(_estimateService.Estimate, otherExpensesWork.Percentages, calendarPlanVM.TotalWorkChapter);

            return calendarPlan;
        }

        private void SetEstimatePercentages(List<UserWorkVM> userWorks)
        {
            userWorks.ForEach(userWork =>
            {
                _estimateService.Estimate.MainEstimateWorks.Single(estimateWork => estimateWork.WorkName == userWork.WorkName).Percentages.AddRange(userWork.Percentages);
            });
        }

        private string GetPreparatoryTemplatePath()
        {
            return Path.Combine(_webHostEnvironment.WebRootPath, CalendarPlanTemplatesPath, "PreparatoryCalendarPlanTemplate.docx");
        }

        private string GetMainTemplatePath()
        {
            return Path.Combine(_webHostEnvironment.WebRootPath, CalendarPlanTemplatesPath, $"MainCalendarPlanTemplate{_estimateService.Estimate.ConstructionDurationCeiling}.docx");
        }

        public string GetSavePath(string userFullName)
        {
            return Path.Combine(_webHostEnvironment.WebRootPath, CalendarPlansSavePath, $"CalendarPlan{userFullName.RemoveBackslashes()}.docx");
        }

        public string GetFileName(string objectCipher)
        {
            return $"{objectCipher}КП.docx";
        }
    }
}
