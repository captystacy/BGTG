using Moq;
using POS.Infrastructure.Replacers.Base;

namespace POS.Tests.Helpers.Replacers
{
    public static class EnergyAndWaterReplacerHelper
    {
        public static Mock<IEnergyAndWaterReplacer> GetMock()
        {
            return new Mock<IEnergyAndWaterReplacer>();
        }
    }
}
