using System.Collections.Generic;
using System.IO;
using System.Linq;
using BGTGWeb.Helpers;
using BGTGWeb.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using POS.CalendarPlanLogic.Interfaces;
using POS.EnergyAndWaterLogic.Interfaces;
using POS.EstimateLogic;

namespace BGTGWeb.Services
{
    public class EnergyAndWaterService : IEnergyAndWaterService
    {
        private readonly IEstimateService _estimateService;
        private readonly IEnergyAndWaterCreator _energyAndWaterCreator;
        private readonly IEnergyAndWaterWriter _energyAndWaterWriter;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICalendarWorkCreator _calendarWorkCreator;

        private const string _energyAndWaterTemplateFileName = "EnergyAndWaterTemplate.docx";
        private const string _energyAndWaterTemplatePath = "Templates\\EnergyAndWaterTemplates";
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
            _estimateService.ReadEstimateFiles(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter);
            var mainTotalCostIncludingCAIW = GetMainTotalCostIncludingCAIW();
            var energyAndWater = _energyAndWaterCreator.Create(mainTotalCostIncludingCAIW, _estimateService.Estimate.ConstructionStartDate.Year);
            var templatePath = Path.Combine(_webHostEnvironment.WebRootPath, _energyAndWaterTemplatePath, _energyAndWaterTemplateFileName);
            var savePath = Path.Combine(GetEnergyAndWatersPath(), GetEnergyAndWaterFileName(userFullName));
            _energyAndWaterWriter.Write(energyAndWater, templatePath, savePath);
        }

        private decimal GetMainTotalCostIncludingCAIW()
        {
            var totalEstimateWork = _estimateService.Estimate.MainEstimateWorks.Single(x => x.Chapter == 12);
            var totalCalendarWork = _calendarWorkCreator.Create(totalEstimateWork, _estimateService.Estimate.ConstructionStartDate);
            return totalCalendarWork.TotalCostIncludingCAIW;
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
