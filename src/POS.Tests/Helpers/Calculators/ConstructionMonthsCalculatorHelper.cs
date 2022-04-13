using Moq;
using POS.Infrastructure.Calculators.Base;

namespace POS.Tests.Helpers.Calculators
{
    public static class ConstructionMonthsCalculatorHelper
    {
        public static Mock<IConstructionMonthsCalculator> GetMock()
        {
            return new Mock<IConstructionMonthsCalculator>();
        }
    }
}
