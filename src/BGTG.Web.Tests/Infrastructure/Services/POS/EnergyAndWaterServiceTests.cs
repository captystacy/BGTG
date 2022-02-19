using System;
using System.Collections.Generic;
using BGTG.POS.CalendarPlanTool;
using BGTG.POS.CalendarPlanTool.Base;
using BGTG.POS.EnergyAndWaterTool;
using BGTG.POS.EnergyAndWaterTool.Base;
using BGTG.POS.EstimateTool;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.POSServices;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels.EnergyAndWaterViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace BGTG.Web.Tests.Infrastructure.Services.POS;

public class EnergyAndWaterServiceTests
{
    private EnergyAndWaterService _energyAndWaterService = null!;
    private Mock<IEstimateService> _estimateServiceMock = null!;
    private Mock<IEnergyAndWaterCreator> _energyAndWaterCreatorMock = null!;
    private Mock<IEnergyAndWaterWriter> _energyAndWaterWriterMock = null!;
    private Mock<IWebHostEnvironment> _webHostEnvironmentMock = null!;
    private Mock<ICalendarWorkCreator> _calendarWorkCreatorMock = null!;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock = null!;

    [SetUp]
    public void SetUp()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        IdentityHelper.Instance.Configure(_httpContextAccessorMock.Object);
        _estimateServiceMock = new Mock<IEstimateService>();
        _energyAndWaterCreatorMock = new Mock<IEnergyAndWaterCreator>();
        _energyAndWaterWriterMock = new Mock<IEnergyAndWaterWriter>();
        _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        _calendarWorkCreatorMock = new Mock<ICalendarWorkCreator>();
        _energyAndWaterService = new EnergyAndWaterService(_estimateServiceMock.Object, _energyAndWaterCreatorMock.Object, _energyAndWaterWriterMock.Object,
            _webHostEnvironmentMock.Object, _calendarWorkCreatorMock.Object);

    }

    [Test]
    public void Write_EnergyAndWaterCreateViewModel()
    {
        var estimateFiles = new FormFileCollection();
        var constructionStartDate = new DateTime(1999, 9, 21);
        var totalEstimateWork = new EstimateWork(default!, default, default, default, 12);
        var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork> { totalEstimateWork }, constructionStartDate, default, default, default);

        var templatePath = @"root\AppData\Templates\POSTemplates\EnergyAndWaterTemplates\EnergyAndWater.docx";
        var savePath = @"root\AppData\UserFiles\POSFiles\EnergyAndWaterFiles\BGTGkss.docx";

        var energyAndWaterCreateViewModel = new EnergyAndWaterCreateViewModel
        {
            EstimateFiles = estimateFiles,
        };
        var totalCalendarWork = new CalendarWork(default!, default, default, default!, default);
        _estimateServiceMock.Setup(x => x.Estimate).Returns(estimate);
        _calendarWorkCreatorMock.Setup(x => x.Create(totalEstimateWork, constructionStartDate)).Returns(totalCalendarWork);
        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");
        IdentityFake.Setup(_httpContextAccessorMock, "BGTG\\kss");
        var actualEnergyAndWater = new EnergyAndWater(default, default, default, default, default, default);
        _energyAndWaterCreatorMock.Setup(x => x.Create(totalCalendarWork.TotalCostIncludingCAIW, constructionStartDate.Year)).Returns(actualEnergyAndWater);

        var expectedEnergyAndWater = _energyAndWaterService.Write(energyAndWaterCreateViewModel);

        Assert.AreSame(actualEnergyAndWater, expectedEnergyAndWater);
        _webHostEnvironmentMock.Verify(x => x.ContentRootPath, Times.Exactly(2));
        _estimateServiceMock.Verify(x => x.Read(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter), Times.Once);
        _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Exactly(3));
        _calendarWorkCreatorMock.Verify(x => x.Create(totalEstimateWork, constructionStartDate), Times.Once);
        _energyAndWaterCreatorMock.Verify(x => x.Create(totalCalendarWork.TotalCostIncludingCAIW, constructionStartDate.Year), Times.Once);
        _energyAndWaterWriterMock.Verify(x => x.Write(actualEnergyAndWater, templatePath, savePath), Times.Once);
    }

    [Test]
    public void Write_EnergyAndWater()
    {
        var templatePath = @"root\AppData\Templates\POSTemplates\EnergyAndWaterTemplates\EnergyAndWater.docx";
        var savePath = @"root\AppData\UserFiles\POSFiles\EnergyAndWaterFiles\BGTGkss.docx";

        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");
        IdentityFake.Setup(_httpContextAccessorMock, "BGTG\\kss");
        var actualEnergyAndWater = new EnergyAndWater(default, default, default, default, default, default);

        var expectedEnergyAndWater = _energyAndWaterService.Write(actualEnergyAndWater);

        Assert.AreSame(actualEnergyAndWater, expectedEnergyAndWater);
        _webHostEnvironmentMock.Verify(x => x.ContentRootPath, Times.Exactly(2));
        _energyAndWaterWriterMock.Verify(x => x.Write(actualEnergyAndWater, templatePath, savePath), Times.Once);
    }

    [Test]
    public void GetSavePath()
    {
        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");
        IdentityFake.Setup(_httpContextAccessorMock, "BGTG\\kss");

        var savePath = _energyAndWaterService.GetSavePath();

        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Once);
        Assert.AreEqual(@"root\AppData\UserFiles\POSFiles\EnergyAndWaterFiles\BGTGkss.docx", savePath);
    }
}