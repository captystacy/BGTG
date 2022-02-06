using BGTG.POS.DurationTools.DurationByTCPTool;
using BGTG.Web.ViewModels.POSViewModels.DurationByTCPViewModels;

namespace BGTG.Web.Infrastructure.Services.Interfaces
{
    public interface IDurationByTCPService : ISavable
    {
        DurationByTCP Write(DurationByTCPCreateViewModel viewModel, string windowsName);
        DurationByTCP Write(DurationByTCP durationByTCP, string windowsName);
    }
}