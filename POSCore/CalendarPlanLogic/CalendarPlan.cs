using System;
using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic
{
    public class CalendarPlan
    {
        public List<CalendarWork> PreparatoryCalendarWorks { get; }
        public List<CalendarWork> MainCalendarWorks { get; }
        public DateTime ConstructionStartDate { get; }
        public decimal ConstructionDuration { get; }

        public CalendarPlan(List<CalendarWork> preparatoryCalendarWorks, List<CalendarWork> mainCalendarWorks, DateTime constructionStartDate, decimal constructionDuration)
        {
            PreparatoryCalendarWorks = preparatoryCalendarWorks;
            MainCalendarWorks = mainCalendarWorks;
            ConstructionStartDate = constructionStartDate;
            ConstructionDuration = constructionDuration;
        }
    }
}
