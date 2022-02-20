using BGTG.POS.EstimateTool;
using BGTG.POS.EstimateTool.Base;
using BGTG.Web.Infrastructure.Services.POSServices;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace BGTG.Web.Tests.Infrastructure.Services.POS;

public class EstimateServiceTests
{
    private EstimateService _estimateService = null!;
    private Mock<IEstimateManager> _estimateManagerMock = null!;

    [SetUp]
    public void SetUp()
    {
        _estimateManagerMock = new Mock<IEstimateManager>();
        _estimateService = new EstimateService(_estimateManagerMock.Object);
    }

    [Test]
    public void Read()
    {
        var estimateFile1Mock = new Mock<IFormFile>();
        var estimateFile2Mock = new Mock<IFormFile>();
        var estimateFiles = new FormFileCollection { estimateFile1Mock.Object, estimateFile2Mock.Object, };
        var stream1Mock = new Mock<Stream>();
        var stream2Mock = new Mock<Stream>();
        var estimateStreams = new List<Stream> { stream1Mock.Object, stream2Mock.Object };
        estimateFile1Mock.Setup(x => x.OpenReadStream()).Returns(stream1Mock.Object);
        estimateFile2Mock.Setup(x => x.OpenReadStream()).Returns(stream2Mock.Object);
        var estimate = new Estimate(default!, default!, DateTime.Today, default, default, default);
        _estimateManagerMock.Setup(x => x.GetEstimate(estimateStreams, TotalWorkChapter.TotalWork1To12Chapter)).Returns(estimate);

        _estimateService.Read(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter);

        estimateFile1Mock.Verify(x => x.OpenReadStream(), Times.Once);
        estimateFile2Mock.Verify(x => x.OpenReadStream(), Times.Once);
        _estimateManagerMock.Verify(x => x.GetEstimate(estimateStreams, TotalWorkChapter.TotalWork1To12Chapter), Times.Once);
        Assert.AreSame(estimate, _estimateService.Estimate);
    }
}