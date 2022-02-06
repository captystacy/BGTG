using System;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.Entities.POSEntities.CalendarPlanToolEntities
{
    public class CalendarWorkEntity : Identity
    {
        public Guid CalendarPlanId { get; set; }
        public CalendarPlanEntity CalendarPlan { get; set; }
        public string WorkName { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalCostIncludingCAIW { get; set; }
        public int EstimateChapter { get; set; }
    }
}
