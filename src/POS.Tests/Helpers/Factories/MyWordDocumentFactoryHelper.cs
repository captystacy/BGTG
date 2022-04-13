using System.Collections.Generic;
using System.IO;
using Moq;
using POS.Infrastructure.Factories.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;

namespace POS.Tests.Helpers.Factories
{
    public static class MyWordDocumentFactoryHelper
    {
        public static Mock<IMyWordDocumentFactory> GetMock()
        {
            return new Mock<IMyWordDocumentFactory>();
        }

        public static Mock<IMyWordDocumentFactory> GetMock(Mock<IMyWordDocument> document)
        {
            var documentFactory = GetMock();

            documentFactory.Setup(x => x.CreateAsync()).ReturnsAsync(document.Object);

            return documentFactory;
        }

        public static Mock<IMyWordDocumentFactory> GetMock(Mock<Stream> stream, Mock<IMyWordDocument> document)
        {
            var documentFactory = GetMock();

            documentFactory.Setup(x => x.CreateAsync(stream.Object)).ReturnsAsync(document.Object);

            return documentFactory;
        }

        public static Mock<IMyWordDocumentFactory> GetMock(string path, Mock<IMyWordDocument> document)
        {
            var documentFactory = GetMock();

            documentFactory.Setup(x => x.CreateAsync(path)).ReturnsAsync(document.Object);

            return documentFactory;
        }

        public static Mock<IMyWordDocumentFactory> GetMock(string path, Mock<IMyWordDocument> document, Dictionary<Mock<Stream>, Mock<IMyWordDocument>> dictionary)
        {
            var documentFactory = GetMock();

            documentFactory.Setup(x => x.CreateAsync(path)).ReturnsAsync(document.Object);

            foreach (var pair in dictionary)
            {
                documentFactory.Setup(x => x.CreateAsync(pair.Key.Object)).ReturnsAsync(pair.Value.Object);
            }

            return documentFactory;
        }
    }
}
