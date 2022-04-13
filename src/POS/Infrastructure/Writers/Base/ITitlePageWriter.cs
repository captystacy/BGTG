using POS.ViewModels;

namespace POS.Infrastructure.Writers.Base
{
    public interface ITitlePageWriter
    {
        Task<MemoryStream> GetTitlePageStream(TitlePageViewModel viewModel);
    }
}