using Moq;
using POS.Infrastructure.Services.DocumentServices.ExcelService.Base;

namespace POS.Tests.Helpers.Services.DocumentServices.ExcelService
{
    public static class MyWorkBookHelper
    {
        public static Mock<IMyWorkBook> GetMock(Mock<IMyWorkSheet> workSheet)
        {
            var workBook = new Mock<IMyWorkBook>();

            workBook.SetupGet(x => x.WorkSheets[0]).Returns(workSheet.Object);

            return workBook;
        }
    }
}