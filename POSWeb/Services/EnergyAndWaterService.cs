using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EnergyAndWaterLogic.Interfaces;
using POSWeb.Helpers;
using POSWeb.Services.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace POSWeb.Services
{
    public class EnergyAndWaterService : IEnergyAndWaterService
    {
        private readonly IEstimateService _estimateService;
        private readonly IEnergyAndWaterCreator _energyAndWaterCreator;
        private readonly IEnergyAndWaterWriter _energyAndWaterWriter;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICalendarWorkCreator _calendarWorkCreator;

        private const string _energyAndWaterTemplateFileName = "EnergyAndWaterTemplate.docx";
        private const string _energyAndWaterTemplatePath = "Templates";
        private const string _energyAndWatersPath = "UsersFiles\\EnergyAndWaters";

        public EnergyAndWaterService(IEstimateService estimateService, IEnergyAndWaterCreator energyAndWaterCreator, 
            IEnergyAndWaterWriter energyAndWaterWriter, IWebHostEnvironment webHostEnvironment, ICalendarWorkCreator calendarWorkCreator)
        {
            _estimateService = estimateService;
            _energyAndWaterCreator = energyAndWaterCreator;
            _energyAndWaterWriter = energyAndWaterWriter;
            _webHostEnvironment = webHostEnvironment;
            _calendarWorkCreator = calendarWorkCreator;
        }

        public void WriteEnergyAndWater(IEnumerable<IFormFile> estimateFiles, string userFullName)
        {
            _estimateService.ReadEstimateFiles(estimateFiles);
            var mainTotalCostIncludingContructionAndInstallationWorks = GetMainTotalCostIncludingContructionAndInstallationWorks();
            var energyAndWater = _energyAndWaterCreator.Create(mainTotalCostIncludingContructionAndInstallationWorks, _estimateService.Estimate.ConstructionStartDate.Year);
            var templatePath = Path.Combine(_webHostEnvironment.WebRootPath, _energyAndWaterTemplatePath, _energyAndWaterTemplateFileName);
            var savePath = GetEnergyAndWatersPath();
            var fileName = GetEnergyAndWaterFileName(userFullName);
            _energyAndWaterWriter.Write(energyAndWater, templatePath, savePath, fileName);
        }

        private decimal GetMainTotalCostIncludingContructionAndInstallationWorks()
        {
            var totalEstimateWork = _estimateService.Estimate.MainEstimateWorks.Find(x => x.Chapter == 10);
            totalEstimateWork.Percentages = new List<decimal>();
            var totalCalendarWork = _calendarWorkCreator.Create(totalEstimateWork, _estimateService.Estimate.ConstructionStartDate);
            return totalCalendarWork.TotalCostIncludingContructionAndInstallationWorks;
        }

        public string GetEnergyAndWatersPath()
        {
            return Path.Combine(_webHostEnvironment.WebRootPath, _energyAndWatersPath);
        }

        public string GetEnergyAndWaterFileName(string userFullName)
        {
            return $"EnergyAndWater{userFullName.RemoveBackslashes()}.docx";
        }

        public string GetDownloadEnergyAndWaterFileName()
        {
            return $"{_estimateService.Estimate.ObjectCipher}ЭИВ.docx";
        }
    }
}
