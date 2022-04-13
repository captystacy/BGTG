using Moq;
using POS.Infrastructure.Replacers.Base;

namespace POS.Tests.Helpers.Replacers
{
    public static class EngineerReplacerHelper
    {
        public static Mock<IEngineerReplacer> GetMock()
        {
            return new Mock<IEngineerReplacer>();
        }
    }
}
