using Moq;
using NUnit.Framework;
using POSCore.CalendarPlanLogic;
using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;
using System;

namespace POSCoreTests.CalendarPlanLogic.CalendarWorkCreatorTests
{
    public class CreateCalendarWorkShould
    {
        private ICalendarWorkCreator _calendarWorkCreator;
        private Mock<IConstructionPeriodCreator> _constructionPeriodCreatorMock;

        [SetUp]
        public void SetUp()
        {
            _constructionPeriodCreatorMock = new Mock<IConstructionPeriodCreator>();
            _calendarWorkCreator = new CalendarWorkCreator(_constructionPeriodCreatorMock.Object);
        }

        [Test]
        public void NotReturnNull()
        {
            var estimateWork = new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", 0, 0, 0, 0);

            var calendarWork = _calendarWorkCreator.CreateCalendarWork(DateTime.Today, estimateWork, new int[] { 0 });

            Assert.NotNull(calendarWork);
        }

        [Test]
        public void ReturnCalendarWork_InWhichSetWorkNameFromEstimateWorkName()
        {
            var estimateWork = new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", 0, 0, 0, 0);

            var calendarWork = _calendarWorkCreator.CreateCalendarWork(DateTime.Today, estimateWork, new int[] { 0 });

            Assert.AreEqual(estimateWork.WorkName, calendarWork.WorkName);
        }

        [Test]
        public void ReturnCalendarWork_InWhichSetTotalCostFromEstimateTotalСost()
        {
            var estimateWork = new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", 0, 0, 0, 0);

            var calendarWork = _calendarWorkCreator.CreateCalendarWork(DateTime.Today, estimateWork, new int[] { 0 });

            Assert.AreEqual(estimateWork.TotalCost, calendarWork.TotalCost);
        }

        [Test]
        public void ReturnCalendarWork_InWhichSetTotalCostIncludingContructionAndInstallationWorks_ToTotalCostMinusEquipmentCostAndOtherProductsCost()
        {
            var estimateWork = new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", (decimal)2.027, (decimal)1.541, (decimal)55.464, 0);

            var calendarWork = _calendarWorkCreator.CreateCalendarWork(DateTime.Today, estimateWork, new int[] { 0 });

            Assert.AreEqual(estimateWork.TotalCost - estimateWork.EquipmentCost - estimateWork.OtherProductsCost, calendarWork.TotalCostIncludingContructionAndInstallationWorks);
        }

        [Test]
        public void ReturnCalendarWork_InWhichSetEstimateChapterFromEstimateChapter()
        {
            var estimateWork = new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", 0, 0, 0, 1);

            var calendarWork = _calendarWorkCreator.CreateCalendarWork(DateTime.Today, estimateWork, new int[] { 0 });

            Assert.AreEqual(estimateWork.Chapter, calendarWork.EstimateChapter);
        }

        [Test]
        public void CreateCalendarPeriod()
        {
            var estimateWork = new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", 0, 0, (decimal)11.324, 0);
            var initialDate = new DateTime(1999, 9, 21);
            var percentages = new int[] { 1 };

            var calendarWork = _calendarWorkCreator.CreateCalendarWork(initialDate, estimateWork, percentages);

            _constructionPeriodCreatorMock.Verify(x => x.CreateConstructionPeriod(initialDate, calendarWork.TotalCost, calendarWork.TotalCostIncludingContructionAndInstallationWorks, percentages), Times.Once);
        }
    }
}
