using POS.ViewModels;

namespace POS.Infrastructure.Services.Base;

public interface ITableOfContentsService
{
    void Write(TableOfContentsViewModel viewModel);
}