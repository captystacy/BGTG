using BGTG.POS.DurationTools.DurationByTCPTool;
using BGTG.Web.ViewModels.POSViewModels.DurationByTCPViewModels;

namespace BGTG.Web.Infrastructure.Services.POSServices.Interfaces
{
    public interface IDurationByTCPService : ISavable
    {
        DurationByTCP Write(DurationByTCPCreateViewModel viewModel, string identityName);
        DurationByTCP Write(DurationByTCP durationByTCP, string identityName);
    }
}