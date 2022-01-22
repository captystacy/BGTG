using System;
using System.Collections.Generic;
using BGTG.POS.Tools.EstimateTool;

namespace BGTG.POS.Tools.CalendarPlanTool.Interfaces
{
    public interface ICalendarWorkCreator
    {
        CalendarWork Create(EstimateWork estimateWork, DateTime constructionStartDate);
        CalendarWork CreateAnyPreparatoryWork(string workName, List<CalendarWork> preparatoryCalendarWorks, int estimateChapter, DateTime constructionStartDate, List<decimal> percentages);
        CalendarWork CreateMainTotalWork(List<CalendarWork> mainCalendarWorks, CalendarWork initialTotalMainWork, DateTime constructionStartDate, int constructionDurationCeiling);
        CalendarWork CreateOtherExpensesWork(List<CalendarWork> mainCalendarWorks, CalendarWork initialTotalMainWork, DateTime constructionStartDate, List<decimal> percentages);
    }
}
