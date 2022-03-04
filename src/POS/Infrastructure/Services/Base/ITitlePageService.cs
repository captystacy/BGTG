using POS.ViewModels;

namespace POS.Infrastructure.Services.Base;

public interface ITitlePageService
{
    void Write(TitlePageViewModel viewModel);
}