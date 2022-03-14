using Moq;
using NUnit.Framework;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Tools.DurationTools.DurationByLCTool;
using System.IO;

namespace POS.Tests.Infrastructure.Tools.DurationTools.DurationByLCTool;

public class DurationByLCWriterTests
{
    private DurationByLCWriter _durationByLCWriter = null!;
    private Mock<IDocumentService> _documentServiceMock = null!;

    [SetUp]
    public void SetUp()
    {
        _documentServiceMock = new Mock<IDocumentService>();
        _durationByLCWriter = new DurationByLCWriter(_documentServiceMock.Object);
    }

    [Test]
    public void Write_RoundingPlusAcceptancePlus_SaveCorrectDurationByLC()
    {
        var durationByLC = new DurationByLC(1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 11111, 22222, 33333, true, true);
        var templatePath = "Rounding+Acceptance+.docx";

        var memoryStream = _durationByLCWriter.Write(durationByLC, templatePath);

        _documentServiceMock.Verify(x => x.Load(templatePath), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%TLC%", " (трудозатраты по сметам и трудозатраты по технологической карте)"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%NOWDIM%", "7000"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%NOE%", "8000"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%WDD%", "5000"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%RD%", "22222"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%TD%", "9000"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%AT%", "33333"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%PP%", "11111"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%S%", "6000"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%D%", "1000"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%LC%", "2000"), Times.Once);

        _documentServiceMock.Verify(x => x.SaveAs(It.IsAny<Stream>(), 0), Times.Once);
        _documentServiceMock.Verify(x => x.Dispose(), Times.Once);
        Assert.NotNull(memoryStream);
    }
}