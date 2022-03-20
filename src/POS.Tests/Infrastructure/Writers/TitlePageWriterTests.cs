using System;
using Moq;
using NUnit.Framework;
using POS.DomainModels;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers;

namespace POS.Tests.Infrastructure.Writers;

public class TitlePageWriterTests
{
    private TitlePageWriter _titlePageWriter = null!;
    private Mock<IWordDocumentService> _documentServiceMock = null!;

    [SetUp]
    public void SetUp()
    {
        _documentServiceMock = new Mock<IWordDocumentService>();
        _titlePageWriter = new TitlePageWriter(_documentServiceMock.Object);
    }

    [Test]
    public void Write_ConstructionObject548()
    {
        var templatePath = "Saiko.docx";
        var objectName = "Электроснабжение станции катодной защиты (СКЗ)№36 аг.Снов Несвижского района";
        var objectCipher = "5.5-20.548";

        var memoryStream = _titlePageWriter.Write(objectCipher, objectName, templatePath);

        _documentServiceMock.Verify(x => x.Load(templatePath), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%NAME%", objectName), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%CIPHER%", objectCipher), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%YEAR%", DateTime.Now.Year.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.SaveAs(memoryStream, MyFileFormat.Doc, 0), Times.Once);
        _documentServiceMock.Verify(x => x.DisposeLastDocument(), Times.Once);
        Assert.NotNull(memoryStream);
    }
}