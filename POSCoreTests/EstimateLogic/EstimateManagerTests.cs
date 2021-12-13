using Moq;
using NUnit.Framework;
using POSCore.EstimateLogic;
using POSCore.EstimateLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace POSCoreTests.EstimateLogic
{
    public class EstimateManagerTests
    {
        private Mock<IEstimateReader> _estimateReaderMock;
        private Mock<IEstimateConnector> _estimateConnectorMock;

        private EstimateManager CreateDefaultEstimateManager()
        {
            _estimateReaderMock = new Mock<IEstimateReader>();
            _estimateConnectorMock = new Mock<IEstimateConnector>();
            return new EstimateManager(_estimateReaderMock.Object, _estimateConnectorMock.Object);
        }

        [Test]
        public void GetEstimate_OneEstimateStream_OneEstimate()
        {
            var estimateManager = CreateDefaultEstimateManager();
            var stream = new Mock<Stream>();
            var streams = new List<Stream>() { stream.Object };
            var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, "", 0);
            var estimates = new List<Estimate> { estimate };
            _estimateReaderMock.Setup(x => x.Read(stream.Object)).Returns(estimate);
            _estimateConnectorMock.Setup(x => x.Connect(estimates)).Returns(estimate);

            var result = estimateManager.GetEstimate(streams);

            _estimateReaderMock.Verify(x => x.Read(stream.Object), Times.Once);
            _estimateConnectorMock.Verify(x => x.Connect(estimates), Times.Once);
            Assert.AreSame(estimate, result);
        }

        [Test]
        public void GetEstimate_TwoEstimateStreams_OneEstimate()
        {
            var estimateManager = CreateDefaultEstimateManager();
            var stream1 = new Mock<Stream>();
            var stream2 = new Mock<Stream>();
            var streams = new List<Stream>() { stream1.Object, stream2.Object };
            var estimate1 = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, "", 0);
            var estimate2 = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, "", 0);
            var estimates = new List<Estimate> { estimate1, estimate2 };
            var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, "", 0);
            _estimateReaderMock.Setup(x => x.Read(stream1.Object)).Returns(estimate1);
            _estimateReaderMock.Setup(x => x.Read(stream2.Object)).Returns(estimate2);
            _estimateConnectorMock.Setup(x => x.Connect(estimates)).Returns(estimate);

            var result = estimateManager.GetEstimate(streams);

            _estimateReaderMock.Verify(x => x.Read(stream1.Object), Times.Once);
            _estimateReaderMock.Verify(x => x.Read(stream2.Object), Times.Once);
            _estimateConnectorMock.Verify(x=>x.Connect(estimates), Times.Once);
            Assert.AreSame(estimate, result);
        }
    }
}
