using System;
using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic
{
    public class CalendarPlan
    {
        public List<CalendarWork> CalendarWorks { get; }
        public DateTime ConstructionStartDate { get; }
        public decimal ConstructionDuration { get; }

        public CalendarPlan(List<CalendarWork> calendarWorks, DateTime constructionStartDate, decimal constructionDuration)
        {
            CalendarWorks = calendarWorks;
            ConstructionStartDate = constructionStartDate;
            ConstructionDuration = constructionDuration;
        }
    }
}
