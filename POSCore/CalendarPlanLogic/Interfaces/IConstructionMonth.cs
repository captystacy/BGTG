namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface IConstructionMonth
    {
        string Month { get; }
        int Year { get; }
        double InvestmentVolume { get; }
        double ContructionAndInstallationWorksVolume { get; }
        int PercentePart { get; }
    }
}
