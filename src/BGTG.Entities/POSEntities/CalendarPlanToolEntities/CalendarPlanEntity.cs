using System;
using System.Collections.Generic;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.Entities.POSEntities.CalendarPlanToolEntities
{
    public class CalendarPlanEntity : Auditable
    {
        public ICollection<PreparatoryCalendarWorkEntity> PreparatoryCalendarWorks { get; set; } = null!;
        public ICollection<MainCalendarWorkEntity> MainCalendarWorks { get; set; } = null!;
        public DateTime ConstructionStartDate { get; set; }
        public decimal ConstructionDuration { get; set; }
        public int ConstructionDurationCeiling { get; set; }
        public Guid POSId { get; set; }
        public POSEntity POS { get; set; } = null!;
    }
}
