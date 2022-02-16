using BGTG.Web.ViewModels.POS.CalendarPlanViewModels;
using BGTG.Web.ViewModels.POS.DurationByLCViewModels;
using BGTG.Web.ViewModels.POS.DurationByTCPViewModels;
using BGTG.Web.ViewModels.POS.EnergyAndWaterViewModels;

namespace BGTG.Web.ViewModels.POS
{
    public class POSViewModel
    {
        public CalendarPlanViewModel? CalendarPlanViewModel { get; set; }
        public DurationByLCViewModel? DurationByLCViewModel { get; set; }
        public DurationByTCPViewModel? DurationByTCPViewModel { get; set; }
        public EnergyAndWaterViewModel? EnergyAndWaterViewModel { get; set; }
    }
}
