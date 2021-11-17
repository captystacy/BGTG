using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic
{
    public class CalendarWork
    {
        public string WorkName { get; }
        public double TotalCost { get; }
        public double TotalCostIncludingContructionAndInstallationWorks { get; }
        public ConstructionPeriod ConstructionPeriod { get; }

        public CalendarWork(string workName, double totalCost, double totalCostIncludingContructionAndInstallationWorks, ConstructionPeriod constructionPeriod)
        {
            WorkName = workName;
            TotalCost = totalCost;
            TotalCostIncludingContructionAndInstallationWorks = totalCostIncludingContructionAndInstallationWorks;
            ConstructionPeriod = constructionPeriod;
        }
    }
}
