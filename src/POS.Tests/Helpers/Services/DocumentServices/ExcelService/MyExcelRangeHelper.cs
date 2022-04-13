using Moq;
using POS.Infrastructure.Services.DocumentServices.ExcelService.Base;

namespace POS.Tests.Helpers.Services.DocumentServices.ExcelService
{
    public static class MyExcelRangeHelper
    {
        public static Mock<IMyExcelRange> GetMock(string text)
        {
            var excelRange = GetMock();

            excelRange.SetupGet(x => x.Text).Returns(text);

            return excelRange;
        }

        public static Mock<IMyExcelRange> GetMock()
        {
            return new Mock<IMyExcelRange>();
        }

        public static Mock<IMyExcelRange> StubRange(this Mock<IMyExcelRange> excelRange, int startRow, int endRow, int col)
        {
            var stub = GetMock();

            for (int row = startRow; row <= endRow; row++)
            {
                excelRange.Setup(x => x[row, col]).Returns(stub.Object);
            }

            return stub;
        }
    }
}