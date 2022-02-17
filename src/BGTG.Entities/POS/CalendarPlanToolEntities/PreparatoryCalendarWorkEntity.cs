using System.Collections.Generic;

namespace BGTG.Entities.POS.CalendarPlanToolEntities
{
    public class PreparatoryCalendarWorkEntity : CalendarWorkEntity
    {
        public ICollection<PreparatoryConstructionMonthEntity> ConstructionMonths { get; set; } = null!;
    }
}
