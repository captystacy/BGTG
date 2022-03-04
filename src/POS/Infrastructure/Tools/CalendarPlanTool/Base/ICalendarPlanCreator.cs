using POS.Infrastructure.Tools.CalendarPlanTool.Models;
using POS.Infrastructure.Tools.EstimateTool.Models;

namespace POS.Infrastructure.Tools.CalendarPlanTool.Base;

public interface ICalendarPlanCreator
{
    CalendarPlan Create(Estimate estimate, List<decimal> otherExpensesPercentages, TotalWorkChapter totalWorkChapter);
}