using OfficeOpenXml;
using POS.Infrastructure.Services.DocumentServices.ExcelService.Base;

namespace POS.Infrastructure.Services.DocumentServices.ExcelService
{
    public class MyExcelDocument : IMyExcelDocument
    {
        private readonly ExcelPackage _excelPackage;
        public IMyWorkBook WorkBook { get; }

        public MyExcelDocument(ExcelPackage excelPackage, MyWorkBook workBook)
        {
            _excelPackage = excelPackage;
            WorkBook = workBook;
        }

        public void Dispose()
        {
            _excelPackage.Dispose();
        }
    }
}