using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using POSCore.EstimateLogic;
using POSCore.EstimateLogic.Interfaces;
using POSWeb.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace POSWebTests.Services
{
    public class EstimateServiceTests
    {
        private Mock<IEstimateManager> _estimateManagerMock;

        private EstimateService CreateDefaultEstimateService()
        {
            _estimateManagerMock = new Mock<IEstimateManager>();
            return new EstimateService(_estimateManagerMock.Object);
        }

        [Test]
        public void GetEstimate()
        {
            var estimateService = CreateDefaultEstimateService();
            var estimateFile1Mock = new Mock<IFormFile>();
            var estimateFile2Mock = new Mock<IFormFile>();
            var estimateFiles = new List<IFormFile> { estimateFile1Mock.Object, estimateFile2Mock.Object, };
            var stream1Mock = new Mock<Stream>();
            var stream2Mock = new Mock<Stream>();
            var estimateStreams = new List<Stream> { stream1Mock.Object, stream2Mock.Object };
            estimateFile1Mock.Setup(x => x.OpenReadStream()).Returns(stream1Mock.Object);
            estimateFile2Mock.Setup(x => x.OpenReadStream()).Returns(stream2Mock.Object);
            var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, "", 0);
            _estimateManagerMock.Setup(x => x.GetEstimate(estimateStreams)).Returns(estimate);

            estimateService.ReadEstimateFiles(estimateFiles);

            estimateFile1Mock.Verify(x => x.OpenReadStream(), Times.Once);
            estimateFile2Mock.Verify(x => x.OpenReadStream(), Times.Once);
            _estimateManagerMock.Verify(x => x.GetEstimate(estimateStreams), Times.Once);
            Assert.AreSame(estimate, estimateService.Estimate);
        }
    }
}
