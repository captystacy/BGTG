using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic
{
    public class CalendarPlan
    {
        public IEnumerable<CalendarWork> CalendarPlanWorks { get; }

        public CalendarPlan(IEnumerable<CalendarWork> calendarPlanWorks)
        {
            CalendarPlanWorks = calendarPlanWorks;
        }
    }
}
