using Moq;
using POS.Infrastructure.Appenders.Base;

namespace POS.Tests.Helpers.Appenders
{
    public class DurationByTCPAppenderHelper
    {
        public static Mock<IDurationByTCPAppender> GetMock()
        {
            return new Mock<IDurationByTCPAppender>();
        }
    }
}
