using Microsoft.AspNetCore.Hosting;
using Moq;

namespace POS.Tests.Helpers
{
    public static class WebHostEnvironmentHelper
    {
        public static Mock<IWebHostEnvironment> GetMock()
        {
            return new Mock<IWebHostEnvironment>();
        }

        public static Mock<IWebHostEnvironment> GetMock(string contentRootPath)
        {
            var webHostEnvironment = GetMock();

            webHostEnvironment.Setup(x => x.ContentRootPath).Returns(contentRootPath);

            return webHostEnvironment;
        }
    }
}
