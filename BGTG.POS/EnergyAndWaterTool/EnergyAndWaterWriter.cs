using System.Globalization;
using BGTG.Core;
using BGTG.POS.EnergyAndWaterTool.Interfaces;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace BGTG.POS.EnergyAndWaterTool
{
    public class EnergyAndWaterWriter : IEnergyAndWaterWriter
    {
        private const string ConstructionYearPattern = "%CY%";
        private const string VolumeCaiwPattern = "%CAIWV%";
        private const string EnergyPattern = "%E%";
        private const string WaterPattern = "%W%";
        private const string CompressedAirPattern = "%CA%";
        private const string OxygenPattern = "%O%";

        public void Write(EnergyAndWater energyAndWater, string templatePath, string savePath)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

            using var document = DocX.Load(templatePath);
            var energyAndWaterTable = document.Tables[0];
            ModifyEnergyAndWaterTable(energyAndWaterTable.Rows[2], energyAndWater);

            document.SaveAs(savePath);
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
}
