using Moq;
using POS.Infrastructure.Replacers.Base;

namespace POS.Tests.Helpers.Replacers
{
    public class ProjectReplacerHelper
    {
        public static Mock<IProjectReplacer> GetMock()
        {
            return new Mock<IProjectReplacer>();
        }
    }
}
