using System;

namespace POSCore.CalendarPlanLogic
{
    public class ConstructionMonth
    {
        public DateTime Date { get; }
        public decimal? InvestmentVolume { get; }
        public decimal? ContructionAndInstallationWorksVolume { get; }
        public int PercentePart { get; }

        public ConstructionMonth(DateTime date, decimal? investmentVolume, decimal? contructionAndInstallationWorksVolume, int percentePart)
        {
            Date = date;
            InvestmentVolume = investmentVolume;
            ContructionAndInstallationWorksVolume = contructionAndInstallationWorksVolume;
            PercentePart = percentePart;
        }
    }
}
