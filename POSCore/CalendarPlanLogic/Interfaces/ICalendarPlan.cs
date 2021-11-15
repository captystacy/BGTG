using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface ICalendarPlan
    {
        IEnumerable<ICalendarPlanWork> CalendarPlanWorks { get; }
    }
}
