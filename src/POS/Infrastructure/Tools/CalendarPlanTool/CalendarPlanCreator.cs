using POS.Infrastructure.Tools.CalendarPlanTool.Base;
using POS.Infrastructure.Tools.CalendarPlanTool.Models;
using POS.Infrastructure.Tools.EstimateTool.Models;

namespace POS.Infrastructure.Tools.CalendarPlanTool;

public class CalendarPlanCreator : ICalendarPlanCreator
{
    private readonly ICalendarWorksProvider _calendarWorksProvider;

    public CalendarPlanCreator(ICalendarWorksProvider calendarWorksProvider)
    {
        _calendarWorksProvider = calendarWorksProvider;
    }

    public CalendarPlan Create(Estimate estimate, List<decimal> otherExpensesPercentages, TotalWorkChapter totalWorkChapter)
    {
        var preparatoryCalendarWorks = _calendarWorksProvider.CreatePreparatoryCalendarWorks(
            estimate.PreparatoryEstimateWorks,
            estimate.ConstructionStartDate);

        var mainCalendarWorks = _calendarWorksProvider.CreateMainCalendarWorks(
            estimate.MainEstimateWorks,
            preparatoryCalendarWorks.First(x => x.WorkName == AppData.TotalWorkName),
            estimate.ConstructionStartDate,
            estimate.ConstructionDurationCeiling,
            otherExpensesPercentages,
            totalWorkChapter);

        return new CalendarPlan( preparatoryCalendarWorks, mainCalendarWorks, estimate.ConstructionStartDate, estimate.ConstructionDuration, estimate.ConstructionDurationCeiling);
    }
}