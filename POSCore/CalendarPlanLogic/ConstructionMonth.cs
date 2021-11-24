using System;

namespace POSCore.CalendarPlanLogic
{
    public class ConstructionMonth
    {
        public DateTime Date { get; }
        public decimal? InvestmentVolume { get; }
        public decimal? ContructionAndInstallationWorksVolume { get; }
        public decimal PercentePart { get; }
        public int Index { get; }

        public ConstructionMonth(DateTime date, decimal? investmentVolume, decimal? contructionAndInstallationWorksVolume, decimal percentePart, int index)
        {
            Date = date;
            InvestmentVolume = investmentVolume;
            ContructionAndInstallationWorksVolume = contructionAndInstallationWorksVolume;
            PercentePart = percentePart;
            Index = index;
        }
    }
}
