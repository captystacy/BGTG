using System.IO;
using Moq;
using NUnit.Framework;
using POS.DomainModels;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers;

namespace POS.Tests.Infrastructure.Writers;

public class EnergyAndWaterWriterTests
{
    private EnergyAndWaterWriter _energyAndWaterWriter = null!;
    private Mock<IWordDocumentService> _documentServiceMock = null!;

    [SetUp]
    public void SetUp()
    {
        _documentServiceMock = new Mock<IWordDocumentService>();
        _energyAndWaterWriter = new EnergyAndWaterWriter(_documentServiceMock.Object);
    }

    [Test]
    public void Write_EnergyAndWater_SaveCorrectEnergyAndWater()
    {
        var energyAndWater = new EnergyAndWater(2021, 1.293M, 2.65M, 0.004M, 0.05M, 56.882M);
        var templatePath = "EnergyAndWater.docx";

        var memoryStream = _energyAndWaterWriter.Write(energyAndWater, templatePath);
        _documentServiceMock.Verify(x => x.Load(templatePath));
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%CY%", energyAndWater.ConstructionYear.ToString()));
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%CAIWV%", energyAndWater.VolumeCAIW.ToString(AppConstants.DecimalThreePlacesFormat)));
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%E%", energyAndWater.Energy.ToString(AppConstants.DecimalThreePlacesFormat)));
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%W%", energyAndWater.Water.ToString(AppConstants.DecimalThreePlacesFormat)));
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%CA%", energyAndWater.CompressedAir.ToString(AppConstants.DecimalThreePlacesFormat)));
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%O%", energyAndWater.Oxygen.ToString(AppConstants.DecimalThreePlacesFormat)));
        _documentServiceMock.Verify(x => x.SaveAs(memoryStream, MyFileFormat.DocX, 0), Times.Once);
        _documentServiceMock.Verify(x => x.DisposeLastDocument(), Times.Once);
        Assert.NotNull(memoryStream);
    }
}