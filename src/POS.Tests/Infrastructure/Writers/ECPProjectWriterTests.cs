using Moq;
using NUnit.Framework;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers;
using System;
using System.IO;

namespace POS.Tests.Infrastructure.Writers;

public class ECPProjectWriterTests
{
    private ECPProjectWriter _ecpProjectWriter = null!;
    private Mock<IDocumentService> _documentServiceMock = null!;

    [SetUp]
    public void SetUp()
    {
        _documentServiceMock = new Mock<IDocumentService>();
        _ecpProjectWriter = new ECPProjectWriter(_documentServiceMock.Object);
    }

    [Test]
    public void Write()
    {
        var objectCipher = "5.5-20.548";
        var durationByLCStream = new MemoryStream();
        var calendarPlanStream = new MemoryStream();
        var energyAndWaterStream = new MemoryStream();
        var templatePath = "HouseholdTown+.docx";

        var paragraphTextInDocument = "Принимаем продолжительность строительства равную 0,6 мес, в том числе подготовительный период – 0,06 мес, приемка объекта в эксплуатацию – 0,5 мес.";
        _documentServiceMock.Setup(x => x.ParagraphTextInDocument).Returns(paragraphTextInDocument);
        _documentServiceMock.Setup(x => x.ParagraphsCountInDocument).Returns(35);
        var constructionStartDate = "август 2022";
        _documentServiceMock.Setup(x => x.ParagraphTextInRow).Returns(constructionStartDate);

        var memoryStream = _ecpProjectWriter.Write(durationByLCStream, calendarPlanStream, energyAndWaterStream,
            objectCipher, templatePath);

        _documentServiceMock.Verify(x => x.Load(templatePath), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%CIPHER%", objectCipher), Times.Once);
        _documentServiceMock.Verify(
            x => x.ReplaceTextInDocument("%DATE%", DateTime.Now.ToString(AppConstants.DateTimeMonthAndYearShortFormat)),
            Times.Once);

        // duration by lc
        _documentServiceMock.Verify(x => x.Load(durationByLCStream), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%DURATION_BY_LC_FIRST_PARAGRAPH%", paragraphTextInDocument), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextWithTable("%DURATION_BY_LC_TABLE%"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextWithTable("%DURATION_BY_LC_DESCRIPTION_TABLE%"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%DURATION_BY_LC_PENULTIMATE_PARAGRAPH%", paragraphTextInDocument), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%DURATION_BY_LC_LAST_PARAGRAPH%", paragraphTextInDocument), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%TD%", "0,6"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%PP%", "0,06"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%AT%", "0,5"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%TLC%", constructionStartDate), Times.Once);

        // calendar plan
        _documentServiceMock.Verify(x => x.Load(calendarPlanStream), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceTextWithTable("%CALENDAR_PLAN_PREPARATORY_TABLE%"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextWithTable("%CALENDAR_PLAN_MAIN_TABLE%"), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%CONSTRUCTION_START_DATE%", constructionStartDate), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%CY%", "2022"), Times.Once);

        // energy and water
        _documentServiceMock.Verify(x => x.Load(energyAndWaterStream), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceTextWithTable("%ENERGY_AND_WATER_TABLE%"), Times.Once);

        _documentServiceMock.Verify(x => x.SaveAs(memoryStream, 0), Times.Once);
        _documentServiceMock.Verify(x => x.DisposeLastDocument(), Times.Exactly(4));
        Assert.NotNull(memoryStream);
    }
}