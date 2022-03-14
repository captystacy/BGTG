using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Tools.EnergyAndWaterTool.Base;
using System.Globalization;

namespace POS.Infrastructure.Tools.EnergyAndWaterTool;

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
        _documentService.Dispose();

        return memoryStream;
    }

    private void ModifyEnergyAndWaterTable(EnergyAndWater energyAndWater)
    {
        _documentService.ReplaceText(TargetRowIndex, ConstructionYearPattern, energyAndWater.ConstructionYear.ToString());
        _documentService.ReplaceText(TargetRowIndex, VolumeCaiwPattern, energyAndWater.VolumeCAIW.ToString(AppData.DecimalThreePlacesFormat));
        _documentService.ReplaceText(TargetRowIndex, EnergyPattern, energyAndWater.Energy.ToString(AppData.DecimalThreePlacesFormat));
        _documentService.ReplaceText(TargetRowIndex, WaterPattern, energyAndWater.Water.ToString(AppData.DecimalThreePlacesFormat));
        _documentService.ReplaceText(TargetRowIndex, CompressedAirPattern, energyAndWater.CompressedAir.ToString(AppData.DecimalThreePlacesFormat));
        _documentService.ReplaceText(TargetRowIndex, OxygenPattern, energyAndWater.Oxygen.ToString(AppData.DecimalThreePlacesFormat));
    }
}