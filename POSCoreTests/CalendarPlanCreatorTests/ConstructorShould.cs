using Moq;
using NUnit.Framework;
using POSCore.CalendarPlanLogic;
using POSCore.EstimateLogic;
using POSCore.EstimateLogic.Interfaces;
using System.Linq;

namespace POSCoreTests.CalendarPlanCreatorTests
{
    public class ConstructorShould
    {
        private Mock<IEstimateConnector> _estimateConnectorMock;

        [SetUp]
        public void SetUp()
        {
            _estimateConnectorMock = new Mock<IEstimateConnector>();
        }

        [Test]
        public void ConnectEstimates()
        {
            var estimateVatFree = new Estimate(Enumerable.Empty<EstimateWork>());
            var estimateVat = new Estimate(Enumerable.Empty<EstimateWork>());

            new CalendarPlanCreator(estimateVatFree, estimateVat, _estimateConnectorMock.Object);

            _estimateConnectorMock.Verify(x => x.Connect(estimateVatFree, estimateVat), Times.Once);
        }
    }
}
