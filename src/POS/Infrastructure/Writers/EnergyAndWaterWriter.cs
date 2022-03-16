using System.Globalization;
using POS.DomainModels;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;

namespace POS.Infrastructure.Writers;

public class EnergyAndWaterWriter : IEnergyAndWaterWriter
{
    private readonly IDocumentService _documentService;

    private const string ConstructionYearPattern = "%CY%";
    private const string VolumeCaiwPattern = "%CAIWV%";
    private const string EnergyPattern = "%E%";
    private const string WaterPattern = "%W%";
    private const string CompressedAirPattern = "%CA%";
    private const string OxygenPattern = "%O%";
    private const int TargetRowIndex = 2;

    public EnergyAndWaterWriter(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    public MemoryStream Write(EnergyAndWater energyAndWater, string templatePath)
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

        _documentService.Load(templatePath);
        ModifyEnergyAndWaterTable(energyAndWater);

        var memoryStream = new MemoryStream();
        _documentService.SaveAs(memoryStream);
        _documentService.DisposeLastDocument();

        return memoryStream;
    }

    private void ModifyEnergyAndWaterTable(EnergyAndWater energyAndWater)
    {
        _documentService.RowIndex = TargetRowIndex;
        _documentService.ReplaceTextInDocument(ConstructionYearPattern, energyAndWater.ConstructionYear.ToString());
        _documentService.ReplaceTextInDocument(VolumeCaiwPattern, energyAndWater.VolumeCAIW.ToString(AppConstants.DecimalThreePlacesFormat));
        _documentService.ReplaceTextInDocument(EnergyPattern, energyAndWater.Energy.ToString(AppConstants.DecimalThreePlacesFormat));
        _documentService.ReplaceTextInDocument(WaterPattern, energyAndWater.Water.ToString(AppConstants.DecimalThreePlacesFormat));
        _documentService.ReplaceTextInDocument(CompressedAirPattern, energyAndWater.CompressedAir.ToString(AppConstants.DecimalThreePlacesFormat));
        _documentService.ReplaceTextInDocument(OxygenPattern, energyAndWater.Oxygen.ToString(AppConstants.DecimalThreePlacesFormat));
    }
}