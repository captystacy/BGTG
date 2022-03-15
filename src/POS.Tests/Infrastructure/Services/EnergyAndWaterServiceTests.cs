using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using POS.DomainModels;
using POS.DomainModels.CalendarPlanDomainModels;
using POS.DomainModels.EstimateDomainModels;
using POS.Infrastructure.Creators.Base;
using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;
using POS.ViewModels;

namespace POS.Tests.Infrastructure.Services;

public class EnergyAndWaterServiceTests
{
    private EnergyAndWaterService _energyAndWaterService = null!;
    private Mock<IEstimateService> _estimateServiceMock = null!;
    private Mock<IEnergyAndWaterCreator> _energyAndWaterCreatorMock = null!;
    private Mock<IEnergyAndWaterWriter> _energyAndWaterWriterMock = null!;
    private Mock<IWebHostEnvironment> _webHostEnvironmentMock = null!;
    private Mock<ICalendarWorkCreator> _calendarWorkCreatorMock = null!;

    [SetUp]
    public void SetUp()
    {
        _estimateServiceMock = new Mock<IEstimateService>();
        _energyAndWaterCreatorMock = new Mock<IEnergyAndWaterCreator>();
        _energyAndWaterWriterMock = new Mock<IEnergyAndWaterWriter>();
        _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        _calendarWorkCreatorMock = new Mock<ICalendarWorkCreator>();
        _energyAndWaterService = new EnergyAndWaterService(_estimateServiceMock.Object, _energyAndWaterCreatorMock.Object, _energyAndWaterWriterMock.Object,
            _webHostEnvironmentMock.Object, _calendarWorkCreatorMock.Object);

    }

    [Test]
    public void Write()
    {
        var estimateFiles = new FormFileCollection();
        var constructionStartDate = new DateTime(1999, 9, 21);
        var totalEstimateWork = new EstimateWork(default!, default, default, default, 12);
        var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork> { totalEstimateWork }, constructionStartDate, default, default, default);

        var templatePath = @"root\Templates\EnergyAndWaterTemplates\EnergyAndWater.docx";

        var energyAndWaterCreateViewModel = new EnergyAndWaterViewModel
        {
            EstimateFiles = estimateFiles,
        };
        var totalCalendarWork = new CalendarWork(default!, default, default, default!, default);
        _estimateServiceMock.Setup(x => x.Estimate).Returns(estimate);
        _calendarWorkCreatorMock.Setup(x => x.Create(totalEstimateWork, constructionStartDate)).Returns(totalCalendarWork);
        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");
        var actualEnergyAndWater = new EnergyAndWater(default, default, default, default, default, default);
        _energyAndWaterCreatorMock.Setup(x => x.Create(totalCalendarWork.TotalCostIncludingCAIW, constructionStartDate.Year)).Returns(actualEnergyAndWater);

        var expectedMemoryStream = new MemoryStream();
        _energyAndWaterWriterMock.Setup(x => x.Write(actualEnergyAndWater, templatePath)).Returns(expectedMemoryStream);
        var actualMemoryStream = _energyAndWaterService.Write(energyAndWaterCreateViewModel);

        Assert.AreSame(expectedMemoryStream, actualMemoryStream);
        _webHostEnvironmentMock.Verify(x => x.ContentRootPath, Times.Once);
        _estimateServiceMock.Verify(x => x.Read(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter), Times.Once);
        _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Exactly(3));
        _calendarWorkCreatorMock.Verify(x => x.Create(totalEstimateWork, constructionStartDate), Times.Once);
        _energyAndWaterCreatorMock.Verify(x => x.Create(totalCalendarWork.TotalCostIncludingCAIW, constructionStartDate.Year), Times.Once);
        _energyAndWaterWriterMock.Verify(x => x.Write(actualEnergyAndWater, templatePath), Times.Once);
    }
}