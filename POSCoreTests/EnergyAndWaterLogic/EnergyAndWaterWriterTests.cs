using NUnit.Framework;
using POSCore.EnergyAndWaterLogic;
using System.IO;
using Xceed.Words.NET;

namespace POSCoreTests.EnergyAndWaterLogic
{
    public class EnergyAndWaterWriterTests
    {
        public EnergyAndWaterWriter CreateDefaultEnergyAndWaterWriter()
        {
            return new EnergyAndWaterWriter();
        }

        private EnergyAndWater CreateDefaultEnergyAndWater()
        {
            return new EnergyAndWater(2021, 1.293M, 2.65M, 0.004M, 0.05M, 56.882M);
        }

        [Test]
        public void Write_EneryAndWater_SaveCorrectEnergyAndWater()
        {
            var energyAndWaterWriter = CreateDefaultEnergyAndWaterWriter();
            var energyAndWater = CreateDefaultEnergyAndWater();
            var templatePath = @"..\..\..\EnergyAndWaterLogic\EnergyAndWaterTemplate.docx";

            var energyAndWaterFileName = "EnergyAndWater.docx";
            energyAndWaterWriter.Write(energyAndWater, templatePath, Directory.GetCurrentDirectory(), energyAndWaterFileName);

            var energyAndWaterPath = Path.Combine(Directory.GetCurrentDirectory(), energyAndWaterFileName);
            using (var document = DocX.Load(energyAndWaterPath))
            {
                var energyAndWaterTable = document.Tables[0];
                var energyAndWaterRow = energyAndWaterTable.Rows[2];

                var constructionYear = int.Parse(energyAndWaterRow.Paragraphs[0].Text);
                var smrVolume = decimal.Parse(energyAndWaterRow.Paragraphs[1].Text);
                var energy = decimal.Parse(energyAndWaterRow.Paragraphs[2].Text);
                var water = decimal.Parse(energyAndWaterRow.Paragraphs[3].Text);
                var compressedAir = decimal.Parse(energyAndWaterRow.Paragraphs[4].Text);
                var oxygen = decimal.Parse(energyAndWaterRow.Paragraphs[5].Text);

                Assert.AreEqual(energyAndWater.ConstructionYear, constructionYear);
                Assert.AreEqual(energyAndWater.SmrVolume, smrVolume);
                Assert.AreEqual(energyAndWater.Energy, energy);
                Assert.AreEqual(energyAndWater.Water, water);
                Assert.AreEqual(energyAndWater.CompressedAir, compressedAir);
                Assert.AreEqual(energyAndWater.Oxygen, oxygen);
            }
        }
    }
}
