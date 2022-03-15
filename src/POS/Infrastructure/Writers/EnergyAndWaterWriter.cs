using POS.DomainModels;
using POS.DomainModels.DocumentDomainModels;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Writers.Base;
using System.Globalization;
using POS.Infrastructure.Services;

namespace POS.Infrastructure.Writers;

public class EnergyAndWaterWriter : IEnergyAndWaterWriter
{
    private const string ConstructionYearPattern = "%CY%";
    private const string VolumeCaiwPattern = "%CAIWV%";
    private const string EnergyPattern = "%E%";
    private const string WaterPattern = "%W%";
    private const string CompressedAirPattern = "%CA%";
    private const string OxygenPattern = "%O%";

    public MemoryStream Write(EnergyAndWater energyAndWater, string templatePath)
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

        using var document = DocumentService.Load(templatePath);
        var energyAndWaterTable = document.Tables[0];
        ModifyEnergyAndWaterTable(energyAndWaterTable.Rows[2], energyAndWater);

        var memoryStream = new MemoryStream();
        document.SaveAs(memoryStream);

        return memoryStream;
    }

    private void ModifyEnergyAndWaterTable(MyRow row, EnergyAndWater energyAndWater)
    {
        row.ReplaceText(ConstructionYearPattern, energyAndWater.ConstructionYear.ToString());
        row.ReplaceText(VolumeCaiwPattern, energyAndWater.VolumeCAIW.ToString(AppConstants.DecimalThreePlacesFormat));
        row.ReplaceText(EnergyPattern, energyAndWater.Energy.ToString(AppConstants.DecimalThreePlacesFormat));
        row.ReplaceText(WaterPattern, energyAndWater.Water.ToString(AppConstants.DecimalThreePlacesFormat));
        row.ReplaceText(CompressedAirPattern,
            energyAndWater.CompressedAir.ToString(AppConstants.DecimalThreePlacesFormat));
        row.ReplaceText(OxygenPattern, energyAndWater.Oxygen.ToString(AppConstants.DecimalThreePlacesFormat));
    }
}