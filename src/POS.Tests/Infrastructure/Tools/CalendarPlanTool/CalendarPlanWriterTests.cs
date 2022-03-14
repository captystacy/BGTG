using System.IO;
using Moq;
using NUnit.Framework;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Tools.CalendarPlanTool;

namespace POS.Tests.Infrastructure.Tools.CalendarPlanTool;

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
    public void Write_CalendarPlan548VAT_SaveCorrectCalendarPlan()
    {
        var preparatoryTemplatePath = "Preparatory.docx";
        var mainTemplatePath = "Main1.docx";

        _documentServiceMock.Setup(x => x.GetRowCount()).Returns(5);

        _documentServiceMock.Setup(x => x.GetParagraphsCount(4)).Returns(2);

        var memoryStream = _calendarPlanWriter.Write(CalendarPlanSource.CalendarPlan548, preparatoryTemplatePath, mainTemplatePath);

        _documentServiceMock.Verify(x => x.Load(preparatoryTemplatePath), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText(1, "%D0%", "Август 2022"), Times.Exactly(2));

        _documentServiceMock.Verify(x => x.InsertTemplateRow(2, 4), Times.Exactly(7));

        _documentServiceMock.Verify(x => x.ReplaceText(4, "%WN%", "Временные здания и сооружения"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText(4, "%WN%", "Итого:"), Times.Exactly(2));
        _documentServiceMock.Verify(x => x.ReplaceText(4, "%WN%", "Работы, выполняемые в подготовительный период"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText(4, "%WN%", "Электрохимическая защита"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText(4, "%WN%", "Благоустройство территории"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText(4, "%WN%", "Прочие работы и затраты"), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceText(4, "%TC%", "0,017"), Times.Exactly(3));
        _documentServiceMock.Verify(x => x.ReplaceText(4, "%TC%", "0,632"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText(4, "%TC%", "0,020"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText(4, "%TC%", "2,557"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText(4, "%TC%", "3,226"), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceText(4, "%TIC%", "0,017"), Times.Exactly(3));
        _documentServiceMock.Verify(x => x.ReplaceText(4, "%TIC%", "0,592"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText(4, "%TIC%", "0,020"), Times.Exactly(2));
        _documentServiceMock.Verify(x => x.ReplaceText(4, "%TIC%", "0,649"), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceText(4, "%IV0%", "0,017"), Times.Exactly(3));
        _documentServiceMock.Verify(x => x.ReplaceText(4, "%IV0%", "0,632"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText(4, "%IV0%", "0,020"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText(4, "%IV0%", "2,557"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText(4, "%IV0%", "3,226"), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceText(4, "%IW0%", "0,017"), Times.Exactly(3));
        _documentServiceMock.Verify(x => x.ReplaceText(4, "%IW0%", "0,592"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText(4, "%IW0%", "0,020"), Times.Exactly(2));
        _documentServiceMock.Verify(x => x.ReplaceText(4, "%IW0%", "0,649"), Times.Once);

        _documentServiceMock.Verify(x => x.RemoveRow(3), Times.Exactly(2));
        _documentServiceMock.Verify(x => x.RemoveRow(2), Times.Exactly(2));

        _documentServiceMock.Verify(x => x.ReplaceText(4, "%P0%", "100,00 %"), Times.Exactly(2));

        _documentServiceMock.Verify(x => x.InsertDocument(0, 1), Times.Once);

        _documentServiceMock.Verify(x => x.SaveAs(It.IsAny<Stream>(), 0), Times.Once);
        _documentServiceMock.Verify(x => x.Dispose(), Times.Once);
        Assert.NotNull(memoryStream);
    }
}