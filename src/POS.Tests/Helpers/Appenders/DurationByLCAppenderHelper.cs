using Moq;
using POS.Infrastructure.Appenders.Base;

namespace POS.Tests.Helpers.Appenders
{
    public static class DurationByLCAppenderHelper
    {
        public static Mock<IDurationByLCAppender> GetMock()
        {
            return new Mock<IDurationByLCAppender>();
        }
    }
}
