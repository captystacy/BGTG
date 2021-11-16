using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic
{
    public class CalendarPlanWork
    {
        string WorkName { get; }
        double TotalEstimateWorkCost { get; }
        double EstimateWorkCostIncludingContructionAndInstallationWorks { get; }
        IEnumerable<ConstructionMonth> ConstructionMonths { get; }
    }
}
