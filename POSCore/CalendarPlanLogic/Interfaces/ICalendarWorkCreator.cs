using POSCore.EstimateLogic;
using System;
using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface ICalendarWorkCreator
    {
        CalendarWork CreateCalendarWork(DateTime initialDate, EstimateWork estimateWork,  List<decimal> percentages);
    }
}
