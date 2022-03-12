using POS.ViewModels;

namespace POS.Infrastructure.Services.Base;

public interface ITitlePageService
{
    MemoryStream Write(TitlePageViewModel viewModel);
}