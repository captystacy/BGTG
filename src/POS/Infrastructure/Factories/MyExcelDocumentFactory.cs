using OfficeOpenXml;
using POS.Infrastructure.Factories.Base;
using POS.Infrastructure.Services.DocumentServices.ExcelService;
using POS.Infrastructure.Services.DocumentServices.ExcelService.Base;

namespace POS.Infrastructure.Factories
{
    public class MyExcelDocumentFactory : IMyExcelDocumentFactory
    {
        public MyExcelDocumentFactory()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public Task<IMyExcelDocument> CreateAsync(Stream stream)
        {
            var excelPackage = new ExcelPackage(stream);

            var workSheets = new List<MyWorkSheet>();

            foreach (var workSheet in excelPackage.Workbook.Worksheets)
            {
                var excelRange = new MyExcelRange(workSheet.Cells);
                var myWorkSheet = new MyWorkSheet(excelRange, workSheet);
                workSheets.Add(myWorkSheet);
            }

            var workBook = new MyWorkBook(workSheets);

            var excelDocument = new MyExcelDocument(excelPackage, workBook);

            return Task.FromResult<IMyExcelDocument>(excelDocument);
        }
    }
}