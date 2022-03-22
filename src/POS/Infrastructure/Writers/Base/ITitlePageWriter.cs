using POS.ViewModels;

namespace POS.Infrastructure.Writers.Base;

public interface ITitlePageWriter
{
    MemoryStream Write(TitlePageViewModel viewModel);
}