using POS.DomainModels.CalendarPlanDomainModels;
using POS.DomainModels.EstimateDomainModels;

namespace POS.Infrastructure.Creators.Base;

public interface ICalendarPlanCreator
{
    CalendarPlan Create(Estimate estimate, List<decimal> otherExpensesPercentages, TotalWorkChapter totalWorkChapter);
}