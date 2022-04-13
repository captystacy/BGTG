using OfficeOpenXml;
using POS.Infrastructure.Services.DocumentServices.ExcelService.Base;

namespace POS.Infrastructure.Services.DocumentServices.ExcelService
{
    public class MyExcelRange : IMyExcelRange
    {
        private readonly ExcelRange _excelRange;

        public IMyExcelRange this[int row, int col] => new MyExcelRange(_excelRange[row, col]);

        public string Text => _excelRange.Text;

        public MyExcelRange(ExcelRange excelRange)
        {
            _excelRange = excelRange;
        }
    }
}