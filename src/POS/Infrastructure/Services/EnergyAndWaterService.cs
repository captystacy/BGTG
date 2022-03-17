using POS.DomainModels.EstimateDomainModels;
using POS.Infrastructure.Creators.Base;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;
using POS.ViewModels;

namespace POS.Infrastructure.Services;

public class EnergyAndWaterService : IEnergyAndWaterService
{
    private readonly IEstimateService _estimateService;
    private readonly IEnergyAndWaterCreator _energyAndWaterCreator;
    private readonly IEnergyAndWaterWriter _energyAndWaterWriter;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ICalendarWorkCreator _calendarWorkCreator;

    private const string EnergyAndWaterTemplateFileName = "EnergyAndWater.docx";
    private const string TemplatesPath = @"Infrastructure\Templates\EnergyAndWaterTemplates";

    public EnergyAndWaterService(IEstimateService estimateService, IEnergyAndWaterCreator energyAndWaterCreator,
        IEnergyAndWaterWriter energyAndWaterWriter, IWebHostEnvironment webHostEnvironment, ICalendarWorkCreator calendarWorkCreator)
    {
        _estimateService = estimateService;
        _energyAndWaterCreator = energyAndWaterCreator;
        _energyAndWaterWriter = energyAndWaterWriter;
        _webHostEnvironment = webHostEnvironment;
        _calendarWorkCreator = calendarWorkCreator;
    }

    public MemoryStream Write(EnergyAndWaterViewModel viewModel)
    {
        _estimateService.Read(viewModel.EstimateFiles, TotalWorkChapter.TotalWork1To12Chapter);

        var mainTotalCostIncludingCAIW = GetMainTotalCostIncludingCAIW();

        var energyAndWater = _energyAndWaterCreator.Create(mainTotalCostIncludingCAIW, _estimateService.Estimate.ConstructionStartDate.Year);

        var templatePath = GetTemplatePath();

        return _energyAndWaterWriter.Write(energyAndWater, templatePath);
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
}