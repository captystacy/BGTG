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
            var totalCost = 1.111M;
            var totalCostIncludingContructionAndInstallationWorks = 2.222M;
            var percentages = new List<decimal> { 0.3M, 0.5M, 0.2M };
            var constructionPeriod = constructionPeriodCreator.Create(initialDate, totalCost, totalCostIncludingContructionAndInstallationWorks, percentages);

            Assert.AreEqual(initialDate, constructionPeriod.ConstructionMonths[0].Date);
            Assert.AreEqual(0.3333, constructionPeriod.ConstructionMonths[0].InvestmentVolume);
            Assert.AreEqual(0.6666, constructionPeriod.ConstructionMonths[0].ContructionAndInstallationWorksVolume);
            Assert.AreEqual(percentages[0], constructionPeriod.ConstructionMonths[0].PercentPart);
            Assert.AreEqual(0, constructionPeriod.ConstructionMonths[0].CreationIndex);

            Assert.AreEqual(initialDate.AddMonths(1), constructionPeriod.ConstructionMonths[1].Date);
            Assert.AreEqual(0.5555, constructionPeriod.ConstructionMonths[1].InvestmentVolume);
            Assert.AreEqual(1.111, constructionPeriod.ConstructionMonths[1].ContructionAndInstallationWorksVolume);
            Assert.AreEqual(percentages[1], constructionPeriod.ConstructionMonths[1].PercentPart);
            Assert.AreEqual(1, constructionPeriod.ConstructionMonths[1].CreationIndex);

            Assert.AreEqual(initialDate.AddMonths(2), constructionPeriod.ConstructionMonths[2].Date);
            Assert.AreEqual(0.2222, constructionPeriod.ConstructionMonths[2].InvestmentVolume);
            Assert.AreEqual(0.4444, constructionPeriod.ConstructionMonths[2].ContructionAndInstallationWorksVolume);
            Assert.AreEqual(percentages[2], constructionPeriod.ConstructionMonths[2].PercentPart);
            Assert.AreEqual(2, constructionPeriod.ConstructionMonths[2].CreationIndex);
        }

        [Test]
        public void Create_PercentIsNotInRangeFrom0To1_DoNotCreateConstructionMonth()
        {
            var constructionPeriodCreator = CreateDefaultConstructionPeriodCreator();
            var initialDate = new DateTime(1999, 9, 21);
            var totalCost = 1.111M;
            var totalCostIncludingContructionAndInstallationWorks = 2.222M;
            var percentages = new List<decimal> { 0, 1.01M, 0.3M };
            var constructionPeriod = constructionPeriodCreator.Create(initialDate, totalCost, totalCostIncludingContructionAndInstallationWorks, percentages);

            Assert.AreEqual(initialDate.AddMonths(2), constructionPeriod.ConstructionMonths[0].Date);
            Assert.AreEqual(0.3333, constructionPeriod.ConstructionMonths[0].InvestmentVolume);
            Assert.AreEqual(0.6666, constructionPeriod.ConstructionMonths[0].ContructionAndInstallationWorksVolume);
            Assert.AreEqual(percentages[2], constructionPeriod.ConstructionMonths[0].PercentPart);
            Assert.AreEqual(2, constructionPeriod.ConstructionMonths[0].CreationIndex);
        }
    }
}
