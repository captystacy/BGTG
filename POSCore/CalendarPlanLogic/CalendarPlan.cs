using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic
{
    public class CalendarPlan
    {
        public IEnumerable<CalendarWork> CalendarWorks { get; }

        public CalendarPlan(IEnumerable<CalendarWork> calendarWorks)
        {
            CalendarWorks = calendarWorks;
        }
    }
}
