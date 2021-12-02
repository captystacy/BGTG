using POSCore.EstimateLogic;

namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface ICalendarPlanCreator
    {
        CalendarPlan Create(Estimate estimate);
    }
}
