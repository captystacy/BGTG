using System.IO;
using NUnit.Framework;
using POS.Infrastructure.Tools.EnergyAndWaterTool;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace POS.Tests.Infrastructure.Tools.EnergyAndWaterTool;

public class EnergyAndWaterWriterTests
{
    private const string EnergyAndWaterTemplateFileName = "EnergyAndWater.docx";
    private const string EnergyAndWaterTemplatesDirectory = @"..\..\..\Infrastructure\Tools\EnergyAndWaterTool\EnergyAndWaterTemplates";

    private EnergyAndWaterWriter _energyAndWaterWriter = null!;

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
    public void Write_EnergyAndWater_SaveCorrectEnergyAndWater()
    {
        var expectedEnergyAndWater = CreateDefaultEnergyAndWater();
        var templatePath = Path.Combine(EnergyAndWaterTemplatesDirectory, EnergyAndWaterTemplateFileName);

        var memoryStream = _energyAndWaterWriter.Write(expectedEnergyAndWater, templatePath);

        using var document = DocX.Load(memoryStream);
        var energyAndWaterRow = document.Tables[0].Rows[2];
        var actualEnergyAndWater = ParseEnergyAndWater(energyAndWaterRow);

        Assert.AreEqual(expectedEnergyAndWater, actualEnergyAndWater);
    }

    private EnergyAndWater ParseEnergyAndWater(Row energyAndWaterRow)
    {
        var constructionYear = int.Parse(energyAndWaterRow.Paragraphs[0].Text);
        var volumeCAIW = decimal.Parse(energyAndWaterRow.Paragraphs[1].Text);
        var energy = decimal.Parse(energyAndWaterRow.Paragraphs[2].Text);
        var water = decimal.Parse(energyAndWaterRow.Paragraphs[3].Text);
        var compressedAir = decimal.Parse(energyAndWaterRow.Paragraphs[4].Text);
        var oxygen = decimal.Parse(energyAndWaterRow.Paragraphs[5].Text);
        return new EnergyAndWater(constructionYear, volumeCAIW, energy, water, compressedAir, oxygen);
    }
}