using Moq;
using NUnit.Framework;
using POS.Infrastructure.Tools.CalendarPlanTool;
using POS.Infrastructure.Tools.CalendarPlanTool.Base;
using POS.Infrastructure.Tools.CalendarPlanTool.Models;
using POS.Infrastructure.Tools.EstimateTool.Models;
using POS.Tests.Infrastructure.Tools.EstimateTool;
using System.Collections.Generic;

namespace POS.Tests.Infrastructure.Tools.CalendarPlanTool
{
    public class CalendarPlanCreatorTests
    {
        private CalendarPlanCreator _calendarPlanCreator = null!;
        private Mock<ICalendarWorksProvider> _calendarWorksProvider = null!;

        [SetUp]
        public void SetUp()
        {
            _calendarWorksProvider = new Mock<ICalendarWorksProvider>();
            _calendarPlanCreator = new CalendarPlanCreator(_calendarWorksProvider.Object);
        }

        [Test]
        public void Create_Estimate548VAT_CorrectCalendarPlan()
        {
            var estimate = EstimateSource.Estimate548VAT;
            var otherExpensesPercentages = new List<decimal>();
            var totalPreparatoryCalendarWork = new CalendarWork(AppData.TotalWorkName, 0, 0, new List<ConstructionMonth>(), 0);

            var preparatoryCalendarWorks = new List<CalendarWork> { totalPreparatoryCalendarWork };
            _calendarWorksProvider
                .Setup(x => x.CreatePreparatoryCalendarWorks(estimate.PreparatoryEstimateWorks, estimate.ConstructionStartDate))
                .Returns(preparatoryCalendarWorks);

            var mainCalendarWorks = new List<CalendarWork>();
            _calendarWorksProvider
                .Setup(x => x.CreateMainCalendarWorks(estimate.MainEstimateWorks, totalPreparatoryCalendarWork,
                estimate.ConstructionStartDate, estimate.ConstructionDurationCeiling, otherExpensesPercentages, TotalWorkChapter.TotalWork1To12Chapter))
                .Returns(mainCalendarWorks);

            var expectedCalendarPlan = new CalendarPlan(preparatoryCalendarWorks, mainCalendarWorks, estimate.ConstructionStartDate, estimate.ConstructionDuration, estimate.ConstructionDurationCeiling);

            var actualCalendarPlan = _calendarPlanCreator.Create( estimate, otherExpensesPercentages, TotalWorkChapter.TotalWork1To12Chapter);

            Assert.AreEqual(expectedCalendarPlan, actualCalendarPlan);
            _calendarWorksProvider.Verify(x => x.CreatePreparatoryCalendarWorks(estimate.PreparatoryEstimateWorks, estimate.ConstructionStartDate), Times.Once);
            _calendarWorksProvider.Verify(x => x.CreateMainCalendarWorks(estimate.MainEstimateWorks, totalPreparatoryCalendarWork,
                estimate.ConstructionStartDate, estimate.ConstructionDurationCeiling, otherExpensesPercentages, TotalWorkChapter.TotalWork1To12Chapter), Times.Once);
        }
    }
}
