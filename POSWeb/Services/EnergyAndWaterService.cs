using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using POSCore.CalendarPlanLogic;
using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EnergyAndWaterLogic.Interfaces;
using POSCore.EstimateLogic;
using POSWeb.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace POSWeb.Services
{
    public class EnergyAndWaterService : IEnergyAndWaterService
    {
        private readonly ICalendarPlanService _calendarPlanService;
        private readonly IEnergyAndWaterCreator _energyAndWaterCreator;
        private readonly IEnergyAndWaterWriter _energyAndWaterWriter;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICalendarWorkCreator _calendarWorkCreator;

        private const string _energyAndWaterTemplateFileName = "EnergyAndWaterTemplate.docx";
        private const string _energyAndWaterTemplatePath = "Templates";
        private const string _energyAndWatersPath = "UsersFiles\\EnergyAndWaters";

        public EnergyAndWaterService(ICalendarPlanService calendarPlanService, IEnergyAndWaterCreator energyAndWaterCreator, 
            IEnergyAndWaterWriter energyAndWaterWriter, IWebHostEnvironment webHostEnvironment, ICalendarWorkCreator calendarWorkCreator)
        {
            _calendarPlanService = calendarPlanService;
            _energyAndWaterCreator = energyAndWaterCreator;
            _energyAndWaterWriter = energyAndWaterWriter;
            _webHostEnvironment = webHostEnvironment;
            _calendarWorkCreator = calendarWorkCreator;
        }

        public void WriteEnergyAndWater(IEnumerable<IFormFile> estimateFiles, DateTime constructionStartDate, string fileName)
        {
            var mainTotalWork = GetMainTotalWork(estimateFiles, constructionStartDate);
            var energyAndWater = _energyAndWaterCreator.Create(mainTotalWork.TotalCostIncludingContructionAndInstallationWorks, constructionStartDate.Year);
            var templatePath = Path.Combine(_webHostEnvironment.WebRootPath, _energyAndWaterTemplatePath, _energyAndWaterTemplateFileName);
            var savePath = Path.Combine(_webHostEnvironment.WebRootPath, _energyAndWatersPath);
            _energyAndWaterWriter.Write(energyAndWater, templatePath, savePath, fileName);
        }

        private CalendarWork GetMainTotalWork(IEnumerable<IFormFile> estimateFiles, DateTime constructionStartDate)
        {
            var estimate = _calendarPlanService.GetEstimate(estimateFiles);
            var totalEstimateWork = estimate.EstimateWorks.Find(x => x.Chapter == 10);
            return _calendarWorkCreator.Create(constructionStartDate, totalEstimateWork);
        }

        public string GetEnergyAndWatersPath()
        {
            return Path.Combine(_webHostEnvironment.WebRootPath, _energyAndWatersPath);
        }

        public string GetDownloadEnergyAndWaterName(string objectCipher)
        {
            return $"{objectCipher}ЭИВ.docx";
        }

        public Estimate GetEstimate(IEnumerable<IFormFile> estimateFiles)
        {
            return _calendarPlanService.GetEstimate(estimateFiles);
        }
    }
}
