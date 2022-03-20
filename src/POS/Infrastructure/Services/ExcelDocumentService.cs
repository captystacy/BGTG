using OfficeOpenXml;
using POS.Infrastructure.Services.Base;

namespace POS.Infrastructure.Services;

public class ExcelDocumentService : IExcelDocumentService
{
    private ExcelPackage _excelPackage = null!;

    public int WorkSheetIndex { get; set; }

    public string WorkSheetName => _excelPackage.Workbook.Worksheets[WorkSheetIndex].Name;

    public ExcelDocumentService()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public void Load(Stream stream)
    {
        _excelPackage = new ExcelPackage(stream);
    }

    public string? GetCellText(int row, int column)
    {
        return _excelPackage.Workbook.Worksheets[WorkSheetIndex].Cells[row, column].Text;
    }

    public void Dispose()
    {
        _excelPackage.Dispose();
    }
}