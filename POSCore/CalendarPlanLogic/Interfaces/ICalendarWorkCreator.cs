using POSCore.EstimateLogic;
using System;
using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface ICalendarWorkCreator
    {
        CalendarWork Create(EstimateWork estimateWork, DateTime constructionStartDate);
        List<CalendarWork> CreatePreparatoryCalendarWorks(List<EstimateWork> preparatoryEstimateWorks, DateTime constructionStartDate);
        List<CalendarWork> CreateMainCalendarWorks(List<EstimateWork> mainEstimateWorks, CalendarWork preparatoryTotalWork, DateTime constructionStartDate, decimal constructionDuration, List<decimal> otherExpensesPercentages);
    }
}
