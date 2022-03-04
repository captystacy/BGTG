using POS.Infrastructure.Tools.CalendarPlanTool.Models;
using POS.Infrastructure.Tools.EstimateTool.Models;

namespace POS.Infrastructure.Tools.CalendarPlanTool.Base;

public interface ICalendarWorksProvider
{
    List<CalendarWork> CreatePreparatoryCalendarWorks(IEnumerable<EstimateWork> preparatoryEstimateWorks, DateTime constructionStartDate);
    List<CalendarWork> CreateMainCalendarWorks(IEnumerable<EstimateWork> mainEstimateWorks, CalendarWork preparatoryTotalWork, DateTime constructionStartDate, int constructionDurationCeiling, List<decimal> otherExpensesPercentages, TotalWorkChapter totalWorkChapter);
}