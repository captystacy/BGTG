namespace POSCore.CalendarPlanLogic
{
    public class CalendarWork
    {
        public string WorkName { get; }
        public double TotalCost { get; }
        public double TotalCostIncludingContructionAndInstallationWorks { get; }
        public ConstructionPeriod ConstructionPeriod { get; }
        public int EstimateChapter { get; }

        public CalendarWork(string workName, double totalCost, double totalCostIncludingContructionAndInstallationWorks, ConstructionPeriod constructionPeriod, int estimateChapter)
        {
            WorkName = workName;
            TotalCost = totalCost;
            TotalCostIncludingContructionAndInstallationWorks = totalCostIncludingContructionAndInstallationWorks;
            ConstructionPeriod = constructionPeriod;
            EstimateChapter = estimateChapter;
        }
    }
}
