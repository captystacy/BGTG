using System.IO;
using BGTG.POS.EnergyAndWaterTool;
using NUnit.Framework;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace BGTG.POS.Tests.EnergyAndWaterTool
{
    public class EnergyAndWaterWriterTests
    {
        private const string EnergyAndWaterTemplateFileName = "EnergyAndWaterTemplate.docx";
        private const string EnergyAndWaterFileName = "EnergyAndWater.docx";
        private const string EnergyAndWaterTemplatesDirectory = @"..\..\..\EnergyAndWaterTool\EnergyAndWaterTemplates";

        private EnergyAndWaterWriter _energyAndWaterWriter;

        [SetUp]
        public void SetUp()
        {
            _energyAndWaterWriter = new EnergyAndWaterWriter();
        }

        private EnergyAndWater CreateDefaultEnergyAndWater()
        {
            return new EnergyAndWater(2021, 1.293M, 2.65M, 0.004M, 0.05M, 56.882M);
        }

        [Test]
        public void Write_EneryAndWater_SaveCorrectEnergyAndWater()
        {
            var expectedEnergyAndWater = CreateDefaultEnergyAndWater();
            var templatePath = Path.Combine(EnergyAndWaterTemplatesDirectory, EnergyAndWaterTemplateFileName);
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), EnergyAndWaterFileName);

            _energyAndWaterWriter.Write(expectedEnergyAndWater, templatePath, savePath);

            using (var document = DocX.Load(savePath))
            {
                var energyAndWaterRow = document.Tables[0].Rows[2];
                var actualEnergyAndWater = ParseEnergyAndWater(energyAndWaterRow);

                Assert.AreEqual(expectedEnergyAndWater, actualEnergyAndWater);
            }
        }

        private EnergyAndWater ParseEnergyAndWater(Row energyAndWaterRow)
        {
            var constructionYear = int.Parse(energyAndWaterRow.Paragraphs[0].Text);
            var vonumeCAIW = decimal.Parse(energyAndWaterRow.Paragraphs[1].Text);
            var energy = decimal.Parse(energyAndWaterRow.Paragraphs[2].Text);
            var water = decimal.Parse(energyAndWaterRow.Paragraphs[3].Text);
            var compressedAir = decimal.Parse(energyAndWaterRow.Paragraphs[4].Text);
            var oxygen = decimal.Parse(energyAndWaterRow.Paragraphs[5].Text);
            return new EnergyAndWater(constructionYear, vonumeCAIW, energy, water, compressedAir, oxygen);
        }
    }
}
