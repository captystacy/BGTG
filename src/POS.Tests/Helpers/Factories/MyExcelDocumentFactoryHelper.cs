using System.IO;
using Moq;
using POS.Infrastructure.Factories.Base;
using POS.Infrastructure.Services.DocumentServices.ExcelService.Base;

namespace POS.Tests.Helpers.Factories
{
    public static class MyExcelDocumentFactoryHelper
    {
        public static Mock<IMyExcelDocumentFactory> GetMock()
        {
            return new Mock<IMyExcelDocumentFactory>();
        }

        public static Mock<IMyExcelDocumentFactory> GetMock(Mock<Stream> stream, Mock<IMyExcelDocument> document)
        {
            var documentFactory = GetMock();

            documentFactory.Setup(x => x.CreateAsync(stream.Object)).ReturnsAsync(document.Object);

            return documentFactory;
        }
    }
}
