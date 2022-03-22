using Moq;
using NUnit.Framework;
using POS.DomainModels;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers;

namespace POS.Tests.Infrastructure.Writers;

public class CalendarPlanWriterTests
{
    private CalendarPlanWriter _calendarPlanWriter = null!;
    private Mock<IWordDocumentService> _documentServiceMock = null!;

    [SetUp]
    public void SetUp()
    {
        _documentServiceMock = new Mock<IWordDocumentService>();
        _calendarPlanWriter = new CalendarPlanWriter(_documentServiceMock.Object);
    }

    [Test]
    public void Write_CalendarPlan548VAT()
    {
        var calendarPlanTemplate = "CaldendarPlanTemplate.docx";
        var preparatoryTemplatePath = "Preparatory.docx";
        var mainTemplatePath = "Main1.docx";

        _documentServiceMock.SetupGet(x => x.CellsCountInRow).Returns(2); // for checking ReplaceTextInRow("%P0%", "100,00 %")

        var memoryStream = _calendarPlanWriter.Write(CalendarPlanSource.CalendarPlan548, calendarPlanTemplate, preparatoryTemplatePath, mainTemplatePath);

        _documentServiceMock.Verify(x => x.Load(calendarPlanTemplate), Times.Once);
        _documentServiceMock.Verify(x => x.Load(preparatoryTemplatePath), Times.Once);
        _documentServiceMock.Verify(x => x.Load(mainTemplatePath), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%D0%", "Август 2022"), Times.Exactly(2));

        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%WN%", "Временные здания и сооружения"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%WN%", "Итого:"), Times.Exactly(2));
        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%WN%", "Работы, выполняемые в подготовительный период"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%WN%", "Электрохимическая защита"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%WN%", "Благоустройство территории"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%WN%", "Прочие работы и затраты"), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%TC%", "0,017"), Times.Exactly(3));
        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%TC%", "0,632"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%TC%", "0,020"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%TC%", "2,557"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%TC%", "3,226"), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%TIC%", "0,017"), Times.Exactly(3));
        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%TIC%", "0,592"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%TIC%", "0,020"), Times.Exactly(2));
        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%TIC%", "0,649"), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%IV0%", "0,017"), Times.Exactly(3));
        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%IV0%", "0,632"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%IV0%", "0,020"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%IV0%", "2,557"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%IV0%", "3,226"), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%IW0%", "0,017"), Times.Exactly(3));
        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%IW0%", "0,592"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%IW0%", "0,020"), Times.Exactly(2));
        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%IW0%", "0,649"), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceTextInCell("%P0%", "100,00 %"), Times.Exactly(2));

        _documentServiceMock.Verify(x => x.SaveAs(memoryStream, MyFileFormat.DocX, 0), Times.Once);
        _documentServiceMock.Verify(x => x.DisposeLastDocument(), Times.Exactly(3));
        Assert.NotNull(memoryStream);
    }
}