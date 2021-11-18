using POSCore.EstimateLogic;
using System;

namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface ICalendarWorkCreator
    {
        CalendarWork CreateCalendarWork(DateTime initialDate, EstimateWork estimateWork,  int[] percentages);
    }
}
