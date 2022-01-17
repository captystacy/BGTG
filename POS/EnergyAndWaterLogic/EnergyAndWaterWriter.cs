using POS.EnergyAndWaterLogic.Interfaces;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace POS.EnergyAndWaterLogic
{
    public class EnergyAndWaterWriter : IEnergyAndWaterWriter
    {
        private const string _constructionYearPattern = "%CY%";
        private const string _volumeCAIWPattern = "%CAIWV%";
        private const string _energyPattern = "%E%";
        private const string _waterPattern = "%W%";
        private const string _compressedAirPattern = "%CA%";
        private const string _oxygenPattern = "%O%";

        public void Write(EnergyAndWater energyAndWater, string templatePath, string savePath)
        {
            using (var document = DocX.Load(templatePath))
            {
                var energyAndWaterTable = document.Tables[0];
                ModifyEnergyAndWaterTable(energyAndWaterTable.Rows[2], energyAndWater);

                document.SaveAs(savePath);
            }
        }

        private void ModifyEnergyAndWaterTable(Row row, EnergyAndWater energyAndWater)
        {
            row.ReplaceText(_constructionYearPattern, energyAndWater.ConstructionYear.ToString());
            row.ReplaceText(_volumeCAIWPattern, energyAndWater.VolumeCAIW.ToString());
            row.ReplaceText(_energyPattern, energyAndWater.Energy.ToString());
            row.ReplaceText(_waterPattern, energyAndWater.Water.ToString());
            row.ReplaceText(_compressedAirPattern, energyAndWater.CompressedAir.ToString());
            row.ReplaceText(_oxygenPattern, energyAndWater.Oxygen.ToString());
        }
    }
}
