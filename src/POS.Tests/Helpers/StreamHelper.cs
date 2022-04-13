using System.IO;
using Moq;

namespace POS.Tests.Helpers
{
    public static class StreamHelper
    {
        public static Mock<Stream> GetMock()
        {
            var stream = new Mock<Stream>();

            stream.SetupGet(x => x.Length).Returns(1);

            return stream;
        }
    }
}