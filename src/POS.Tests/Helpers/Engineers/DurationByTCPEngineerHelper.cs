using Moq;
using POS.Infrastructure.Engineers;

namespace POS.Tests.Helpers.Engineers
{
    public static class DurationByTCPEngineerHelper
    {
        public static Mock<IDurationByTCPEngineer> GetMock()
        {
            return new Mock<IDurationByTCPEngineer>();
        }
    }
}