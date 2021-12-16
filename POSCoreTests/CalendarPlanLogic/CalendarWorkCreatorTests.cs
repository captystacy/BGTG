using Moq;
using NUnit.Framework;
using OfficeOpenXml.ConditionalFormatting;
using POSCore.CalendarPlanLogic;
using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;
using System;
using System.Collections.Generic;

namespace POSCoreTests.CalendarPlanLogic
{
    public class CalendarWorkCreatorTests
    {
        private Mock<IConstructionPeriodCreator> _constructionPeriodCreatorMock;

        private CalendarWorkCreator CreateDefaultCalendarWorkCreator()
        {
            _constructionPeriodCreatorMock = new Mock<IConstructionPeriodCreator>();
            return new CalendarWorkCreator(_constructionPeriodCreatorMock.Object);
        }

        [Test]
        public void Create_CorrectArgs_CorrectCalendarWork()
        {
            var calendarWorkCreator = CreateDefaultCalendarWorkCreator();
            var initialDate = new DateTime(1999, 9, 21);
            var estimateWork = new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", 1.111M, 1.222M, 3.333M, 2);
            estimateWork.Percentages = new List<decimal> { 0.3M, 0.5M, 0.2M };
            var constructionPeriod = new ConstructionPeriod(new List<ConstructionMonth>());
            _constructionPeriodCreatorMock.Setup(x => x.Create(initialDate, estimateWork.TotalCost, 1, estimateWork.Percentages)).Returns(constructionPeriod);

            var calendarWork = calendarWorkCreator.Create(estimateWork, initialDate);

            Assert.AreEqual(estimateWork.WorkName, calendarWork.WorkName);
            Assert.AreEqual(3.333, calendarWork.TotalCost);
            Assert.AreEqual(1, calendarWork.TotalCostIncludingContructionAndInstallationWorks);
            Assert.AreEqual(2, calendarWork.EstimateChapter);
            Assert.AreEqual(constructionPeriod, calendarWork.ConstructionPeriod);
        }

        private List<EstimateWork> CreateDefaultPreparatoryEstimateWorks()
        {
            return new List<EstimateWork>
            {
                new EstimateWork("ТРАССИРОВКА КАНАЛОВ (8,04 КМ)", 0.1M, 0.1M, 1, 1, new List<decimal> { 1 }),
                new EstimateWork("НЕ БУДЕТ ВКЛЮЧЕНА", 0.1M, 0.1M, 0.2M, 1, new List<decimal> { 1 }),
                new EstimateWork("ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 9,1Х0,93 - 8,463%", 0.1M, 0.1M, 1M, 8, new List<decimal> { 1 }),
            };
        }

        private List<EstimateWork> CreateDefaultMainEstimateWorks()
        {
            return new List<EstimateWork>
            {
                new EstimateWork("БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ", 0.1M, 0.1M, 1, 2),
                new EstimateWork("СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ", 0.1M, 0.1M, 1, 3),
                new EstimateWork("ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ", 0.1M, 0.1M, 1, 10),
            };
        }

        [Test]
        public void CreatePreparatoryCalendarWorks_DefaultPreparatoryCalendarWorks_CorrectPreparatoryCalendarWorks()
        {
            var calendarWorkCreator = CreateDefaultCalendarWorkCreator();
            var preparatoryEstimateWorks = CreateDefaultPreparatoryEstimateWorks();
            var constructionStartDate = new DateTime(1999, 9, 21);
            var constructionPeriod = new ConstructionPeriod(new List<ConstructionMonth> { new ConstructionMonth(constructionStartDate, 0, 0, 0, 0) });
            _constructionPeriodCreatorMock.Setup(x => x.Create(constructionStartDate, It.IsAny<decimal>(), It.IsAny<decimal>(),
                It.IsAny<List<decimal>>())).Returns(constructionPeriod);

            var preparatoryCalendarWorks = calendarWorkCreator.CreatePreparatoryCalendarWorks(preparatoryEstimateWorks, constructionStartDate);

            AssertCalendarWork(preparatoryCalendarWorks[0], "Подготовка территории строительства", 1, 0.8M, 1, constructionPeriod);
            AssertCalendarWork(preparatoryCalendarWorks[1], "Временные здания и сооружения", 1, 0.8M, 8, constructionPeriod);
            AssertCalendarWork(preparatoryCalendarWorks[2], "Итого:", 2, 1.6M, 1, constructionPeriod);
            _constructionPeriodCreatorMock.Verify(x => x.Create(constructionStartDate, It.IsAny<decimal>(), It.IsAny<decimal>(),
                It.IsAny<List<decimal>>()), Times.Exactly(5));
        }

        private void AssertCalendarWork(CalendarWork calendarWork, string workName, decimal totalCost,
            decimal totalCostIncludingContructionAndInstallationWorks, int estimateChapter, ConstructionPeriod constructionPeriod)
        {
            Assert.AreEqual(workName, calendarWork.WorkName);
            Assert.AreEqual(totalCost, calendarWork.TotalCost);
            Assert.AreEqual(totalCostIncludingContructionAndInstallationWorks, calendarWork.TotalCostIncludingContructionAndInstallationWorks);
            Assert.AreEqual(estimateChapter, calendarWork.EstimateChapter);

            for (int i = 0; i < calendarWork.ConstructionPeriod.ConstructionMonths.Count; i++)
            {
                Assert.AreEqual(constructionPeriod.ConstructionMonths[i].InvestmentVolume, calendarWork.ConstructionPeriod.ConstructionMonths[i].InvestmentVolume);
                Assert.AreEqual(constructionPeriod.ConstructionMonths[i].ContructionAndInstallationWorksVolume, calendarWork.ConstructionPeriod.ConstructionMonths[i].ContructionAndInstallationWorksVolume);
                Assert.AreEqual(constructionPeriod.ConstructionMonths[i].PercentPart, calendarWork.ConstructionPeriod.ConstructionMonths[i].PercentPart);
                Assert.AreEqual(constructionPeriod.ConstructionMonths[i].Date, calendarWork.ConstructionPeriod.ConstructionMonths[i].Date);
                Assert.AreEqual(constructionPeriod.ConstructionMonths[i].CreationIndex, calendarWork.ConstructionPeriod.ConstructionMonths[i].CreationIndex);
            }
        }

        [Test]
        public void CreateMainCalendarWorks_DefaultMainCalendarWorks_CorrectMainCalendarWorks()
        {
            var calendarWorkCreator = CreateDefaultCalendarWorkCreator();
            var mainEstimateWorks = CreateDefaultMainEstimateWorks();
            var constructionStartDate = new DateTime(1999, 9, 21);
            var constructionPeriod = new ConstructionPeriod(new List<ConstructionMonth>
            {
                new ConstructionMonth(constructionStartDate, 0, 0, 0, 0),
                new ConstructionMonth(constructionStartDate.AddMonths(1), 0, 0, 0, 1),
            });
            var constructionDuration = 2;
            _constructionPeriodCreatorMock.Setup(x => x.Create(constructionStartDate, It.IsAny<decimal>(), It.IsAny<decimal>(),
                It.IsAny<List<decimal>>())).Returns(constructionPeriod);
            var preparatoryTotalWork = new CalendarWork(null, 1, 0.8M, new ConstructionPeriod(new List<ConstructionMonth> { new ConstructionMonth(constructionStartDate, 0, 0, 0, 0) }), 1);
            var otherExpensesPercentages = new List<decimal> { 0.7M, 0.3M };

            var mainCalendarWorks = calendarWorkCreator.CreateMainCalendarWorks(mainEstimateWorks, preparatoryTotalWork,
                constructionStartDate, constructionDuration, otherExpensesPercentages);

            AssertCalendarWork(mainCalendarWorks[0], "Работы, выполняемые в подготовительный период", 1, 0.8M, 2, constructionPeriod);
            AssertCalendarWork(mainCalendarWorks[1], "БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ", 1, 0.8M, 2, constructionPeriod);
            AssertCalendarWork(mainCalendarWorks[2], "СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ", 1, 0.8M, 3, constructionPeriod);
            AssertCalendarWork(mainCalendarWorks[3], "Прочие работы и затраты", -2, -1.6M, 9, constructionPeriod);
            AssertCalendarWork(mainCalendarWorks[4], "Итого:", 1, 0.8M, 10, constructionPeriod);
            _constructionPeriodCreatorMock.Verify(x => x.Create(constructionStartDate, It.IsAny<decimal>(), It.IsAny<decimal>(),
                It.IsAny<List<decimal>>()), Times.Exactly(4));
        }
    }
}
