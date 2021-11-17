using System;

namespace POSCore.CalendarPlanLogic
{
    public class ConstructionMonth
    {
        public DateTime Date { get; }
        public double InvestmentVolume { get; }
        public double ContructionAndInstallationWorksVolume { get; }
        public int PercentePart { get; }

        public ConstructionMonth(DateTime date, double investmentVolume, double contructionAndInstallationWorksVolume, int percentePart)
        {
            Date = date;
            InvestmentVolume = investmentVolume;
            ContructionAndInstallationWorksVolume = contructionAndInstallationWorksVolume;
            PercentePart = percentePart;
        }
    }
}
