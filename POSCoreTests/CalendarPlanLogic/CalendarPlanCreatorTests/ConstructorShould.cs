using Moq;
using NUnit.Framework;
using POSCore.CalendarPlanLogic;
using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;
using POSCore.EstimateLogic.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace POSCoreTests.CalendarPlanLogic.CalendarPlanCreatorTests
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
            var estimates = new List<Estimate>
            {
                new Estimate(new List<EstimateWork>()),
                new Estimate(new List<EstimateWork>()),
            };

            new CalendarPlanCreator(estimates, _estimateConnectorMock.Object, _calendarWorkCreator.Object);

            _estimateConnectorMock.Verify(x => x.Connect(estimates), Times.Once);
        }
    }
}
