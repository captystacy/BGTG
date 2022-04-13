using Moq;
using POS.Infrastructure.Rounders;

namespace POS.Tests.Helpers.Rounders
{
    public static class DurationRounderHelper
    {
        public static Mock<IDurationRounder> GetMock()
        {
            return new Mock<IDurationRounder>();
        }
    }
}
