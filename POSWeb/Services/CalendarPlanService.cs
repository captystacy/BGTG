using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using POSCore.CalendarPlanLogic;
using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;
using POSCore.EstimateLogic.Interfaces;
using POSWeb.Models;
using POSWeb.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace POSWeb.Services
{
    public class CalendarPlanService : ICalendarPlanService
    {
        private readonly IEstimateReader _estimateReader;
        private readonly IEstimateConnector _estimateConnector;
        private readonly ICalendarPlanCreator _calendarPlanCreator;
        private readonly ICalendarPlanWriter _calendarPlanWriter;
        private readonly ICalendarPlanSeparator _calendarPlanSeparator;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public const string DocxMimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

        private const string _calendarPlansPath = "UsersFiles\\CalendarPlans";
        private const string _calendarPlanTemplatesPath = "Templates\\CalendarPlanTemplates";

        public CalendarPlanService(IEstimateReader estimateReader, IEstimateConnector estimateConnector,
            ICalendarPlanCreator calendarPlanCreator, ICalendarPlanSeparator calendarPlanSeparator,
            ICalendarPlanWriter calendarPlanWriter, IWebHostEnvironment webHostEnvironment)
        {
            _estimateReader = estimateReader;
            _estimateConnector = estimateConnector;
            _calendarPlanCreator = calendarPlanCreator;
            _calendarPlanSeparator = calendarPlanSeparator;
            _calendarPlanWriter = calendarPlanWriter;
            _webHostEnvironment = webHostEnvironment;
        }

        public Estimate GetEstimate(IEnumerable<IFormFile> estimateFiles)
        {
            var estimates = estimateFiles.Select(x => _estimateReader.Read(x.OpenReadStream()));
            return _estimateConnector.Connect(estimates.ToList());
        }

        public CalendarWork GetMainTotalWork(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM)
        {
            CalculateCalendarPlan(estimateFiles, calendarPlanVM);
            return _calendarPlanSeparator.MainCalendarPlan.CalendarWorks.Find(x => x.EstimateChapter == 10);
        }

        public void WriteCalendarPlan(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM, string fileName)
        {
            CalculateCalendarPlan(estimateFiles, calendarPlanVM);
            var ceilingDuration = decimal.Ceiling(calendarPlanVM.ConstructionDuration);
            var savePath = GetCalendarPlansPath();
            var templatePath = Path.Combine(_webHostEnvironment.WebRootPath, _calendarPlanTemplatesPath, $"CalendarPlan{ceilingDuration}MonthsTemplate.docx");
            _calendarPlanWriter.Write(_calendarPlanSeparator.PreparatoryCalendarPlan, _calendarPlanSeparator.MainCalendarPlan,
                templatePath, savePath, fileName);
        }

        private void CalculateCalendarPlan(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM)
        {
            foreach (var userWork in calendarPlanVM.UserWorks)
            {
                userWork.Percentages = userWork.Percentages.Select(x => x / 100).ToList();
            }

            var otherExpensesWork = calendarPlanVM.UserWorks.Find(x => x.WorkName == CalendarPlanSeparator.MainOtherExpensesWork);
            calendarPlanVM.UserWorks.Remove(otherExpensesWork);

            var estimate = PrepareEstimate(estimateFiles, calendarPlanVM.UserWorks);

            var calendarPlan = _calendarPlanCreator.Create(estimate);

            _calendarPlanSeparator.Separate(calendarPlan, otherExpensesWork.Percentages);
        }

        private Estimate PrepareEstimate(IEnumerable<IFormFile> estimateFiles, List<UserWork> userWorks)
        {
            var estimate = GetEstimate(estimateFiles);

            foreach (var preparatoryEstimateWork in estimate.EstimateWorks.FindAll(x => x.Chapter == 1 || x.WorkName.StartsWith(CalendarPlanSeparator.TemporaryBuildingsSearchPattern)))
            {
                preparatoryEstimateWork.Percentages = new List<decimal>() { 1 };
            }

            foreach (var userWork in userWorks)
            {
                var estimateWork = estimate.EstimateWorks.Find(x => x.WorkName == userWork.WorkName);
                estimateWork.Percentages = userWork.Percentages;
            }

            var totalEstimateWork = estimate.EstimateWorks.Find(x => x.WorkName == CalendarPlanSeparator.TotalWorkSearchPattern);
            totalEstimateWork.Percentages = new List<decimal>();

            return estimate;
        }

        public string GetCalendarPlansPath()
        {
            return Path.Combine(_webHostEnvironment.WebRootPath, _calendarPlansPath);
        }

        public string GetDownloadCalendarPlanName(string objectCipher)
        {
            return $"{objectCipher}КП.docx";
        }
    }
}
