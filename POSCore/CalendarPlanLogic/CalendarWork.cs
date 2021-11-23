namespace POSCore.CalendarPlanLogic
{
    public class CalendarWork
    {
        public string WorkName { get; }
        public decimal TotalCost { get; }
        public decimal TotalCostIncludingContructionAndInstallationWorks { get; }
        public ConstructionPeriod ConstructionPeriod { get; }
        public int EstimateChapter { get; }

        public CalendarWork(string workName, decimal totalCost, decimal totalCostIncludingContructionAndInstallationWorks, ConstructionPeriod constructionPeriod, int estimateChapter)
        {
            WorkName = workName;
            TotalCost = totalCost;
            TotalCostIncludingContructionAndInstallationWorks = totalCostIncludingContructionAndInstallationWorks;
            ConstructionPeriod = constructionPeriod;
            EstimateChapter = estimateChapter;
        }
    }
}
