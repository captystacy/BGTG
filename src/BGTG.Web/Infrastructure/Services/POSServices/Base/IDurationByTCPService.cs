using BGTG.POS.DurationTools.DurationByTCPTool;
using BGTG.Web.ViewModels.POSViewModels.DurationByTCPViewModels;

namespace BGTG.Web.Infrastructure.Services.POSServices.Base
{
    public interface IDurationByTCPService : ISavable
    {
        DurationByTCP? Write(DurationByTCPCreateViewModel viewModel);
        DurationByTCP Write(DurationByTCP durationByTCP);
    }
}