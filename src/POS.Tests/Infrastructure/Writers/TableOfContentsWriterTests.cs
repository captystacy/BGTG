using Moq;
using NUnit.Framework;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers;
using System;

namespace POS.Tests.Infrastructure.Writers;

public class TableOfContentsWriterTests
{
    private TableOfContentsWriter _tableOfContentsWriter = null!;
    private Mock<IWordDocumentService> _documentServiceMock = null!;

    [SetUp]
    public void SetUp()
    {
        _documentServiceMock = new Mock<IWordDocumentService>();
        _tableOfContentsWriter = new TableOfContentsWriter(_documentServiceMock.Object);
    }

    [Test]
    public void Write_ConstructionObject548()
    {
        var templatePath = "Kapitan.docx";
        var objectCipher = "5.5-20.548";

        var memoryStream = _tableOfContentsWriter.Write(objectCipher, templatePath);

        _documentServiceMock.Verify(x => x.Load(templatePath), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%CIPHER%", objectCipher), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%DATE%", DateTime.Now.ToString(AppConstants.DateTimeMonthAndYearShortFormat)), Times.Once);

        _documentServiceMock.Verify(x => x.SaveAs(memoryStream, 0), Times.Once);
        _documentServiceMock.Verify(x => x.DisposeLastDocument(), Times.Once);

        Assert.NotNull(memoryStream);
    }
}