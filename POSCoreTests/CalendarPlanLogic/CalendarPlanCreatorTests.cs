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

        [Test]
        public void Create_DefaultEstimate_CorrectCalendarPlan()
        {
            var calendarPlanCreator = CreateDefaultCalendarPlanCreator();
            var preparatoryEstimateWorks = new List<EstimateWork>();
            var mainEstimateWorks = new List<EstimateWork>();
            var constructionStartDate = new DateTime(1999, 9, 21);
            var construcitonDuration = 3;
            var estimate = new Estimate(preparatoryEstimateWorks, mainEstimateWorks, constructionStartDate, construcitonDuration, null, 0);
            var otherExpensesPercentages = new List<decimal>();
            var lastPreparatoryCalendarWork = new CalendarWork("", 0, 0, null, 0);
            var preparatoryCalendarWorks = new List<CalendarWork> 
            { 
                new CalendarWork("", 0, 0, null, 0),
                new CalendarWork("", 0, 0, null, 0), 
                lastPreparatoryCalendarWork
            };
            _calendarWorkCreator.Setup(x => x.CreatePreparatoryCalendarWorks(preparatoryEstimateWorks, constructionStartDate)).Returns(preparatoryCalendarWorks);
            var mainCalendarWorks = new List<CalendarWork>();
            _calendarWorkCreator.Setup(x => x.CreateMainCalendarWorks(mainEstimateWorks, lastPreparatoryCalendarWork, constructionStartDate,
                construcitonDuration, otherExpensesPercentages)).Returns(mainCalendarWorks);

            var calendarPlan = calendarPlanCreator.Create(estimate, otherExpensesPercentages);

            _calendarWorkCreator.Verify(x => x.CreatePreparatoryCalendarWorks(preparatoryEstimateWorks, constructionStartDate), Times.Once);
            _calendarWorkCreator.Verify(x => x.CreateMainCalendarWorks(mainEstimateWorks, lastPreparatoryCalendarWork, constructionStartDate, 
                construcitonDuration, otherExpensesPercentages), Times.Once);

            Assert.AreEqual(preparatoryCalendarWorks, calendarPlan.PreparatoryCalendarWorks);
            Assert.AreEqual(mainCalendarWorks, calendarPlan.MainCalendarWorks);
            Assert.AreEqual(construcitonDuration, calendarPlan.ConstructionDuration);
            Assert.AreEqual(constructionStartDate, calendarPlan.ConstructionStartDate);
        }
    }
}
