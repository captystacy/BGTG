using Moq;
using NUnit.Framework;
using POSCore.CalendarPlanLogic;
using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;
using System;
using System.Collections.Generic;

namespace POSCoreTests.CalendarPlanLogic
{
    public class CalendarPlanCreatorTests
    {
        private Mock<ICalendarWorkCreator> _calendarWorkCreator;

        private CalendarPlanCreator CreateDefaultCalendarPlanCreator()
        {
            _calendarWorkCreator = new Mock<ICalendarWorkCreator>();
            return new CalendarPlanCreator(_calendarWorkCreator.Object);
        }

        private Estimate CreatDefaultEstimate()
        {
            return new Estimate(new List<EstimateWork>
            {
                new EstimateWork("ВЫНОС ТРАССЫ В НАТУРУ", 0, (decimal)0.013, (decimal)0.013, 1),
                new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", (decimal)0.04, 0, (decimal)0.632, 2),
                new EstimateWork("БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ", 0, 0, (decimal)0.02, 7),
                new EstimateWork("ОДД НА ПЕРИОД ПРОИЗВОДСТВА РАБОТ", 0, 0, (decimal)0.005, 8),
                new EstimateWork("ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 8,56Х0,93 - 7,961%", 0, 0, (decimal)0.012, 8),
                new EstimateWork("ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ", (decimal)0.041, (decimal)2.536, (decimal)3.226, 10),
            }, new DateTime(2022, 8, 1), (decimal)0.7);
        }

        [Test]
        public void Create_DefaultEstimate_CorrectCalendarPlan()
        {
            var calendarPlanCreator = CreateDefaultCalendarPlanCreator();
            var estimate = CreatDefaultEstimate();

            var expectedCalendarWorks = new List<CalendarWork>();
            foreach (var estimateWork in estimate.EstimateWorks)
            {
                var calendarWork = new CalendarWork(estimateWork.WorkName, 0, 0, null, 0);
                expectedCalendarWorks.Add(calendarWork);
                _calendarWorkCreator.Setup(x => x.Create(estimate.ConstructionStartDate, estimateWork)).Returns(calendarWork);
            }

            var calendarPlan = calendarPlanCreator.Create(estimate);

            for (int i = 0; i < expectedCalendarWorks.Count; i++)
            {
                Assert.AreEqual(expectedCalendarWorks[i], calendarPlan.CalendarWorks[i]);
            }

            Assert.AreEqual(estimate.ConstructionStartDate, calendarPlan.ConstructionStartDate);
            Assert.AreEqual(estimate.ConstructionDuration, calendarPlan.ConstructionDuration);
        }
    }
}
