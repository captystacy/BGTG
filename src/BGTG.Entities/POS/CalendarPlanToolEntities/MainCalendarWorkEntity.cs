using System.Collections.Generic;

namespace BGTG.Entities.POS.CalendarPlanToolEntities
{
    public class MainCalendarWorkEntity : CalendarWorkEntity
    {
        public ICollection<MainConstructionMonthEntity> ConstructionMonths { get; set; } = null!;
    }
}
