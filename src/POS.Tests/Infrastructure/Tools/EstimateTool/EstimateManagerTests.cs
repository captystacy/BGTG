using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using NUnit.Framework;
using POS.Infrastructure.Tools.EstimateTool;
using POS.Infrastructure.Tools.EstimateTool.Base;
using POS.Infrastructure.Tools.EstimateTool.Models;

namespace POS.Tests.Infrastructure.Tools.EstimateTool;

public class EstimateManagerTests
{
    private EstimateManager _estimateManager = null!;
    private Mock<IEstimateReader> _estimateReaderMock = null!;
    private Mock<IEstimateConnector> _estimateConnectorMock = null!;

    [SetUp]
    public void SetUp()
    {
        _estimateReaderMock = new Mock<IEstimateReader>();
        _estimateConnectorMock = new Mock<IEstimateConnector>();
        _estimateManager = new EstimateManager(_estimateReaderMock.Object, _estimateConnectorMock.Object);
    }

    [Test]
    public void GetEstimate_OneEstimateStream_OneEstimate()
    {
        var stream = new Mock<Stream>();
        var streams = new List<Stream> { stream.Object };
        var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, 0, 0);
        var estimates = new List<Estimate> { estimate };
        _estimateReaderMock.Setup(x => x.Read(stream.Object, TotalWorkChapter.TotalWork1To12Chapter)).Returns(estimate);
        _estimateConnectorMock.Setup(x => x.Connect(estimates)).Returns(estimate);

        var result = _estimateManager.GetEstimate(streams, TotalWorkChapter.TotalWork1To12Chapter);

        _estimateReaderMock.Verify(x => x.Read(stream.Object, TotalWorkChapter.TotalWork1To12Chapter), Times.Once);
        _estimateConnectorMock.Verify(x => x.Connect(estimates), Times.Once);
        Assert.AreSame(estimate, result);
    }

    [Test]
    public void GetEstimate_TwoEstimateStreams_OneEstimate()
    {
        var stream1 = new Mock<Stream>();
        var stream2 = new Mock<Stream>();
        var streams = new List<Stream>() { stream1.Object, stream2.Object };
        var estimate1 = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, 0, 0);
        var estimate2 = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, 0, 0);
        var estimates = new List<Estimate> { estimate1, estimate2 };
        var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, 0, 0);
        _estimateReaderMock.Setup(x => x.Read(stream1.Object, TotalWorkChapter.TotalWork1To12Chapter)).Returns(estimate1);
        _estimateReaderMock.Setup(x => x.Read(stream2.Object, TotalWorkChapter.TotalWork1To12Chapter)).Returns(estimate2);
        _estimateConnectorMock.Setup(x => x.Connect(estimates)).Returns(estimate);

        var result = _estimateManager.GetEstimate(streams, TotalWorkChapter.TotalWork1To12Chapter);

        _estimateReaderMock.Verify(x => x.Read(stream1.Object, TotalWorkChapter.TotalWork1To12Chapter), Times.Once);
        _estimateReaderMock.Verify(x => x.Read(stream2.Object,TotalWorkChapter.TotalWork1To12Chapter), Times.Once);
        _estimateConnectorMock.Verify(x=>x.Connect(estimates), Times.Once);
        Assert.AreSame(estimate, result);
    }
}