using POS.DomainModels.CalendarPlanDomainModels;
using POS.DomainModels.EstimateDomainModels;

namespace POS.Infrastructure.Creators.Base;

public interface ICalendarWorksCreator
{
    List<CalendarWork> CreatePreparatoryCalendarWorks(IEnumerable<EstimateWork> preparatoryEstimateWorks, DateTime constructionStartDate);
    List<CalendarWork> CreateMainCalendarWorks(IEnumerable<EstimateWork> mainEstimateWorks, CalendarWork preparatoryTotalWork, DateTime constructionStartDate, int constructionDurationCeiling, List<decimal> otherExpensesPercentages, TotalWorkChapter totalWorkChapter);
}