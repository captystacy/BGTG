using BGTG.POS.DurationTools.DurationByTCPTool;
using BGTG.Web.ViewModels.POS.DurationByTCPViewModels;

namespace BGTG.Web.Infrastructure.Services.POS.Base
{
    public interface IDurationByTCPService : ISavable
    {
        DurationByTCP? Write(DurationByTCPCreateViewModel viewModel);
        DurationByTCP Write(DurationByTCP durationByTCP);
    }
}