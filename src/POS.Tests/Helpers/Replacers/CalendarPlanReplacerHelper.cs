using Moq;
using POS.Infrastructure.Replacers.Base;

namespace POS.Tests.Helpers.Replacers
{
    public static class CalendarPlanReplacerHelper
    {
        public static Mock<ICalendarPlanReplacer> GetMock()
        {
            return new Mock<ICalendarPlanReplacer>();
        }
    }
}
