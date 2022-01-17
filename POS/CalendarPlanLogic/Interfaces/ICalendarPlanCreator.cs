using System.Collections.Generic;
using POS.EstimateLogic;

namespace POS.CalendarPlanLogic.Interfaces
{
    public interface ICalendarPlanCreator
    {
        CalendarPlan Create(Estimate estimate, List<decimal> otherExpensesPercentages, TotalWorkChapter totalWorkChapter);
    }
}
