using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface ICalendarPlanWork
    {
        string WorkName { get; }
        double TotalEstimateWorkCost { get; }
        double EstimateWorkCostIncludingContructionAndInstallationWorks { get; }
        IEnumerable<IConstructionMonth> ConstructionMonths { get; }
    }
}
