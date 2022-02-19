using BGTG.Web.ViewModels.POSViewModels.CalendarPlanViewModels;
using BGTG.Web.ViewModels.POSViewModels.DurationByLCViewModels;
using BGTG.Web.ViewModels.POSViewModels.DurationByTCPViewModels;
using BGTG.Web.ViewModels.POSViewModels.EnergyAndWaterViewModels;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.Web.ViewModels.POSViewModels
{
    public class POSViewModel : ViewModelBase
    {
        public CalendarPlanViewModel? CalendarPlanViewModel { get; set; }
        public DurationByLCViewModel? DurationByLCViewModel { get; set; }
        public DurationByTCPViewModel? DurationByTCPViewModel { get; set; }
        public EnergyAndWaterViewModel? EnergyAndWaterViewModel { get; set; }
    }
}
