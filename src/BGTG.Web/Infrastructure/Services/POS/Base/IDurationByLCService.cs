using BGTG.POS.DurationTools.DurationByLCTool;
using BGTG.Web.ViewModels.POS.DurationByLCViewModels;

namespace BGTG.Web.Infrastructure.Services.POS.Base
{
    public interface IDurationByLCService : ISavable
    {
        DurationByLC Write(DurationByLCCreateViewModel viewModel);
        DurationByLC Write(DurationByLC durationByLC);
    }
}
