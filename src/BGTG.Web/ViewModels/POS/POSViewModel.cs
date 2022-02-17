﻿using BGTG.Web.ViewModels.POS.CalendarPlanViewModels;
using BGTG.Web.ViewModels.POS.DurationByLCViewModels;
using BGTG.Web.ViewModels.POS.DurationByTCPViewModels;
using BGTG.Web.ViewModels.POS.EnergyAndWaterViewModels;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.Web.ViewModels.POS
{
    public class POSViewModel : ViewModelBase
    {
        public CalendarPlanViewModel? CalendarPlanViewModel { get; set; }
        public DurationByLCViewModel? DurationByLCViewModel { get; set; }
        public DurationByTCPViewModel? DurationByTCPViewModel { get; set; }
        public EnergyAndWaterViewModel? EnergyAndWaterViewModel { get; set; }
    }
}
