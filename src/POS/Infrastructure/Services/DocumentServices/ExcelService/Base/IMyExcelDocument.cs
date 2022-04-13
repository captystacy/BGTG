namespace POS.Infrastructure.Services.DocumentServices.ExcelService.Base
{
    public interface IMyExcelDocument : IDisposable
    {
        IMyWorkBook WorkBook { get; }
    }
}