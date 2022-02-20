using AutoMapper;
using BGTG.POS.CalendarPlanTool;
using BGTG.POS.CalendarPlanTool.Base;
using BGTG.POS.EstimateTool;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.POSServices;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels.CalendarPlanViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace BGTG.Web.Tests.Infrastructure.Services.POS;

public class CalendarPlanServiceTests
{
    private CalendarPlanService _calendarPlanService = null!;
    private Mock<IMapper> _mapperMock = null!;
    private Mock<IEstimateService> _estimateServiceMock = null!;
    private Mock<ICalendarPlanCreator> _calendarPlanCreatorMock = null!;
    private Mock<ICalendarPlanWriter> _calendarPlanWriterMock = null!;
    private Mock<IWebHostEnvironment> _webHostEnvironmentMock = null!;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock = null!;

    [SetUp]
    public void SetUp()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        IdentityHelper.Instance.Configure(_httpContextAccessorMock.Object);
        _mapperMock = new Mock<IMapper>();
        _estimateServiceMock = new Mock<IEstimateService>();
        _calendarPlanCreatorMock = new Mock<ICalendarPlanCreator>();
        _calendarPlanWriterMock = new Mock<ICalendarPlanWriter>();
        _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        _calendarPlanService = new CalendarPlanService(_estimateServiceMock.Object, _calendarPlanCreatorMock.Object,
            _calendarPlanWriterMock.Object,
            _webHostEnvironmentMock.Object, _mapperMock.Object);

    }

    [Test]
    public void GetCalendarPlanCreateViewModel()
    {
        var estimateFiles = new FormFileCollection();

        var calendarPlanPreCreateViewModel = new CalendarPlanPreCreateViewModel
        {
            EstimateFiles = estimateFiles,
            TotalWorkChapter = TotalWorkChapter.TotalWork1To12Chapter
        };
        var estimate = new Estimate(default!, default!, default, default, default, default);
        var calendarPlanCreateViewModel = new CalendarPlanCreateViewModel
        {
            CalendarWorkViewModels = new List<CalendarWorkViewModel>
            {
                new()
                {
                    WorkName = CalendarPlanInfo.TotalWorkName,
                    Chapter = (int)TotalWorkChapter.TotalWork1To12Chapter
                },
            }
        };
        _estimateServiceMock.SetupGet(x => x.Estimate).Returns(estimate);
        _mapperMock.Setup(x => x.Map<CalendarPlanCreateViewModel>(estimate)).Returns(calendarPlanCreateViewModel);

        var expectedCalendarPlanCreateViewModel = new CalendarPlanCreateViewModel
        {
            CalendarWorkViewModels = new List<CalendarWorkViewModel>
            {
                new()
                {
                    WorkName = CalendarPlanInfo.MainOtherExpensesWorkName,
                    Chapter = CalendarPlanInfo.MainOtherExpensesWorkChapter,
                    Percentages = new List<decimal>()
                }
            }
        };

        var actualCalendarPlanCreateViewModel =
            _calendarPlanService.GetCalendarPlanCreateViewModel(calendarPlanPreCreateViewModel);

        _estimateServiceMock.Verify(x => x.Read(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter), Times.Once);
        _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Once);
        _mapperMock.Verify(x => x.Map<CalendarPlanCreateViewModel>(estimate), Times.Once);
        Assert.AreEqual(expectedCalendarPlanCreateViewModel, actualCalendarPlanCreateViewModel);
    }

    [Test]
    public void Write_CalendarPlanCreateViewModel()
    {
        var otherExpensesPercentages = new List<decimal> { 0.2M, 0.3M, 0.5M };
        var calendarPlanCreateViewModel = new CalendarPlanCreateViewModel
        {
            EstimateFiles = new FormFileCollection(),
            TotalWorkChapter = TotalWorkChapter.TotalWork1To12Chapter,
            ConstructionStartDate = new DateTime(DateTime.Now.Ticks),
            ConstructionDurationCeiling = 1,
            ConstructionDuration = 0.7M,
            CalendarWorkViewModels = new List<CalendarWorkViewModel>
            {
                new()
                {
                    WorkName = "Test work 1",
                    Percentages = new List<decimal> { 0.2M, 0.3M}
                },
                new()
                {
                    WorkName = "Test work 2",
                    Percentages = new List<decimal> { 0.3M, 0.4M}
                },
                new()
                {
                    WorkName = CalendarPlanInfo.MainOtherExpensesWorkName,
                    Percentages = otherExpensesPercentages
                },
            }
        };

        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");

        IdentityFake.Setup(_httpContextAccessorMock, "BGTG\\kss");
        var preparatoryTemplatePath = @"root\AppData\Templates\POSTemplates\CalendarPlanTemplates\Preparatory.docx";
        var mainTemplatePath = @"root\AppData\Templates\POSTemplates\CalendarPlanTemplates\Main1.docx";
        var savePath = @"root\AppData\UserFiles\POSFiles\CalendarPlanFiles\BGTGkss.docx";

        var mainEstimateWorks = new List<EstimateWork>
        {
            new("Test work 1", default, default, default, default),
            new("Test work 2", default, default, default, default),
        };
        var estimate = new Estimate(default!, mainEstimateWorks, default, default, default, default);
        _estimateServiceMock.SetupGet(x => x.Estimate).Returns(estimate);

        var expectedCalendarPlan = new CalendarPlan(default!, default!, default, default, 1);
        _calendarPlanCreatorMock.Setup(x => x.Create(_estimateServiceMock.Object.Estimate, otherExpensesPercentages,
            calendarPlanCreateViewModel.TotalWorkChapter)).Returns(expectedCalendarPlan);

        var actualCalendarPlan = _calendarPlanService.Write(calendarPlanCreateViewModel);

        _estimateServiceMock.Verify(x => x.Read(calendarPlanCreateViewModel.EstimateFiles, calendarPlanCreateViewModel.TotalWorkChapter), Times.Once);

        _calendarPlanCreatorMock.Verify(x =>
            x.Create(
                It.Is<Estimate>(x =>
                    x.MainEstimateWorks.First(x => x.WorkName == "Test work 1").Percentages
                        .SequenceEqual(calendarPlanCreateViewModel.CalendarWorkViewModels
                            .First(x => x.WorkName == "Test work 1").Percentages)
                    && x.MainEstimateWorks.First(x => x.WorkName == "Test work 2").Percentages
                        .SequenceEqual(calendarPlanCreateViewModel.CalendarWorkViewModels
                            .First(x => x.WorkName == "Test work 2").Percentages)
                    && x.ConstructionStartDate == calendarPlanCreateViewModel.ConstructionStartDate
                    && x.ConstructionDurationCeiling == calendarPlanCreateViewModel.ConstructionDurationCeiling
                    && x.ConstructionDuration == calendarPlanCreateViewModel.ConstructionDuration),
                otherExpensesPercentages, calendarPlanCreateViewModel.TotalWorkChapter), Times.Once);

        _calendarPlanWriterMock.Verify(x => x.Write(expectedCalendarPlan, preparatoryTemplatePath, mainTemplatePath, savePath));
        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Exactly(3));

        Assert.AreSame(expectedCalendarPlan, actualCalendarPlan);
    }

    [Test]
    public void Write_CalendarPlan()
    {
        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");

        IdentityFake.Setup(_httpContextAccessorMock, "BGTG\\kss");
        var preparatoryTemplatePath = @"root\AppData\Templates\POSTemplates\CalendarPlanTemplates\Preparatory.docx";
        var mainTemplatePath = @"root\AppData\Templates\POSTemplates\CalendarPlanTemplates\Main1.docx";
        var savePath = @"root\AppData\UserFiles\POSFiles\CalendarPlanFiles\BGTGkss.docx";

        var mainEstimateWorks = new List<EstimateWork>
        {
            new("Test work 1", default, default, default, default),
            new("Test work 2", default, default, default, default),
        };
        var estimate = new Estimate(default!, mainEstimateWorks, default, default, default, default);
        _estimateServiceMock.SetupGet(x => x.Estimate).Returns(estimate);

        var expectedCalendarPlan = new CalendarPlan(default!, default!, default, default, 1);

        var actualCalendarPlan = _calendarPlanService.Write(expectedCalendarPlan);

        _calendarPlanWriterMock.Verify(x => x.Write(expectedCalendarPlan, preparatoryTemplatePath, mainTemplatePath, savePath));
        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Exactly(3));

        Assert.AreSame(expectedCalendarPlan, actualCalendarPlan);
    }

    [Test]
    public void GetSavePath()
    {
        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");
        IdentityFake.Setup(_httpContextAccessorMock, "BGTG\\kss");

        var savePath = _calendarPlanService.GetSavePath();

        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Once);
        Assert.AreEqual(@"root\AppData\UserFiles\POSFiles\CalendarPlanFiles\BGTGkss.docx", savePath);
    }
}