using Moq;
using POS.Infrastructure.Parsers.Base;

namespace POS.Tests.Helpers.Parsers
{
    public static class EstimateParserHelper
    {
        public static Mock<IEstimateParser> GetMock()
        {
            return new Mock<IEstimateParser>();
        }
    }
}
