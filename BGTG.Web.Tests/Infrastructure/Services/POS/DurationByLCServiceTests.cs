using BGTG.POS.DurationTools.DurationByLCTool;
using BGTG.POS.DurationTools.DurationByLCTool.Base;
using BGTG.POS.EstimateTool;
using BGTG.Web.Infrastructure.Auth;
using BGTG.Web.Infrastructure.Services.POS;
using BGTG.Web.Infrastructure.Services.POS.Base;
using BGTG.Web.ViewModels.POS.DurationByLCViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace BGTG.Web.Tests.Infrastructure.Services.POS;

public class DurationByLCServiceTests
{
    private DurationByLCService _durationByLCService = null!;
    private Mock<IEstimateService> _estimateServiceMock = null!;
    private Mock<IDurationByLCCreator> _durationByLCCreatorMock = null!;
    private Mock<IDurationByLCWriter> _durationByLCWriterMock = null!;
    private Mock<IWebHostEnvironment> _webHostEnvironmentMock = null!;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock = null!;

    [SetUp]
    public void SetUp()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        IdentityHelper.Instance.Configure(_httpContextAccessorMock.Object);
        _estimateServiceMock = new Mock<IEstimateService>();
        _durationByLCCreatorMock = new Mock<IDurationByLCCreator>();
        _durationByLCWriterMock = new Mock<IDurationByLCWriter>();
        _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        _durationByLCService = new DurationByLCService(_estimateServiceMock.Object, _durationByLCCreatorMock.Object,
            _durationByLCWriterMock.Object, _webHostEnvironmentMock.Object);

    }

    [Test]
    public void Write_DurationByLCCreateViewModel()
    {
        var estimateFiles = new FormFileCollection();
        var durationByLCCreateViewModel = new DurationByLCCreateViewModel()
        {
            ObjectCipher = "5.5-20.548",
            EstimateFiles = estimateFiles,
            AcceptanceTimeIncluded = true,
            NumberOfEmployees = 4,
            NumberOfWorkingDays = 21.5M,
            Shift = 1.5M,
            WorkingDayDuration = 8,
            TechnologicalLaborCosts = 110,
        };
        var estimate = new Estimate(default!, default!, default, default, default, default);
        _estimateServiceMock.Setup(x => x.Estimate).Returns(estimate);
        var expectedDurationByLC = new DurationByLC(default, default, default,
            durationByLCCreateViewModel.TechnologicalLaborCosts, default, default, default, default, default, default,
            default, default, true, true);

        var templatePath = @"wwwroot\AppData\Templates\DurationByLCTemplates\Rounding+Acceptance+.docx";
        var savePath = @"wwwroot\AppData\UserFiles\DurationByLCFiles\BGTGkss.docx";

        _durationByLCCreatorMock.Setup(x => x.Create(estimate.LaborCosts,
                durationByLCCreateViewModel.TechnologicalLaborCosts,
                durationByLCCreateViewModel.WorkingDayDuration, durationByLCCreateViewModel.Shift,
                durationByLCCreateViewModel.NumberOfWorkingDays,
                durationByLCCreateViewModel.NumberOfEmployees, durationByLCCreateViewModel.AcceptanceTimeIncluded))
            .Returns(expectedDurationByLC);

        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");
        IdentityFake.Setup(_httpContextAccessorMock, "BGTG\\kss");

        var actualDurationByLC = _durationByLCService.Write(durationByLCCreateViewModel);

        Assert.AreSame(expectedDurationByLC, actualDurationByLC);
        _estimateServiceMock.Verify(x => x.Read(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter), Times.Once);
        _durationByLCCreatorMock.Verify(x => x.Create(estimate.LaborCosts,
            durationByLCCreateViewModel.TechnologicalLaborCosts,
            durationByLCCreateViewModel.WorkingDayDuration, durationByLCCreateViewModel.Shift,
            durationByLCCreateViewModel.NumberOfWorkingDays,
            durationByLCCreateViewModel.NumberOfEmployees, durationByLCCreateViewModel.AcceptanceTimeIncluded), Times.Once);
        _durationByLCWriterMock.Verify(x => x.Write(expectedDurationByLC, templatePath, savePath), Times.Once);
        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Exactly(2));
        _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Once);
    }

    [Test]
    public void Write_DurationByLC()
    {
        var durationByLC = new DurationByLC(default, default, default,
            default, default, default, default, default, default, default,
            default, default, true, true);

        var templatePath = @"wwwroot\AppData\Templates\DurationByLCTemplates\Rounding+Acceptance+.docx";
        var savePath = @"wwwroot\AppData\UserFiles\DurationByLCFiles\BGTGkss.docx";

        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");
        IdentityFake.Setup(_httpContextAccessorMock, "BGTG\\kss");

        var result = _durationByLCService.Write(durationByLC);

        Assert.AreSame(durationByLC, result);
        _durationByLCWriterMock.Verify(x => x.Write(durationByLC, templatePath, savePath), Times.Once);
        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Exactly(2));
    }

    [Test]
    public void GetSavePath()
    {
        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");
        IdentityFake.Setup(_httpContextAccessorMock, "BGTG\\kss");

        var savePath = _durationByLCService.GetSavePath();

        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Once);
        Assert.AreEqual(@"wwwroot\AppData\UserFiles\DurationByLCFiles\BGTGkss.docx", savePath);
    }
}