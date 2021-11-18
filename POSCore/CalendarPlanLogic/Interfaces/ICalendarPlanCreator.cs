using System;
using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface ICalendarPlanCreator
    {
        CalendarPlan CreateCalendarPlan(DateTime initialDate, IEnumerable<int[]> groupsOfPercentages);
    }
}
