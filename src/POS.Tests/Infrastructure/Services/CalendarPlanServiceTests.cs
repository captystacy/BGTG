using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using POS.DomainModels.CalendarPlanDomainModels;
using POS.DomainModels.EstimateDomainModels;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Creators.Base;
using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;
using POS.ViewModels;

namespace POS.Tests.Infrastructure.Services;

public class CalendarPlanServiceTests
{
    private CalendarPlanService _calendarPlanService = null!;
    private Mock<IMapper> _mapperMock = null!;
    private Mock<IEstimateService> _estimateServiceMock = null!;
    private Mock<ICalendarPlanCreator> _calendarPlanCreatorMock = null!;
    private Mock<ICalendarPlanWriter> _calendarPlanWriterMock = null!;
    private Mock<IWebHostEnvironment> _webHostEnvironmentMock = null!;

    [SetUp]
    public void SetUp()
    {
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

        var calendarPlanCreateViewModel = new CalendarPlanCreateViewModel()
        {
            EstimateFiles = estimateFiles,
            TotalWorkChapter = TotalWorkChapter.TotalWork1To12Chapter
        };
        var estimate = new Estimate(default!, default!, default, default, default, default);
        var calendarPlanViewModel = new CalendarPlanViewModel
        {
            CalendarWorks = new List<CalendarWorkViewModel>
            {
                new()
                {
                    WorkName = AppConstants.TotalWorkName,
                    Chapter = (int)TotalWorkChapter.TotalWork1To12Chapter
                },
            }
        };
        _estimateServiceMock.SetupGet(x => x.Estimate).Returns(estimate);
        _mapperMock.Setup(x => x.Map<CalendarPlanViewModel>(estimate)).Returns(calendarPlanViewModel);

        var expectedCalendarPlanViewModel = new CalendarPlanViewModel
        {
            CalendarWorks = new List<CalendarWorkViewModel>
            {
                new()
                {
                    WorkName = AppConstants.MainOtherExpensesWorkName,
                    Chapter = AppConstants.MainOtherExpensesWorkChapter,
                    Percentages = new List<decimal>()
                }
            }
        };

        var actualCalendarPlanCreateViewModel =
            _calendarPlanService.GetCalendarPlanViewModel(calendarPlanCreateViewModel);

        _estimateServiceMock.Verify(x => x.Read(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter), Times.Once);
        _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Once);
        _mapperMock.Verify(x => x.Map<CalendarPlanViewModel>(estimate), Times.Once);
        Assert.AreEqual(expectedCalendarPlanViewModel, actualCalendarPlanCreateViewModel);
    }

    [Test]
    public void Write()
    {
        var otherExpensesPercentages = new List<decimal> { 0.2M, 0.3M, 0.5M };
        var calendarPlanViewModel = new CalendarPlanViewModel
        {
            EstimateFiles = new FormFileCollection(),
            TotalWorkChapter = TotalWorkChapter.TotalWork1To12Chapter,
            ConstructionStartDate = new DateTime(DateTime.Now.Ticks),
            ConstructionDuration = 0.7M,
            CalendarWorks = new List<CalendarWorkViewModel>
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
                    WorkName = AppConstants.MainOtherExpensesWorkName,
                    Percentages = otherExpensesPercentages
                },
            }
        };

        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");

        var preparatoryTemplatePath = @"root\Templates\CalendarPlanTemplates\Preparatory.docx";
        var mainTemplatePath = @"root\Templates\CalendarPlanTemplates\Main1.docx";

        var mainEstimateWorks = new List<EstimateWork>
        {
            new("Test work 1", default, default, default, default),
            new("Test work 2", default, default, default, default),
        };
        var estimate = new Estimate(default!, mainEstimateWorks, default, default, default, default);
        _estimateServiceMock.SetupGet(x => x.Estimate).Returns(estimate);

        var calendarPlan = new CalendarPlan(default!, default!, default, default, 1);
        _calendarPlanCreatorMock.Setup(x => x.Create(_estimateServiceMock.Object.Estimate, otherExpensesPercentages,
            calendarPlanViewModel.TotalWorkChapter)).Returns(calendarPlan);

        var expectedMemoryStream = new MemoryStream();
        _calendarPlanWriterMock.Setup(x => x.Write(calendarPlan, preparatoryTemplatePath, mainTemplatePath)).Returns(expectedMemoryStream);

        var actualMemoryStream = _calendarPlanService.Write(calendarPlanViewModel);

        Assert.AreSame(expectedMemoryStream, actualMemoryStream);
        _estimateServiceMock.Verify(x => x.Read(calendarPlanViewModel.EstimateFiles, calendarPlanViewModel.TotalWorkChapter), Times.Once);

        _calendarPlanCreatorMock.Verify(x =>
            x.Create(
                It.Is<Estimate>(x =>
                    x.MainEstimateWorks.First(x => x.WorkName == "Test work 1").Percentages
                        .SequenceEqual(calendarPlanViewModel.CalendarWorks
                            .First(x => x.WorkName == "Test work 1").Percentages)
                    && x.MainEstimateWorks.First(x => x.WorkName == "Test work 2").Percentages
                        .SequenceEqual(calendarPlanViewModel.CalendarWorks
                            .First(x => x.WorkName == "Test work 2").Percentages)
                    && x.ConstructionStartDate == calendarPlanViewModel.ConstructionStartDate
                    && x.ConstructionDuration == calendarPlanViewModel.ConstructionDuration),
                otherExpensesPercentages, calendarPlanViewModel.TotalWorkChapter), Times.Once);

        _calendarPlanWriterMock.Verify(x => x.Write(calendarPlan, preparatoryTemplatePath, mainTemplatePath));
        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Exactly(2));
    }
}