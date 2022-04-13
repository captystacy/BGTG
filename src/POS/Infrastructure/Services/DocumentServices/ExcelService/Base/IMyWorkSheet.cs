namespace POS.Infrastructure.Services.DocumentServices.ExcelService.Base
{
    public interface IMyWorkSheet
    {
        string Name { get; }
        IMyExcelRange Cells { get; }
    }
}