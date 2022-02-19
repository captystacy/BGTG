using System.Collections.Generic;

namespace BGTG.Entities.POSEntities.CalendarPlanToolEntities
{
    public class MainCalendarWorkEntity : CalendarWorkEntity
    {
        public ICollection<MainConstructionMonthEntity> ConstructionMonths { get; set; } = null!;
    }
}
