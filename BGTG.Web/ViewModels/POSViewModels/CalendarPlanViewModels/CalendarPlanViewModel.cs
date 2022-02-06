using System;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.Web.ViewModels.POSViewModels.CalendarPlanViewModels
{
    public class CalendarPlanViewModel : ViewModelBase
    {
        public DateTime ConstructionStartDate { get; set; }
        public decimal ConstructionDuration { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
