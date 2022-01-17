using POSCore.EstimateLogic;
using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface ICalendarPlanCreator
    {
        CalendarPlan Create(Estimate estimate, List<decimal> otherExpensesPercentages, TotalWorkChapter totalWorkChapter);
    }
}
