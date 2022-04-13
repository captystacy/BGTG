using Moq;
using POS.Infrastructure.Replacers.Base;

namespace POS.Tests.Helpers.Replacers
{
    public static class DurationByLCReplacerHelper
    {
        public static Mock<IDurationByLCReplacer> GetMock()
        {
            return new Mock<IDurationByLCReplacer>();
        }
    }
}
