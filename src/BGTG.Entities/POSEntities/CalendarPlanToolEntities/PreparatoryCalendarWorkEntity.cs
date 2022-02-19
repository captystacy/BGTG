using System.Collections.Generic;

namespace BGTG.Entities.POSEntities.CalendarPlanToolEntities
{
    public class PreparatoryCalendarWorkEntity : CalendarWorkEntity
    {
        public ICollection<PreparatoryConstructionMonthEntity> ConstructionMonths { get; set; } = null!;
    }
}
