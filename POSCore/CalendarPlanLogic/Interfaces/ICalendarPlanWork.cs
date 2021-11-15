using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface ICalendarPlanWork
    {
        double TotalEstimateObjectCost { get; }
        double EstimateObjectCostIncludingContructionAndInstallationWorks { get; }
        IEnumerable<IConstructionMonth> ConstructionMonths { get; }
    }
}
