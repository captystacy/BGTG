using Moq;
using NUnit.Framework;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Tools.EnergyAndWaterTool;
using System.IO;

namespace POS.Tests.Infrastructure.Tools.EnergyAndWaterTool;

public class EnergyAndWaterWriterTests
{
    private EnergyAndWaterWriter _energyAndWaterWriter = null!;
    private Mock<IDocumentService> _documentServiceMock = null!;

    [SetUp]
    public void SetUp()
    {
        _documentServiceMock = new Mock<IDocumentService>();
        _energyAndWaterWriter = new EnergyAndWaterWriter(_documentServiceMock.Object);
    }

    [Test]
    public void Write_EnergyAndWater_SaveCorrectEnergyAndWater()
    {
        var energyAndWater = new EnergyAndWater(2021, 1.293M, 2.65M, 0.004M, 0.05M, 56.882M);
        var templatePath = "EnergyAndWater.docx";

        var memoryStream = _energyAndWaterWriter.Write(energyAndWater, templatePath);
        var targetRowIndex = 2;
        _documentServiceMock.Verify(x => x.Load(templatePath));
        _documentServiceMock.Verify(x => x.ReplaceText(targetRowIndex, "%CY%", energyAndWater.ConstructionYear.ToString()));
        _documentServiceMock.Verify(x => x.ReplaceText(targetRowIndex, "%CAIWV%", energyAndWater.VolumeCAIW.ToString(AppData.DecimalThreePlacesFormat)));
        _documentServiceMock.Verify(x => x.ReplaceText(targetRowIndex, "%E%", energyAndWater.Energy.ToString(AppData.DecimalThreePlacesFormat)));
        _documentServiceMock.Verify(x => x.ReplaceText(targetRowIndex, "%W%", energyAndWater.Water.ToString(AppData.DecimalThreePlacesFormat)));
        _documentServiceMock.Verify(x => x.ReplaceText(targetRowIndex, "%CA%", energyAndWater.CompressedAir.ToString(AppData.DecimalThreePlacesFormat)));
        _documentServiceMock.Verify(x => x.ReplaceText(targetRowIndex, "%O%", energyAndWater.Oxygen.ToString(AppData.DecimalThreePlacesFormat)));
        _documentServiceMock.Verify(x => x.SaveAs(It.IsAny<Stream>(), 0), Times.Once);
        _documentServiceMock.Verify(x => x.Dispose(), Times.Once);
        Assert.NotNull(memoryStream);
    }
}