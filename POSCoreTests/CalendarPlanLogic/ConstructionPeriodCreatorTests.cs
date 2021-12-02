using NUnit.Framework;
using POSCore.CalendarPlanLogic;
using System;
using System.Collections.Generic;

namespace POSCoreTests.CalendarPlanLogic
{
    public class ConstructionPeriodCreatorTests
    {
        private ConstructionPeriodCreator CreateDefaultConstructionPeriodCreator()
        {
            return new ConstructionPeriodCreator();
        }

        [Test]
        public void Create_CorrectArgs_CorrectConstructionPeriod()
        {
            var constructionPeriodCreator = CreateDefaultConstructionPeriodCreator();
            var initialDate = new DateTime(1999, 9, 21);
            var totalCost = (decimal)1.111;
            var totalCostIncludingContructionAndInstallationWorks = (decimal)2.222;
            var percentages = new List<decimal> { (decimal)0.3, (decimal)0.5, (decimal)0.2 };
            var constructionPeriod = constructionPeriodCreator.Create(initialDate, totalCost, totalCostIncludingContructionAndInstallationWorks, percentages);

            Assert.AreEqual(initialDate, constructionPeriod.ConstructionMonths[0].Date);
            Assert.AreEqual((decimal)0.333, constructionPeriod.ConstructionMonths[0].InvestmentVolume);
            Assert.AreEqual((decimal)0.667, constructionPeriod.ConstructionMonths[0].ContructionAndInstallationWorksVolume);
            Assert.AreEqual(percentages[0], constructionPeriod.ConstructionMonths[0].PercentePart);
            Assert.AreEqual(0, constructionPeriod.ConstructionMonths[0].CreationIndex);

            Assert.AreEqual(initialDate.AddMonths(1), constructionPeriod.ConstructionMonths[1].Date);
            Assert.AreEqual((decimal)0.556, constructionPeriod.ConstructionMonths[1].InvestmentVolume);
            Assert.AreEqual((decimal)1.111, constructionPeriod.ConstructionMonths[1].ContructionAndInstallationWorksVolume);
            Assert.AreEqual(percentages[1], constructionPeriod.ConstructionMonths[1].PercentePart);
            Assert.AreEqual(1, constructionPeriod.ConstructionMonths[1].CreationIndex);

            Assert.AreEqual(initialDate.AddMonths(2), constructionPeriod.ConstructionMonths[2].Date);
            Assert.AreEqual((decimal)0.222, constructionPeriod.ConstructionMonths[2].InvestmentVolume);
            Assert.AreEqual((decimal)0.444, constructionPeriod.ConstructionMonths[2].ContructionAndInstallationWorksVolume);
            Assert.AreEqual(percentages[2], constructionPeriod.ConstructionMonths[2].PercentePart);
            Assert.AreEqual(2, constructionPeriod.ConstructionMonths[2].CreationIndex);
        }

        [Test]
        public void Create_PercentIsNotInRangeFrom0To1_DoNotCreateConstructionMonth()
        {
            var constructionPeriodCreator = CreateDefaultConstructionPeriodCreator();
            var initialDate = new DateTime(1999, 9, 21);
            var totalCost = (decimal)1.111;
            var totalCostIncludingContructionAndInstallationWorks = (decimal)2.222;
            var percentages = new List<decimal> { 0, (decimal)1.01, (decimal)0.3 };
            var constructionPeriod = constructionPeriodCreator.Create(initialDate, totalCost, totalCostIncludingContructionAndInstallationWorks, percentages);

            Assert.AreEqual(initialDate.AddMonths(2), constructionPeriod.ConstructionMonths[0].Date);
            Assert.AreEqual((decimal)0.333, constructionPeriod.ConstructionMonths[0].InvestmentVolume);
            Assert.AreEqual((decimal)0.667, constructionPeriod.ConstructionMonths[0].ContructionAndInstallationWorksVolume);
            Assert.AreEqual(percentages[2], constructionPeriod.ConstructionMonths[0].PercentePart);
            Assert.AreEqual(2, constructionPeriod.ConstructionMonths[0].CreationIndex);
        }
    }
}
