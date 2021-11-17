using POSCore.EstimateLogic;

namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface ICalendarWorkConverter
    {
        CalendarWork Convert(EstimateWork estimateWork, ConstructionPeriod constructionPeriod);
    }
}
