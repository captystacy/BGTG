using System.IO;
using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;
using POS.Infrastructure.Services;
using POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.Base;
using POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.Models;
using POS.ViewModels;

namespace POS.Tests.Infrastructure.Services;

public class DurationByTCPServiceTests
{
    private DurationByTCPService _durationByTCPService = null!;
    private Mock<IDurationByTCPCreator> _durationByTCPCreatorMock = null!;
    private Mock<IDurationByTCPWriter> _durationByTCPWriterMock = null!;
    private Mock<IWebHostEnvironment> _webHostEnvironmentMock = null!;

    [SetUp]
    public void SetUp()
    {
        _durationByTCPCreatorMock = new Mock<IDurationByTCPCreator>();
        _durationByTCPWriterMock = new Mock<IDurationByTCPWriter>();
        _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        _durationByTCPService = new DurationByTCPService(_durationByTCPCreatorMock.Object, _durationByTCPWriterMock.Object, _webHostEnvironmentMock.Object);
    }

    [Test]
    public void Write()
    {
        var expectedDurationByTCP = new InterpolationDurationByTCP(default!, default, default!, default, default!,
            DurationCalculationType.Interpolation, default, default, default, default, default, default, default);

        var durationByTCPCreateViewModel = new DurationByTCPViewModel();

        var templatePath = @"root\Templates\DurationByTCPTemplates\Interpolation.docx";

        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");

        _durationByTCPCreatorMock.Setup(x => x.Create(durationByTCPCreateViewModel.PipelineMaterial,
            durationByTCPCreateViewModel.PipelineDiameter, durationByTCPCreateViewModel.PipelineLength, durationByTCPCreateViewModel.AppendixKey,
            durationByTCPCreateViewModel.PipelineCategoryName)).Returns(expectedDurationByTCP);

        var expectedMemoryStream = new MemoryStream();
        _durationByTCPWriterMock.Setup(x => x.Write(expectedDurationByTCP, templatePath)).Returns(expectedMemoryStream);
        var actualMemoryStream = _durationByTCPService.Write(durationByTCPCreateViewModel);
        
        Assert.AreSame(expectedMemoryStream, actualMemoryStream);
        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Once);
        _durationByTCPCreatorMock.Verify(x => x.Create(durationByTCPCreateViewModel.PipelineMaterial,
            durationByTCPCreateViewModel.PipelineDiameter, durationByTCPCreateViewModel.PipelineLength, durationByTCPCreateViewModel.AppendixKey,
            durationByTCPCreateViewModel.PipelineCategoryName), Times.Once);
        _durationByTCPWriterMock.Verify(x => x.Write(expectedDurationByTCP, templatePath), Times.Once);
    }

    [Test]
    public void Write_CreatorReturnNull()
    {
        var expectedDurationByTCP = new InterpolationDurationByTCP(default!, default, default!, default, default!,
            DurationCalculationType.Interpolation, default, default, default, default, default, default, default);

        var durationByTCPCreateViewModel = new DurationByTCPViewModel();

        var templatePath = @"root\AppData\Templates\POSTemplates\DurationByTCPTemplates\Interpolation.docx";

        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");

        var memoryStream = _durationByTCPService.Write(durationByTCPCreateViewModel);

        Assert.Null(memoryStream);
        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Never);
        _durationByTCPCreatorMock.Verify(x => x.Create(durationByTCPCreateViewModel.PipelineMaterial,
            durationByTCPCreateViewModel.PipelineDiameter, durationByTCPCreateViewModel.PipelineLength, durationByTCPCreateViewModel.AppendixKey,
            durationByTCPCreateViewModel.PipelineCategoryName), Times.Once);
        _durationByTCPWriterMock.Verify(x => x.Write(expectedDurationByTCP, templatePath), Times.Never);
    }
}