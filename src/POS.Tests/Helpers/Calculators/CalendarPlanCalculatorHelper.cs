using Moq;
using POS.Infrastructure.Calculators.Base;

namespace POS.Tests.Helpers.Calculators
{
    public static class CalendarPlanCalculatorHelper
    {
        public static Mock<ICalendarPlanCalculator> GetMock()
        {
            return new Mock<ICalendarPlanCalculator>();
        }
    }
}
