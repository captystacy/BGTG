using Moq;
using POS.Infrastructure.Appenders.Base;

namespace POS.Tests.Helpers.Appenders
{
    public static class CalendarPlanAppenderHelper
    {
        public static Mock<ICalendarPlanAppender> GetMock()
        {
            return new Mock<ICalendarPlanAppender>();
        }
    }
}
