using POSCore.CalendarPlanLogic.Interfaces;
using System;
using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic
{
    public class ConstructionPeriodCreator : IConstructionPeriodCreator
    {
        public ConstructionPeriod CreateConstructionPeriod(DateTime initialDate, decimal totalCost, decimal totalCostIncludingContructionAndInstallationWorks, int[] percentages)
        {
            var constructionMonths = new List<ConstructionMonth>();

            for (int i = 0; i < percentages.Length; i++)
            {
                int percent = percentages[i];
                var date = initialDate.AddMonths(i);
                var investmentVolume = totalCost * percent / 100;
                var constructionAndInstallationWorksVolume = totalCostIncludingContructionAndInstallationWorks * percent / 100;
                var constructionMonth = new ConstructionMonth(date, investmentVolume, constructionAndInstallationWorksVolume, percent);
                constructionMonths.Add(constructionMonth);
            }

            return new ConstructionPeriod(constructionMonths);
        }
    }
}
