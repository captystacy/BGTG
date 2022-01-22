using System.Collections.Generic;
using BGTG.POS.Tools.EstimateTool;

namespace BGTG.POS.Tools.CalendarPlanTool.Interfaces
{
    public interface ICalendarPlanCreator
    {
        CalendarPlan Create(Estimate estimate, List<decimal> otherExpensesPercentages, TotalWorkChapter totalWorkChapter);
    }
}
