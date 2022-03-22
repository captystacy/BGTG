using POS.ViewModels;

namespace POS.Infrastructure.Writers.Base;

public interface ITableOfContentsWriter
{
    MemoryStream Write(TableOfContentsViewModel viewModel);
}