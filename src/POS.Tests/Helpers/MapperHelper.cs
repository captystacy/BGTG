using AutoMapper;
using Moq;

namespace POS.Tests.Helpers
{
    public static class MapperHelper
    {
        public static Mock<IMapper> GetMock()
        {
            return new Mock<IMapper>();
        }
    }
}
