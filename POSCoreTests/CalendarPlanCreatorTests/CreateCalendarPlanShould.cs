using Moq;
using NUnit.Framework;
using POSCore.CalendarPlanLogic;
using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;
using POSCore.EstimateLogic.Interfaces;
using System.Linq;

namespace POSCoreTests.CalendarPlanCreatorTests
{
    public class CreateCalendarPlanShould
    {
        private ICalendarPlanCreator _calendarPlanCreator;
        private Estimate _estimateVatFree;
        private Estimate _estimateVat;
        private Mock<IEstimateConnector> _estimateConnectorMock;

        [SetUp]
        public void SetUp()
        {
            _estimateVatFree = new Estimate(Enumerable.Empty<EstimateWork>());
            _estimateVat = new Estimate(Enumerable.Empty<EstimateWork>());
            _estimateConnectorMock = new Mock<IEstimateConnector>();
            _calendarPlanCreator = new CalendarPlanCreator(_estimateVatFree, _estimateVat, _estimateConnectorMock.Object);
        }

        [Test]
        public void NotReturnNull()
        {
            var calendarPlan = _calendarPlanCreator.CreateCalendarPlan();

            Assert.NotNull(calendarPlan);
        }
    }
}
