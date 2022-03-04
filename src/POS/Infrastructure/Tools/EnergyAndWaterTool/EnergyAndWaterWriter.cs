using System.Globalization;
using POS.Infrastructure.Tools.EnergyAndWaterTool.Base;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace POS.Infrastructure.Tools.EnergyAndWaterTool;

public class EnergyAndWaterWriter : IEnergyAndWaterWriter
{
    private const string ConstructionYearPattern = "%CY%";
    private const string VolumeCaiwPattern = "%CAIWV%";
    private const string EnergyPattern = "%E%";
    private const string WaterPattern = "%W%";
    private const string CompressedAirPattern = "%CA%";
    private const string OxygenPattern = "%O%";

    public void Write(EnergyAndWater energyAndWater, string templatePath)
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

        using var document = DocX.Load(templatePath);
        var energyAndWaterTable = document.Tables[0];
        ModifyEnergyAndWaterTable(energyAndWaterTable.Rows[2], energyAndWater);
    }

    private void ModifyEnergyAndWaterTable(Row row, EnergyAndWater energyAndWater)
    {
        row.ReplaceText(ConstructionYearPattern, energyAndWater.ConstructionYear.ToString());
        row.ReplaceText(VolumeCaiwPattern, energyAndWater.VolumeCAIW.ToString(AppData.DecimalThreePlacesFormat));
        row.ReplaceText(EnergyPattern, energyAndWater.Energy.ToString(AppData.DecimalThreePlacesFormat));
        row.ReplaceText(WaterPattern, energyAndWater.Water.ToString(AppData.DecimalThreePlacesFormat));
        row.ReplaceText(CompressedAirPattern, energyAndWater.CompressedAir.ToString(AppData.DecimalThreePlacesFormat));
        row.ReplaceText(OxygenPattern, energyAndWater.Oxygen.ToString(AppData.DecimalThreePlacesFormat));
    }
}