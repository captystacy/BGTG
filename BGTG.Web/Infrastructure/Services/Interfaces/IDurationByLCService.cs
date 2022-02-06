using BGTG.POS.DurationTools.DurationByLCTool;
using BGTG.Web.ViewModels.POSViewModels.DurationByLCViewModels;

namespace BGTG.Web.Infrastructure.Services.Interfaces
{
    public interface IDurationByLCService : ISavable
    {
        DurationByLC Write(DurationByLCCreateViewModel viewModel, string windowsName);
        DurationByLC Write(DurationByLC durationByLC, string windowsName);
    }
}
