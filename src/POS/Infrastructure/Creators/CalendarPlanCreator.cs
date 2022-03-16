using POS.DomainModels.CalendarPlanDomainModels;
using POS.DomainModels.EstimateDomainModels;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Creators.Base;

namespace POS.Infrastructure.Creators;

public class CalendarPlanCreator : ICalendarPlanCreator
{
    private readonly ICalendarWorksCreator _calendarWorksCreator;

    public CalendarPlanCreator(ICalendarWorksCreator calendarWorksCreator)
    {
        _calendarWorksCreator = calendarWorksCreator;
    }

    public CalendarPlan Create(Estimate estimate, List<decimal> otherExpensesPercentages, TotalWorkChapter totalWorkChapter)
    {
        var preparatoryCalendarWorks = _calendarWorksCreator.CreatePreparatoryCalendarWorks(
            estimate.PreparatoryEstimateWorks,
            estimate.ConstructionStartDate);

        var mainCalendarWorks = _calendarWorksCreator.CreateMainCalendarWorks(
            estimate.MainEstimateWorks,
            preparatoryCalendarWorks.First(x => x.WorkName == Constants.AppConstants.TotalWorkName),
            estimate.ConstructionStartDate,
            estimate.ConstructionDurationCeiling,
            otherExpensesPercentages,
            totalWorkChapter);

        return new CalendarPlan( preparatoryCalendarWorks, mainCalendarWorks, estimate.ConstructionStartDate, estimate.ConstructionDuration, estimate.ConstructionDurationCeiling);
    }
}