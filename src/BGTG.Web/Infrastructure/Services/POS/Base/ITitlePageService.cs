using BGTG.Web.ViewModels.POS;

namespace BGTG.Web.Infrastructure.Services.POS.Base
{
    public interface ITitlePageService : ISavable
    {
        void Write(TitlePageViewModel viewModel);
    }
}
