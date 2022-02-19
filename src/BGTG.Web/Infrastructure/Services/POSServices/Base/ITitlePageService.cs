using BGTG.Web.ViewModels.POSViewModels;

namespace BGTG.Web.Infrastructure.Services.POSServices.Base
{
    public interface ITitlePageService : ISavable
    {
        void Write(TitlePageViewModel viewModel);
    }
}
