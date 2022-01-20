using POS.EnergyAndWaterLogic.Interfaces;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace POS.EnergyAndWaterLogic
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
            using var document = DocX.Load(templatePath);
            var energyAndWaterTable = document.Tables[0];
            ModifyEnergyAndWaterTable(energyAndWaterTable.Rows[2], energyAndWater);

            document.SaveAs(savePath);
        }

        private void ModifyEnergyAndWaterTable(Row row, EnergyAndWater energyAndWater)
        {
            row.ReplaceText(ConstructionYearPattern, energyAndWater.ConstructionYear.ToString());
            row.ReplaceText(VolumeCaiwPattern, energyAndWater.VolumeCAIW.ToString());
            row.ReplaceText(EnergyPattern, energyAndWater.Energy.ToString());
            row.ReplaceText(WaterPattern, energyAndWater.Water.ToString());
            row.ReplaceText(CompressedAirPattern, energyAndWater.CompressedAir.ToString());
            row.ReplaceText(OxygenPattern, energyAndWater.Oxygen.ToString());
        }
    }
}
