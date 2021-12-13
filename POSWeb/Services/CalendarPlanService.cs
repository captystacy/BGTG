using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using POSCore.CalendarPlanLogic;
using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;
using POSWeb.Helpers;
using POSWeb.Models;
using POSWeb.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace POSWeb.Services
{
    public class CalendarPlanService : ICalendarPlanService
    {
        private readonly IEstimateService _estimateService;
        private readonly ICalendarPlanCreator _calendarPlanCreator;
        private readonly ICalendarPlanWriter _calendarPlanWriter;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public const string DocxMimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

        private const string _calendarPlansPath = "UsersFiles\\CalendarPlans";
        private const string _calendarPlanTemplatesPath = "Templates\\CalendarPlanTemplates";

        public CalendarPlanService(IEstimateService estimateService, ICalendarPlanCreator calendarPlanCreator,
            ICalendarPlanWriter calendarPlanWriter, IWebHostEnvironment webHostEnvironment)
        {
            _estimateService = estimateService;
            _calendarPlanCreator = calendarPlanCreator;
            _calendarPlanWriter = calendarPlanWriter;
            _webHostEnvironment = webHostEnvironment;
        }

        public Estimate GetEstimate(IEnumerable<IFormFile> estimateFiles)
        {
            _estimateService.ReadEstimateFiles(estimateFiles);
            return _estimateService.Estimate;
        }

        public IEnumerable<decimal> GetTotalPercentages(IEnumerable<IFormFile> estimateFiles, List<UserWork> userWorks)
        {
            var calendarPlan = CalculateCalendarPlan(estimateFiles, userWorks);
            return calendarPlan.MainCalendarWorks.Find(x => x.EstimateChapter == 10).ConstructionPeriod.ConstructionMonths.Select(x => x.PercentPart);
        }

        public void WriteCalendarPlan(IEnumerable<IFormFile> estimateFiles, List<UserWork> userWorks, string userFullName)
        {
            var calendarPlan = CalculateCalendarPlan(estimateFiles, userWorks);
            var ceilingDuration = decimal.Ceiling(_estimateService.Estimate.ConstructionDuration);
            var savePath = GetCalendarPlansPath();
            var templatePath = Path.Combine(_webHostEnvironment.WebRootPath, _calendarPlanTemplatesPath, $"CalendarPlan{ceilingDuration}MonthsTemplate.docx");
            var fileName = GetCalendarPlanFileName(userFullName);
            _calendarPlanWriter.Write(calendarPlan, templatePath, savePath, fileName);
        }

        private CalendarPlan CalculateCalendarPlan(IEnumerable<IFormFile> estimateFiles, List<UserWork> userWorks)
        {
            _estimateService.ReadEstimateFiles(estimateFiles);

            var otherExpensesWork = userWorks.Find(x => x.WorkName == CalendarWorkCreator.MainOtherExpensesWorkName);
            userWorks.Remove(otherExpensesWork);

            SetEstimatePercenatages(userWorks);

            var calendarPlan = _calendarPlanCreator.Create(_estimateService.Estimate, otherExpensesWork.Percentages);

            return calendarPlan;
        }

        private void SetEstimatePercenatages(List<UserWork> userWorks)
        {
            userWorks.ForEach(userWork =>
            {
                _estimateService.Estimate.MainEstimateWorks.Find(estimateWork => estimateWork.WorkName == userWork.WorkName).Percentages = userWork.Percentages;
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

        public string GetDownloadCalendarPlanFileName()
        {
            return $"{_estimateService.Estimate.ObjectCipher}КП.docx";
        }
    }
}
