using Moq;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;

namespace POS.Tests.Helpers.Services.DocumentServices.WordService
{
    public static class MyParagraphHelper
    {
        public static Mock<IMyParagraph> GetMock()
        {
            return new Mock<IMyParagraph>();
        }

        public static Mock<IMyParagraph> GetMock(string text)
        {
            var paragraph = GetMock();
            paragraph.Setup(x => x.Text).Returns(text);
            return paragraph;
        }
    }
}