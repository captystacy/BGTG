using POS.Infrastructure.Services.DocumentServices.ExcelService.Base;

namespace POS.Infrastructure.Factories.Base
{
    public interface IMyExcelDocumentFactory
    {
        Task<IMyExcelDocument> CreateAsync(Stream stream);
    }
}