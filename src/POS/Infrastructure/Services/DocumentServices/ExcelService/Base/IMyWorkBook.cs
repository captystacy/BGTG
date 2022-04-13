namespace POS.Infrastructure.Services.DocumentServices.ExcelService.Base
{
    public interface IMyWorkBook
    {
        IReadOnlyList<IMyWorkSheet> WorkSheets { get; }
    }
}