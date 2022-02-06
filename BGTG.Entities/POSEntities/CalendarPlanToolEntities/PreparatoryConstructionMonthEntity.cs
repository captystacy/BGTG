using System;

namespace BGTG.Entities.POSEntities.CalendarPlanToolEntities
{
    public class PreparatoryConstructionMonthEntity : ConstructionMonthEntity
    {
        public Guid PreparatoryCalendarWorkId { get; set; }
        public PreparatoryCalendarWorkEntity PreparatoryCalendarWork { get; set; }
    }
}
