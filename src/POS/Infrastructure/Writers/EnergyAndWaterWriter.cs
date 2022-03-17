using System.Globalization;
using POS.DomainModels;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;

namespace POS.Infrastructure.Writers;

public class EnergyAndWaterWriter : IEnergyAndWaterWriter
{
    private readonly IWordDocumentService _wordDocumentService;

    private const string ConstructionYearPattern = "%CY%";
    private const string VolumeCaiwPattern = "%CAIWV%";
    private const string EnergyPattern = "%E%";
    private const string WaterPattern = "%W%";
    private const string CompressedAirPattern = "%CA%";
    private const string OxygenPattern = "%O%";
    private const int TargetRowIndex = 2;

    public EnergyAndWaterWriter(IWordDocumentService wordDocumentService)
    {
        _wordDocumentService = wordDocumentService;
    }

    public MemoryStream Write(EnergyAndWater energyAndWater, string templatePath)
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

        _wordDocumentService.Load(templatePath);
        ModifyEnergyAndWaterTable(energyAndWater);

        var memoryStream = new MemoryStream();
        _wordDocumentService.SaveAs(memoryStream);
        _wordDocumentService.DisposeLastDocument();

        return memoryStream;
    }

    private void ModifyEnergyAndWaterTable(EnergyAndWater energyAndWater)
    {
        _wordDocumentService.RowIndex = TargetRowIndex;
        _wordDocumentService.ReplaceTextInDocument(ConstructionYearPattern, energyAndWater.ConstructionYear.ToString());
        _wordDocumentService.ReplaceTextInDocument(VolumeCaiwPattern, energyAndWater.VolumeCAIW.ToString(AppConstants.DecimalThreePlacesFormat));
        _wordDocumentService.ReplaceTextInDocument(EnergyPattern, energyAndWater.Energy.ToString(AppConstants.DecimalThreePlacesFormat));
        _wordDocumentService.ReplaceTextInDocument(WaterPattern, energyAndWater.Water.ToString(AppConstants.DecimalThreePlacesFormat));
        _wordDocumentService.ReplaceTextInDocument(CompressedAirPattern, energyAndWater.CompressedAir.ToString(AppConstants.DecimalThreePlacesFormat));
        _wordDocumentService.ReplaceTextInDocument(OxygenPattern, energyAndWater.Oxygen.ToString(AppConstants.DecimalThreePlacesFormat));
    }
}