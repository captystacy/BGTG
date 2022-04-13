using Moq;
using POS.Infrastructure.Replacers.Base;

namespace POS.Tests.Helpers.Replacers
{
    public static class TechnicalAndEconomicalIndicatorsReplacerHelper
    {
        public static Mock<ITechnicalAndEconomicalIndicatorsReplacer> GetMock()
        {
            return new Mock<ITechnicalAndEconomicalIndicatorsReplacer>();
        }
    }
}
