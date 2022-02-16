using System;

namespace BGTG.Entities.POS.CalendarPlanToolEntities
{
    public class MainConstructionMonthEntity : ConstructionMonthEntity
    {
        public Guid MainCalendarWorkId { get; set; }
        public MainCalendarWorkEntity MainCalendarWork { get; set; } = null!;
    }
}
