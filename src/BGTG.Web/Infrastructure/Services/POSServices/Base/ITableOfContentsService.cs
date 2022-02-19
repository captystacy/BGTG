using BGTG.Web.ViewModels.POSViewModels;

namespace BGTG.Web.Infrastructure.Services.POSServices.Base
{
    public interface ITableOfContentsService : ISavable
    {
        void Write(TableOfContentsViewModel viewModel);
    }
}