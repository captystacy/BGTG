using System.IO;
using System.Linq;
using BGTG.POS.Tools.CalendarPlanTool.Interfaces;
using BGTG.POS.Tools.EnergyAndWaterTool.Interfaces;
using BGTG.POS.Tools.EstimateTool;
using BGTG.POS.Web.Infrastructure.Helpers;
using BGTG.POS.Web.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BGTG.POS.Web.Infrastructure.Services
{
    public class EnergyAndWaterService : IEnergyAndWaterService
    {
        private readonly IEstimateService _estimateService;
        private readonly IEnergyAndWaterCreator _energyAndWaterCreator;
        private readonly IEnergyAndWaterWriter _energyAndWaterWriter;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICalendarWorkCreator _calendarWorkCreator;

        private const string EnergyAndWaterTemplateFileName = "EnergyAndWaterTemplate.docx";
        private const string EnergyAndWaterTemplatePath = @"AppData\Templates\EnergyAndWaterTemplates";
        private const string EnergyAndWatersPath = @"AppData\UserFiles\EnergyAndWaterFiles";

        public EnergyAndWaterService(IEstimateService estimateService, IEnergyAndWaterCreator energyAndWaterCreator, 
            IEnergyAndWaterWriter energyAndWaterWriter, IWebHostEnvironment webHostEnvironment, ICalendarWorkCreator calendarWorkCreator)
        {
            _estimateService = estimateService;
            _energyAndWaterCreator = energyAndWaterCreator;
            _energyAndWaterWriter = energyAndWaterWriter;
            _webHostEnvironment = webHostEnvironment;
            _calendarWorkCreator = calendarWorkCreator;
        }

        public void Write(IFormFileCollection estimateFiles, string userFullName)
        {
            _estimateService.Read(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter);

            var mainTotalCostIncludingCAIW = GetMainTotalCostIncludingCAIW();

            var energyAndWater = _energyAndWaterCreator.Create(mainTotalCostIncludingCAIW, _estimateService.Estimate.ConstructionStartDate.Year);

            var templatePath = GetTemplatePath();

            var savePath = GetSavePath(userFullName);

            _energyAndWaterWriter.Write(energyAndWater, templatePath, savePath);
        }

        private decimal GetMainTotalCostIncludingCAIW()
        {
            var totalEstimateWork = _estimateService.Estimate.MainEstimateWorks.Single(x => x.Chapter == 12);
            var totalCalendarWork = _calendarWorkCreator.Create(totalEstimateWork, _estimateService.Estimate.ConstructionStartDate);
            return totalCalendarWork.TotalCostIncludingCAIW;
        }

        private string GetTemplatePath()
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, EnergyAndWaterTemplatePath, EnergyAndWaterTemplateFileName);
        }

        public string GetSavePath(string userFullName)
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, EnergyAndWatersPath, $"EnergyAndWater{userFullName.RemoveBackslashes()}.docx");
        }

        public string GetFileName(string objectCipher = null)
        {
            return $"{objectCipher ?? _estimateService.Estimate.ObjectCipher}ЭИВ.docx";
        }
    }
}
