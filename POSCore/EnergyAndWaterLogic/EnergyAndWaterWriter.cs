using POSCore.EnergyAndWaterLogic.Interfaces;
using System.IO;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace POSCore.EnergyAndWaterLogic
{
    public class EnergyAndWaterWriter : IEnergyAndWaterWriter
    {
        private const string _constructionYearPattern = "CY";
        private const string _smrVolumePattern = "SV";
        private const string _energyPattern = "E";
        private const string _waterPattern = "W";
        private const string _compressedAirPattern = "CA";
        private const string _oxygenPattern = "O";

        public void Write(EnergyAndWater energyAndWater, string templatePath, string savePath, string fileName)
        {
            using (var document = DocX.Load(templatePath))
            {
                var energyAndWaterTable = document.Tables[0];
                ModifyEnergyAndWaterTable(energyAndWaterTable.Rows[2], energyAndWater);

                var saveAsPath = Path.Combine(savePath, fileName);
                document.SaveAs(saveAsPath);
            }
        }

        private void ModifyEnergyAndWaterTable(Row row, EnergyAndWater energyAndWater)
        {
            row.ReplaceText(_constructionYearPattern, energyAndWater.ConstructionYear.ToString());
            row.ReplaceText(_smrVolumePattern, decimal.Round(energyAndWater.SmrVolume, 3).ToString());
            row.ReplaceText(_energyPattern, decimal.Round(energyAndWater.Energy, 3).ToString());
            row.ReplaceText(_waterPattern, decimal.Round(energyAndWater.Water, 3).ToString());
            row.ReplaceText(_compressedAirPattern, decimal.Round(energyAndWater.CompressedAir, 3).ToString());
            row.ReplaceText(_oxygenPattern, decimal.Round(energyAndWater.Oxygen, 3).ToString());
        }
    }
}
