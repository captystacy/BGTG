using Moq;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;

namespace POS.Tests.Helpers.Services.DocumentServices.WordService
{
    public static class MyWordDocumentHelper
    {
        public static Mock<IMyWordDocument> GetMock()
        {
            return new Mock<IMyWordDocument>();
        }

        public static Mock<IMyWordDocument> GetMock(Mock<IMySection> section)
        {
            var document = new Mock<IMyWordDocument>();

            document.Setup(x => x.Sections[It.IsAny<int>()]).Returns(section.Object);

            document.Setup(x => x.AddSection()).Returns(section.Object);

            return document;
        }
    }
}
