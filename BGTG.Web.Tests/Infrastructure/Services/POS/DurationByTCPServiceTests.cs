using BGTG.POS.DurationTools.DurationByTCPTool;
using BGTG.POS.DurationTools.DurationByTCPTool.Base;
using BGTG.Web.Infrastructure.Auth;
using BGTG.Web.Infrastructure.Services.POS;
using BGTG.Web.ViewModels.POS.DurationByTCPViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace BGTG.Web.Tests.Infrastructure.Services.POS;

public class DurationByTCPServiceTests
{
    private DurationByTCPService _durationByTCPService = null!;
    private Mock<IDurationByTCPCreator> _durationByTCPCreatorMock = null!;
    private Mock<IDurationByTCPWriter> _durationByTCPWriterMock = null!;
    private Mock<IWebHostEnvironment> _webHostEnvironmentMock = null!;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock = null!;

    [SetUp]
    public void SetUp()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        IdentityHelper.Instance.Configure(_httpContextAccessorMock.Object);
        _durationByTCPCreatorMock = new Mock<IDurationByTCPCreator>();
        _durationByTCPWriterMock = new Mock<IDurationByTCPWriter>();
        _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        _durationByTCPService = new DurationByTCPService(_durationByTCPCreatorMock.Object, _durationByTCPWriterMock.Object, _webHostEnvironmentMock.Object);
    }

    [Test]
    public void Write_DurationByTCPCreateViewModel()
    {
        var expectedDurationByTCP = new InterpolationDurationByTCP(default!, default, default!, default, default!,
            DurationCalculationType.Interpolation, default, default, default, default, default, default, default);

        var durationByTCPCreateViewModel = new DurationByTCPCreateViewModel();

        var templatePath = @"wwwroot\AppData\Templates\DurationByTCPTemplates\Interpolation.docx";
        var savePath = @"wwwroot\AppData\UserFiles\DurationByTCPFiles\BGTGkss.docx";

        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");
        IdentityFake.Setup(_httpContextAccessorMock, "BGTG\\kss");

        _durationByTCPCreatorMock.Setup(x => x.Create(durationByTCPCreateViewModel.PipelineMaterial,
            durationByTCPCreateViewModel.PipelineDiameter, durationByTCPCreateViewModel.PipelineLength, durationByTCPCreateViewModel.AppendixKey,
            durationByTCPCreateViewModel.PipelineCategoryName)).Returns(expectedDurationByTCP);

        var actualDurationByTCP = _durationByTCPService.Write(durationByTCPCreateViewModel);

        Assert.AreSame(expectedDurationByTCP, actualDurationByTCP);
        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Exactly(2));
        _durationByTCPCreatorMock.Verify(x => x.Create(durationByTCPCreateViewModel.PipelineMaterial,
            durationByTCPCreateViewModel.PipelineDiameter, durationByTCPCreateViewModel.PipelineLength, durationByTCPCreateViewModel.AppendixKey,
            durationByTCPCreateViewModel.PipelineCategoryName), Times.Once);
        _durationByTCPWriterMock.Verify(x => x.Write(expectedDurationByTCP, templatePath, savePath), Times.Once);
    }

    [Test]
    public void Write_DurationByTCPCreateViewModel_CreatorReturnNull()
    {
        var expectedDurationByTCP = new InterpolationDurationByTCP(default!, default, default!, default, default!,
            DurationCalculationType.Interpolation, default, default, default, default, default, default, default);

        var durationByTCPCreateViewModel = new DurationByTCPCreateViewModel();

        var templatePath = @"wwwroot\AppData\Templates\DurationByTCPTemplates\Interpolation.docx";
        var savePath = @"wwwroot\AppData\UserFiles\DurationByTCPFiles\BGTGkss.docx";

        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");
        IdentityFake.Setup(_httpContextAccessorMock, "BGTG\\kss");

        var actualDurationByTCP = _durationByTCPService.Write(durationByTCPCreateViewModel);

        Assert.Null(actualDurationByTCP);
        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Never);
        _durationByTCPCreatorMock.Verify(x => x.Create(durationByTCPCreateViewModel.PipelineMaterial,
            durationByTCPCreateViewModel.PipelineDiameter, durationByTCPCreateViewModel.PipelineLength, durationByTCPCreateViewModel.AppendixKey,
            durationByTCPCreateViewModel.PipelineCategoryName), Times.Once);
        _durationByTCPWriterMock.Verify(x => x.Write(expectedDurationByTCP, templatePath, savePath), Times.Never);
    }

    [Test]
    public void Write_DurationByTCP()
    {
        var durationByTCP = new InterpolationDurationByTCP(default!, default, default!, default, default!,
            DurationCalculationType.Interpolation, default, default, default, default, default, default, default);

        var templatePath = @"wwwroot\AppData\Templates\DurationByTCPTemplates\Interpolation.docx";
        var savePath = @"wwwroot\AppData\UserFiles\DurationByTCPFiles\BGTGkss.docx";

        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");
        IdentityFake.Setup(_httpContextAccessorMock, "BGTG\\kss");

        var result = _durationByTCPService.Write(durationByTCP);

        Assert.AreSame(durationByTCP, result);
        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Exactly(2));
        _durationByTCPWriterMock.Verify(x => x.Write(durationByTCP, templatePath, savePath), Times.Once);
    }

    [Test]
    public void GetSavePath()
    {
        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");
        IdentityFake.Setup(_httpContextAccessorMock, "BGTG\\kss");

        var savePath = _durationByTCPService.GetSavePath();

        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Once);
        Assert.AreEqual(@"wwwroot\AppData\UserFiles\DurationByTCPFiles\BGTGkss.docx", savePath);
    }
}