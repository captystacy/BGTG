using Moq;
using POS.Infrastructure.Services.DocumentServices.ExcelService.Base;

namespace POS.Tests.Helpers.Services.DocumentServices.ExcelService
{
    public static class MyWorkSheetHelper
    {
        public static Mock<IMyWorkSheet> GetMock(Mock<IMyExcelRange> excelRange, string name)
        {
            var workSheet = new Mock<IMyWorkSheet>();

            workSheet.SetupGet(x => x.Cells).Returns(excelRange.Object);
            workSheet.SetupGet(x => x.Name).Returns(name);

            return workSheet;
        }
    }
}