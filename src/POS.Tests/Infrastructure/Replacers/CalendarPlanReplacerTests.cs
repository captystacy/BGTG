using Moq;
using POS.Infrastructure.Replacers;
using POS.Tests.Helpers.Services.DocumentServices.WordService;
using Xunit;

namespace POS.Tests.Infrastructure.Replacers
{
    public class CalendarPlanReplacerTests
    {
        [Fact]
        public void ItShould_replace_calendar_plan_patterns()
        {
            // arrange

            var baseDocument = MyWordDocumentHelper.GetMock();
            var preparatory = MyTableHelper.GetMock();
            var main = MyTableHelper.GetMock();

            var sut = new CalendarPlanReplacer();

            // act

            sut.Replace(baseDocument.Object, preparatory.Object, main.Object);

            // assert

            baseDocument.Verify(x => x.ReplaceTextWithTable("%CALENDAR_PLAN_PREPARATORY_TABLE%", preparatory.Object), Times.Once);
            baseDocument.Verify(x => x.ReplaceTextWithTable("%CALENDAR_PLAN_MAIN_TABLE%", main.Object), Times.Once);
        }
    }
}
