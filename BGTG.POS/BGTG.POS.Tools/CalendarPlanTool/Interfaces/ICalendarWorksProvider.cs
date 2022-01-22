using System;
using System.Collections.Generic;
using BGTG.POS.Tools.EstimateTool;

namespace BGTG.POS.Tools.CalendarPlanTool.Interfaces
{
    public interface ICalendarWorksProvider
    {
        List<CalendarWork> CreatePreparatoryCalendarWorks(IEnumerable<EstimateWork> preparatoryEstimateWorks, DateTime constructionStartDate);
        List<CalendarWork> CreateMainCalendarWorks(IEnumerable<EstimateWork> mainEstimateWorks, CalendarWork preparatoryTotalWork, DateTime constructionStartDate, int constructionDurationCeiling, List<decimal> otherExpensesPercentages, TotalWorkChapter totalWorkChapter);
    }
}
