using POS.DomainModels.CalendarPlanDomainModels;
using POS.DomainModels.EstimateDomainModels;

namespace POS.Infrastructure.Creators.Base;

public interface ICalendarWorkCreator
{
    CalendarWork Create(EstimateWork estimateWork, DateTime constructionStartDate);
    CalendarWork CreateAnyPreparatoryWork(string workName, List<CalendarWork> preparatoryCalendarWorks, int estimateChapter, DateTime constructionStartDate, List<decimal> percentages);
    CalendarWork CreateMainTotalWork(List<CalendarWork> mainCalendarWorks, CalendarWork initialTotalMainWork, DateTime constructionStartDate, int constructionDurationCeiling);
    CalendarWork CreateOtherExpensesWork(List<CalendarWork> mainCalendarWorks, CalendarWork initialTotalMainWork, DateTime constructionStartDate, List<decimal> percentages);
}