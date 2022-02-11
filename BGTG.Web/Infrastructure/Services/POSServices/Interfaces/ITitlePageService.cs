using BGTG.Web.ViewModels.POSViewModels;

namespace BGTG.Web.Infrastructure.Services.POSServices.Interfaces
{
    public interface ITitlePageService : ISavable
    {
        void Write(TitlePageViewModel viewModel, string windowsName);
    }
}
