using Moq;
using NUnit.Framework;
using POSCore.CalendarPlanLogic;
using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic.Interfaces;

namespace POSCoreTests.CalendarPlanCreatorTests
{
    public class CreateCalendarPlanShould
    {
        private ICalendarPlanCreator _calendarPlanCreator;
        private Mock<IEstimate> _estimateVatFree;
        private Mock<IEstimate> _estimateVat;
        private Mock<IEstimateConnector> _estimateConnectorMock;

        [SetUp]
        public void SetUp()
        {
            _estimateVatFree = new Mock<IEstimate>();
            _estimateVat = new Mock<IEstimate>();
            _estimateConnectorMock = new Mock<IEstimateConnector>();
            _calendarPlanCreator = new CalendarPlanCreator(_estimateVatFree.Object, _estimateVat.Object, _estimateConnectorMock.Object);
        }

        [Test]
        public void NotReturnNull()
        {
            var calendarPlan = _calendarPlanCreator.CreateCalendarPlan();

            Assert.NotNull(calendarPlan);
        }
    }
}
