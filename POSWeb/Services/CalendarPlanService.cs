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

        private const string _calendarPlansPath = "UsersFiles\\CalendarPlans";
        private const string _calendarPlanTemplatesPath = "Templates\\CalendarPlanTemplates";

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
            _estimateService.ReadEstimateFiles(estimateFiles, totalWorkChapter);

            var calendarPlanVM = _mapper.Map<CalendarPlanVM>(_estimateService.Estimate);

            calendarPlanVM.UserWorks.RemoveAll(x => x.Chapter == CalendarPlanInfo.MainTotalWork1To12Chapter);
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
            return calendarPlan.MainCalendarWorks.Single(x => x.EstimateChapter == CalendarPlanInfo.MainTotalWork1To12Chapter).ConstructionMonths.Select(x => x.PercentPart);
        }

        public void WriteCalendarPlan(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM, string userFullName)
        {
            var calendarPlan = CalculateCalendarPlan(estimateFiles, calendarPlanVM);
            var preparatoryTemplatePath = Path.Combine(_webHostEnvironment.WebRootPath, _calendarPlanTemplatesPath, "PreparatoryCalendarPlanTemplate.docx");
            var mainTemplatePath = Path.Combine(_webHostEnvironment.WebRootPath, _calendarPlanTemplatesPath, $"MainCalendarPlanTemplate{_estimateService.Estimate.ConstructionDurationCeiling}.docx");
            var savePath = Path.Combine(GetCalendarPlansPath(), GetCalendarPlanFileName(userFullName));

            _calendarPlanWriter.Write(calendarPlan, preparatoryTemplatePath, mainTemplatePath, savePath);
        }

        private CalendarPlan CalculateCalendarPlan(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM)
        {
            _estimateService.ReadEstimateFiles(estimateFiles, calendarPlanVM.TotalWorkChapter);

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

        public string GetCalendarPlansPath()
        {
            return Path.Combine(_webHostEnvironment.WebRootPath, _calendarPlansPath);
        }

        public string GetCalendarPlanFileName(string userFullName)
        {
            return $"CalendarPlan{userFullName.RemoveBackslashes()}.docx";
        }

        public string GetDownloadCalendarPlanFileName(string objectCipher)
        {
            return $"{objectCipher}КП.docx";
        }
    }
}
