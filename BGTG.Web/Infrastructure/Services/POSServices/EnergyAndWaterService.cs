using System.IO;
using System.Linq;
using BGTG.POS.CalendarPlanTool.Interfaces;
using BGTG.POS.EnergyAndWaterTool;
using BGTG.POS.EnergyAndWaterTool.Interfaces;
using BGTG.POS.EstimateTool;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.Interfaces;
using BGTG.Web.Infrastructure.Services.POSServices.Interfaces;
using BGTG.Web.ViewModels.POSViewModels.EnergyAndWaterViewModels;
using Microsoft.AspNetCore.Hosting;

namespace BGTG.Web.Infrastructure.Services.POSServices
{
    public class EnergyAndWaterService : IEnergyAndWaterService
    {
        private readonly IEstimateService _estimateService;
        private readonly IEnergyAndWaterCreator _energyAndWaterCreator;
        private readonly IEnergyAndWaterWriter _energyAndWaterWriter;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICalendarWorkCreator _calendarWorkCreator;

        private const string EnergyAndWaterTemplateFileName = "EnergyAndWaterTemplate.docx";
        private const string TemplatesPath = @"AppData\Templates\EnergyAndWaterTemplates";
        private const string UserFilesPath = @"AppData\UserFiles\EnergyAndWaterFiles";

        public EnergyAndWaterService(IEstimateService estimateService, IEnergyAndWaterCreator energyAndWaterCreator,
            IEnergyAndWaterWriter energyAndWaterWriter, IWebHostEnvironment webHostEnvironment, ICalendarWorkCreator calendarWorkCreator)
        {
            _estimateService = estimateService;
            _energyAndWaterCreator = energyAndWaterCreator;
            _energyAndWaterWriter = energyAndWaterWriter;
            _webHostEnvironment = webHostEnvironment;
            _calendarWorkCreator = calendarWorkCreator;
        }

        public EnergyAndWater Write(EnergyAndWaterCreateViewModel viewModel, string windowsName)
        {
            _estimateService.Read(viewModel.EstimateFiles, TotalWorkChapter.TotalWork1To12Chapter);

            var mainTotalCostIncludingCAIW = GetMainTotalCostIncludingCAIW();

            var energyAndWater = _energyAndWaterCreator.Create(mainTotalCostIncludingCAIW, _estimateService.Estimate.ConstructionStartDate.Year);

            return Write(energyAndWater, windowsName);
        }

        public EnergyAndWater Write(EnergyAndWater energyAndWater, string windowsName)
        {
            var templatePath = GetTemplatePath();

            var savePath = GetSavePath(windowsName);

            _energyAndWaterWriter.Write(energyAndWater, templatePath, savePath);

            return energyAndWater;
        }

        private decimal GetMainTotalCostIncludingCAIW()
        {
            var totalEstimateWork = _estimateService.Estimate.MainEstimateWorks.Single(x => x.Chapter == 12);
            var totalCalendarWork = _calendarWorkCreator.Create(totalEstimateWork, _estimateService.Estimate.ConstructionStartDate);
            return totalCalendarWork.TotalCostIncludingCAIW;
        }

        private string GetTemplatePath()
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath, EnergyAndWaterTemplateFileName);
        }

        public string GetSavePath(string windowsName)
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, UserFilesPath, $"EnergyAndWater{windowsName.RemoveBackslashes()}.docx");
        }
    }
}
