using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic
{
    public class CalendarPlan
    {
        public List<CalendarWork> CalendarWorks { get; }

        public CalendarPlan(List<CalendarWork> calendarWorks)
        {
            CalendarWorks = calendarWorks;
        }
    }
}
