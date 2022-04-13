namespace POS.Infrastructure.Services.DocumentServices.ExcelService.Base
{
    public interface IMyExcelRange
    {
        string? Text { get; }
        IMyExcelRange this[int row, int col] { get; }
    }
}