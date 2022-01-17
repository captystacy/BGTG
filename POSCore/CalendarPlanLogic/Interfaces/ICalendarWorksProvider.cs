using POSCore.EstimateLogic;
using System;
using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface ICalendarWorksProvider
    {
        List<CalendarWork> CreatePreparatoryCalendarWorks(IEnumerable<EstimateWork> preparatoryEstimateWorks, DateTime constructionStartDate);
        List<CalendarWork> CreateMainCalendarWorks(IEnumerable<EstimateWork> mainEstimateWorks, CalendarWork preparatoryTotalWork, DateTime constructionStartDate, int constructionDurationCeiling, List<decimal> otherExpensesPercentages, TotalWorkChapter totalWorkChapter);
    }
}
