using POSCore.EstimateLogic;
using System;

namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface ICalendarWorkCreator
    {
        CalendarWork Create(DateTime initialDate, EstimateWork estimateWork);
    }
}
