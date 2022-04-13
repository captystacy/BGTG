using System.IO;
using Microsoft.AspNetCore.Http;
using Moq;

namespace POS.Tests.Helpers
{
    public static class FormFileHelper
    {
        public static Mock<IFormFile> GetMock()
        {
            return new Mock<IFormFile>();
        }

        public static Mock<IFormFile> GetMock(Mock<Stream> stream)
        {
            var formFile = GetMock();

            formFile.Setup(x => x.OpenReadStream()).Returns(stream.Object);

            return formFile;
        }

        public static Mock<IFormFile> GetMock(Mock<Stream> stream, string fileName)
        {
            var formFile = GetMock(stream);

            formFile.Setup(x => x.FileName).Returns(fileName);

            return formFile;
        }
    }
}
