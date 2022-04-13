using OfficeOpenXml;
using POS.Infrastructure.Services.DocumentServices.ExcelService.Base;

namespace POS.Infrastructure.Services.DocumentServices.ExcelService
{
    public class MyWorkSheet : IMyWorkSheet
    {
        private readonly ExcelWorksheet _worksheet;

        public string Name => _worksheet.Name;
        public IMyExcelRange Cells { get; }

        public MyWorkSheet(IMyExcelRange cells, ExcelWorksheet worksheet)
        {
            _worksheet = worksheet;
            Cells = cells;
        }
    }
}