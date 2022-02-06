using System.Collections.Generic;
using BGTG.POS.EstimateTool;

namespace BGTG.POS.CalendarPlanTool.Interfaces
{
    public interface ICalendarPlanCreator
    {
        CalendarPlan Create(Estimate estimate, List<decimal> otherExpensesPercentages, TotalWorkChapter totalWorkChapter);
    }
}
