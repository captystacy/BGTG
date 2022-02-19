using BGTG.POS.DurationTools.DurationByLCTool;
using BGTG.Web.ViewModels.POSViewModels.DurationByLCViewModels;

namespace BGTG.Web.Infrastructure.Services.POSServices.Base
{
    public interface IDurationByLCService : ISavable
    {
        DurationByLC Write(DurationByLCCreateViewModel viewModel);
        DurationByLC Write(DurationByLC durationByLC);
    }
}
