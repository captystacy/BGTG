using BGTG.POS.Web.ViewModels.DurationByTCPViewModels;

namespace BGTG.POS.Web.Infrastructure.Services.Interfaces
{
    public interface IDurationByTCPService : ISavable
    {
        bool Write(DurationByTCPViewModel durationByTCPViewModel);
    }
}