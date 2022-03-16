using System.IO;
using Moq;
using NUnit.Framework;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers;

namespace POS.Tests.Infrastructure.Writers;

public class CalendarPlanWriterTests
{
    private CalendarPlanWriter _calendarPlanWriter = null!;
    private Mock<IDocumentService> _documentServiceMock = null!;

    [SetUp]
    public void SetUp()
    {
        _documentServiceMock = new Mock<IDocumentService>();
        _calendarPlanWriter = new CalendarPlanWriter(_documentServiceMock.Object);
    }

    [Test]
    public void Write_CalendarPlan548VAT()
    {
        var preparatoryTemplatePath = "Preparatory.docx";
        var mainTemplatePath = "Main1.docx";

        _documentServiceMock.SetupGet(x => x.ParagraphsCountInRow).Returns(2); // for checking ReplaceTextInRow("%P0%", "100,00 %")

        var memoryStream = _calendarPlanWriter.Write(CalendarPlanSource.CalendarPlan548, preparatoryTemplatePath, mainTemplatePath);

        _documentServiceMock.Verify(x => x.Load(preparatoryTemplatePath), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%D0%", "Август 2022"), Times.Exactly(2));

        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%WN%", "Временные здания и сооружения"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%WN%", "Итого:"), Times.Exactly(2));
        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%WN%", "Работы, выполняемые в подготовительный период"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%WN%", "Электрохимическая защита"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%WN%", "Благоустройство территории"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%WN%", "Прочие работы и затраты"), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%TC%", "0,017"), Times.Exactly(3));
        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%TC%", "0,632"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%TC%", "0,020"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%TC%", "2,557"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%TC%", "3,226"), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%TIC%", "0,017"), Times.Exactly(3));
        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%TIC%", "0,592"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%TIC%", "0,020"), Times.Exactly(2));
        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%TIC%", "0,649"), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%IV0%", "0,017"), Times.Exactly(3));
        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%IV0%", "0,632"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%IV0%", "0,020"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%IV0%", "2,557"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%IV0%", "3,226"), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%IW0%", "0,017"), Times.Exactly(3));
        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%IW0%", "0,592"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%IW0%", "0,020"), Times.Exactly(2));
        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%IW0%", "0,649"), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceTextInRow("%P0%", "100,00 %"), Times.Exactly(2));

        _documentServiceMock.Verify(x => x.InsertDocument(0, 1), Times.Once);
        _documentServiceMock.Verify(x => x.SaveAs(memoryStream, 0), Times.Once);
        _documentServiceMock.Verify(x => x.DisposeAllDocuments(), Times.Once);
        Assert.NotNull(memoryStream);
    }
}