using POSCore.CalendarPlanLogic.Interfaces;
using System;
using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic
{
    public class ConstructionPeriodCreator : IConstructionPeriodCreator
    {
        public ConstructionPeriod Create(DateTime initialDate, decimal totalCost, decimal totalCostIncludingContructionAndInstallationWorks, List<decimal> percentages)
        {
            var constructionMonths = new List<ConstructionMonth>();

            for (int i = 0; i < percentages.Count; i++)
            {
                var percent = percentages[i];
                if (percent <= 0 || percent > 1)
                {
                    continue;
                }

                var date = initialDate.AddMonths(i);
                var investmentVolume = decimal.Round(totalCost * percent, 3);
                var constructionAndInstallationWorksVolume = decimal.Round(totalCostIncludingContructionAndInstallationWorks * percent, 3);
                var constructionMonth = new ConstructionMonth(date, investmentVolume, constructionAndInstallationWorksVolume, percent, i);
                constructionMonths.Add(constructionMonth);
            }

            return new ConstructionPeriod(constructionMonths);
        }
    }
}
