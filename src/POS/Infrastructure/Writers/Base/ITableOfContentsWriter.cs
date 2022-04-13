using POS.ViewModels;

namespace POS.Infrastructure.Writers.Base
{
    public interface ITableOfContentsWriter
    {
        Task<MemoryStream> GetTableOfContentsStream(TableOfContentsViewModel viewModel);
    }
}