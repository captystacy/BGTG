using BGTG.Web.ViewModels.POSViewModels;

namespace BGTG.Web.Infrastructure.Services.POSServices.Interfaces
{
    public interface ITableOfContentsService : ISavable
    {
        void Write(TableOfContentsViewModel viewModel, string identityName);
    }
}