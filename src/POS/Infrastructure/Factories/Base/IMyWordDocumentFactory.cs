using POS.Infrastructure.Services.DocumentServices.WordService.Base;

namespace POS.Infrastructure.Factories.Base
{
    public interface IMyWordDocumentFactory
    {
        Task<IMyWordDocument> CreateAsync(string path);
        Task<IMyWordDocument> CreateAsync();
        Task<IMyWordDocument> CreateAsync(Stream stream);
    }
}