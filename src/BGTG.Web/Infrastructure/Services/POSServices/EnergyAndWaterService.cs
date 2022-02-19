using System.IO;
using System.Linq;
using BGTG.POS.CalendarPlanTool.Base;
using BGTG.POS.EnergyAndWaterTool;
using BGTG.POS.EnergyAndWaterTool.Base;
using BGTG.POS.EstimateTool;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
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

        private const string EnergyAndWaterTemplateFileName = "EnergyAndWater.docx";
        private const string TemplatesPath = @"AppData\Templates\POSTemplates\EnergyAndWaterTemplates";
        private const string UserFilesPath = @"AppData\UserFiles\POSFiles\EnergyAndWaterFiles";

        public EnergyAndWaterService(IEstimateService estimateService, IEnergyAndWaterCreator energyAndWaterCreator,
            IEnergyAndWaterWriter energyAndWaterWriter, IWebHostEnvironment webHostEnvironment, ICalendarWorkCreator calendarWorkCreator)
        {
            _estimateService = estimateService;
            _energyAndWaterCreator = energyAndWaterCreator;
            _energyAndWaterWriter = energyAndWaterWriter;
            _webHostEnvironment = webHostEnvironment;
            _calendarWorkCreator = calendarWorkCreator;
        }

        public EnergyAndWater Write(EnergyAndWaterCreateViewModel viewModel)
        {
            _estimateService.Read(viewModel.EstimateFiles, TotalWorkChapter.TotalWork1To12Chapter);

            var mainTotalCostIncludingCAIW = GetMainTotalCostIncludingCAIW();

            var energyAndWater = _energyAndWaterCreator.Create(mainTotalCostIncludingCAIW, _estimateService.Estimate.ConstructionStartDate.Year);

            return Write(energyAndWater);
        }

        public EnergyAndWater Write(EnergyAndWater energyAndWater)
        {
            var templatePath = GetTemplatePath();

            var savePath = GetSavePath();

            _energyAndWaterWriter.Write(energyAndWater, templatePath, savePath);

            return energyAndWater;
        }

        private decimal GetMainTotalCostIncludingCAIW()
        {
            var totalEstimateWork = _estimateService.Estimate.MainEstimateWorks.First(x => x.Chapter == 12);
            var totalCalendarWork = _calendarWorkCreator.Create(totalEstimateWork, _estimateService.Estimate.ConstructionStartDate);
            return totalCalendarWork.TotalCostIncludingCAIW;
        }

        private string GetTemplatePath()
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath, EnergyAndWaterTemplateFileName);
        }

        public string GetSavePath()
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, UserFilesPath, $"{IdentityHelper.Instance.User!.Name!.RemoveBackslashes()}.docx");
        }
    }
}
