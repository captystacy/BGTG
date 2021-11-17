using POSCore.EstimateLogic;
using System;

namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface ICalendarWorkCreator
    {
        CalendarWork CreateCalendarWork(EstimateWork estimateWork, DateTime initialDate, params int[] percentages);
    }
}
