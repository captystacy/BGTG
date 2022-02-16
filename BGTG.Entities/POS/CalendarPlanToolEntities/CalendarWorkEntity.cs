using System;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.Entities.POS.CalendarPlanToolEntities
{
    public class CalendarWorkEntity : Identity
    {
        public Guid CalendarPlanId { get; set; }
        public CalendarPlanEntity CalendarPlan { get; set; } = null!;
        public string WorkName { get; set; } = null!;
        public decimal TotalCost { get; set; }
        public decimal TotalCostIncludingCAIW { get; set; }
        public int EstimateChapter { get; set; }
    }
}
