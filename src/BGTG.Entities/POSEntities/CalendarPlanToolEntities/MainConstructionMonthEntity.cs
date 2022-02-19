using System;

namespace BGTG.Entities.POSEntities.CalendarPlanToolEntities
{
    public class MainConstructionMonthEntity : ConstructionMonthEntity
    {
        public Guid MainCalendarWorkId { get; set; }
        public MainCalendarWorkEntity MainCalendarWork { get; set; } = null!;
    }
}
