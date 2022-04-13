using Moq;
using POS.Infrastructure.Services.DocumentServices.ExcelService.Base;

namespace POS.Tests.Helpers.Services.DocumentServices.ExcelService
{
    public static class MyExcelDocumentHelper
    {
        public static Mock<IMyExcelDocument> GetMock(Mock<IMyWorkBook> workBook)
        {
            var excelDocument = GetMock();

            excelDocument.SetupGet(x => x.WorkBook).Returns(workBook.Object);

            return excelDocument;
        }

        public static Mock<IMyExcelDocument> GetMock()
        {
            return new Mock<IMyExcelDocument>();
        }
    }
}