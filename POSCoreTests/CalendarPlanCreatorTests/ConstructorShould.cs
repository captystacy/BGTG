using Moq;
using NUnit.Framework;
using POSCore.CalendarPlanLogic;
using POSCore.EstimateLogic.Interfaces;

namespace POSCoreTests.CalendarPlanCreatorTests
{
    public class ConstructorShould
    {
        private Mock<IEstimate> _estimateVatFree;
        private Mock<IEstimate> _estimateVat;
        private Mock<IEstimateConnector> _estimateConnectorMock;

        [SetUp]
        public void SetUp()
        {
            _estimateVatFree = new Mock<IEstimate>();
            _estimateVat = new Mock<IEstimate>();
            _estimateConnectorMock = new Mock<IEstimateConnector>();
        }

        [Test]
        public void ConnectEstimates()
        {
            new CalendarPlanCreator(_estimateVatFree.Object, _estimateVat.Object, _estimateConnectorMock.Object);
            _estimateConnectorMock.Verify(x => x.Connect(_estimateVatFree.Object, _estimateVat.Object), Times.Once);
        }
    }
}
