using Moq;
using NUnit.Framework;
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
            var estimateWork = new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", (decimal)1.111, (decimal)1.222, (decimal)3.333, 2);
            estimateWork.Percentages = new List<decimal> { (decimal)0.3, (decimal)0.5, (decimal)0.2 };
            var constructionPeriod = new ConstructionPeriod(new List<ConstructionMonth>());
            _constructionPeriodCreatorMock.Setup(x => x.Create(initialDate, estimateWork.TotalCost, 1, estimateWork.Percentages)).Returns(constructionPeriod);

            var calendarWork = calendarWorkCreator.Create(initialDate, estimateWork);

            Assert.AreEqual(estimateWork.WorkName, calendarWork.WorkName);
            Assert.AreEqual((decimal)3.333, calendarWork.TotalCost);
            Assert.AreEqual(1, calendarWork.TotalCostIncludingContructionAndInstallationWorks);
            Assert.AreEqual(2, calendarWork.EstimateChapter);
            Assert.AreEqual(constructionPeriod, calendarWork.ConstructionPeriod);
        }

        [Test]
        public void Create_EstimateWorkPercentagesInNull_ConstructionPeriodNull()
        {
            var calendarWorkCreator = CreateDefaultCalendarWorkCreator();
            var initialDate = new DateTime(1999, 9, 21);
            var estimateWork = new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", (decimal)1.111, (decimal)1.222, (decimal)3.333, 2);
            var constructionPeriod = new ConstructionPeriod(new List<ConstructionMonth>());
            _constructionPeriodCreatorMock.Setup(x => x.Create(initialDate, estimateWork.TotalCost, 1, estimateWork.Percentages)).Returns(constructionPeriod);

            var calendarWork = calendarWorkCreator.Create(initialDate, estimateWork);

            Assert.AreEqual(estimateWork.WorkName, calendarWork.WorkName);
            Assert.AreEqual((decimal)3.333, calendarWork.TotalCost);
            Assert.AreEqual(1, calendarWork.TotalCostIncludingContructionAndInstallationWorks);
            Assert.AreEqual(2, calendarWork.EstimateChapter);
            Assert.AreEqual(null, calendarWork.ConstructionPeriod);
        }
    }
}
