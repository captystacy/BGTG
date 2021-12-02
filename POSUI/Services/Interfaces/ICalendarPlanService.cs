using POSCore.EstimateLogic;

namespace POSUI.Services.Interfaces
{
    public interface ICalendarPlanService
    {
        Estimate GetEstimate(string[] estimatesPaths);
    }
}