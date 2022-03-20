using System.IO;
using Moq;
using NUnit.Framework;
using POS.DomainModels;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers;

namespace POS.Tests.Infrastructure.Writers;

public class DurationByLCWriterTests
{
    private DurationByLCWriter _durationByLCWriter = null!;
    private Mock<IWordDocumentService> _documentServiceMock = null!;

    [SetUp]
    public void SetUp()
    {
        _documentServiceMock = new Mock<IWordDocumentService>();
        _durationByLCWriter = new DurationByLCWriter(_documentServiceMock.Object);
    }

    [Test]
    public void Write_RoundingPlusAcceptancePlus_SaveCorrectDurationByLC()
    {
        var durationByLC = new DurationByLC(1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 11111, 22222, 33333, true, true);
        var templatePath = "Rounding+Acceptance+.docx";

        var memoryStream = _durationByLCWriter.Write(durationByLC, templatePath);

        _documentServiceMock.Verify(x => x.Load(templatePath), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%TLC%", " (трудозатраты по сметам и трудозатраты по технологической карте)"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%NOWDIM%", "7000"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%NOE%", "8000"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%WDD%", "5000"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%RD%", "22222"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%TD%", "9000"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%AT%", "33333"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%PP%", "11111"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%S%", "6000"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%D%", "1000"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%LC%", "2000"), Times.Once);

        _documentServiceMock.Verify(x => x.SaveAs(memoryStream, MyFileFormat.DocX, 0), Times.Once);
        _documentServiceMock.Verify(x => x.DisposeLastDocument(), Times.Once);
        Assert.NotNull(memoryStream);
    }
}