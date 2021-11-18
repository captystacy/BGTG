using Moq;
using NUnit.Framework;
using POSCore.CalendarPlanLogic;
using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;
using POSCore.EstimateLogic.Interfaces;
using System.Linq;

namespace POSCoreTests.CalendarPlanCreatorTests
{
    public class ConstructorShould
    {
        private Mock<IEstimateConnector> _estimateConnectorMock;
        private Mock<ICalendarWorkCreator> _calendarWorkCreator;

        [SetUp]
        public void SetUp()
        {
            _estimateConnectorMock = new Mock<IEstimateConnector>();
            _calendarWorkCreator = new Mock<ICalendarWorkCreator>();
        }

        [Test]
        public void ConnectEstimates_IfTwoEstimatesPassed()
        {
            var estimateVatFree = new Estimate(Enumerable.Empty<EstimateWork>());
            var estimateVat = new Estimate(Enumerable.Empty<EstimateWork>());

            new CalendarPlanCreator(estimateVatFree, estimateVat, _estimateConnectorMock.Object, _calendarWorkCreator.Object);

            _estimateConnectorMock.Verify(x => x.Connect(estimateVatFree, estimateVat), Times.Once);
        }
    }
}
