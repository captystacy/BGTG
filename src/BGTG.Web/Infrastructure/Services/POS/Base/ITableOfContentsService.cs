using BGTG.Web.ViewModels.POS;

namespace BGTG.Web.Infrastructure.Services.POS.Base
{
    public interface ITableOfContentsService : ISavable
    {
        void Write(TableOfContentsViewModel viewModel);
    }
}