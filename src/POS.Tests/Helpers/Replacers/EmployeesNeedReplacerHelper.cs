using Moq;
using POS.Infrastructure.Replacers.Base;

namespace POS.Tests.Helpers.Replacers
{
    public static class EmployeesNeedReplacerHelper
    {
        public static Mock<IEmployeesNeedReplacer> GetMock()
        {
            return new Mock<IEmployeesNeedReplacer>();
        }
    }
}
