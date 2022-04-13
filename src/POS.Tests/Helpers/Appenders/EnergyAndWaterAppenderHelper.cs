using Moq;
using POS.Infrastructure.Appenders.Base;

namespace POS.Tests.Helpers.Appenders
{
    public static class EnergyAndWaterAppenderHelper
    {
        public static Mock<IEnergyAndWaterAppender> GetMock()
        {
            return new Mock<IEnergyAndWaterAppender>();
        }
    }
}
