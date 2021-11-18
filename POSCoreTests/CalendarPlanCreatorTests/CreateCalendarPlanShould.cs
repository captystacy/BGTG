using Moq;
using NUnit.Framework;
using POSCore.CalendarPlanLogic;
using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POSCoreTests.CalendarPlanCreatorTests
{
    public class CreateCalendarPlanShould
    {
        private ICalendarPlanCreator _calendarPlanCreator;
        private Mock<ICalendarWorkCreator> _calendarWorkCreator;

        [SetUp]
        public void SetUp()
        {
            _calendarWorkCreator = new Mock<ICalendarWorkCreator>();
        }

        [Test]
        public void ReturnNull_IfEstimateWorksCountWasZero()
        {
            var initialDate = DateTime.Today;
            var percentagesGroups = new List<int[]>() { new int[] { 1, 2, 3 } };
            var estimate = new Estimate(Enumerable.Empty<EstimateWork>());

            _calendarPlanCreator = new CalendarPlanCreator(estimate, _calendarWorkCreator.Object);
            var calendarPlan = _calendarPlanCreator.CreateCalendarPlan(initialDate, percentagesGroups);

            Assert.Null(calendarPlan);
        }

        [Test]
        public void ReturnNull_IfPercentagesGroupsCountWasZero()
        {
            var initialDate = DateTime.Today;
            var percentagesGroups = new List<int[]>();
            var estimate = new Estimate(new EstimateWork[] { new EstimateWork("", 0, 0, 0) });

            _calendarPlanCreator = new CalendarPlanCreator(estimate, _calendarWorkCreator.Object);
            var calendarPlan = _calendarPlanCreator.CreateCalendarPlan(initialDate, percentagesGroups);

            Assert.Null(calendarPlan);
        }

        [Test]
        public void ReturnNull_IfEstimateWorksCountNotEqualPercentagesGroupsCount()
        {
            var initialDate = DateTime.Today;
            var percentagesGroups = new List<int[]>() { new int[] { }, new int[] { } };
            var estimate = new Estimate(new EstimateWork[] { new EstimateWork("", 0, 0, 0) });

            _calendarPlanCreator = new CalendarPlanCreator(estimate, _calendarWorkCreator.Object);
            var calendarPlan = _calendarPlanCreator.CreateCalendarPlan(initialDate, percentagesGroups);

            Assert.Null(calendarPlan);
        }

        [Test]
        public void ReturnCalendarPlan_InWhichCreateCalendarWorkFromEstimateWorkAndPercentageGroupInOrder()
        {
            var initialDate = DateTime.Today;
            var percentagesGroups = new List<int[]>();
            var estimateWorks = new List<EstimateWork>();
            var countOfWorks = 3;
            for (int i = 0; i < countOfWorks; i++)
            {
                percentagesGroups.Add(new int[] { });
                estimateWorks.Add(new EstimateWork("", 0, 0, 0));
            }
            var estimate = new Estimate(estimateWorks);

            _calendarPlanCreator = new CalendarPlanCreator(estimate, _calendarWorkCreator.Object);
            var calendarPlan = _calendarPlanCreator.CreateCalendarPlan(initialDate, percentagesGroups);

            for (int i = 0; i < countOfWorks; i++)
            {
                _calendarWorkCreator.Verify(x => x.CreateCalendarWork(initialDate, estimateWorks[i], percentagesGroups[i]), Times.Once);
            }
        }
    }
}
