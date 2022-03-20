namespace POS.Infrastructure.Services.Base;

public interface IExcelDocumentService
{
    void Load(Stream stream);
    void Dispose();
    int WorkSheetIndex { get; set; }
    string WorkSheetName { get; }
    string? GetCellText(int row, int column);
}