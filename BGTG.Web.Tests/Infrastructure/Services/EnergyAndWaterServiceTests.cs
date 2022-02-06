using System;
using System.Collections.Generic;
using BGTG.POS.CalendarPlanTool;
using BGTG.POS.CalendarPlanTool.Interfaces;
using BGTG.POS.EnergyAndWaterTool;
using BGTG.POS.EnergyAndWaterTool.Interfaces;
using BGTG.POS.EstimateTool;
using BGTG.Web.Infrastructure.Services;
using BGTG.Web.Infrastructure.Services.Interfaces;
using BGTG.Web.ViewModels.POSViewModels.EnergyAndWaterViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace BGTG.Web.Tests.Infrastructure.Services
{
    public class EnergyAndWaterServiceTests
    {
        private EnergyAndWaterService _energyAndWaterService;
        private Mock<IEstimateService> _estimateServiceMock;
        private Mock<IEnergyAndWaterCreator> _energyAndWaterCreatorMock;
        private Mock<IEnergyAndWaterWriter> _energyAndWaterWriterMock;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;
        private Mock<ICalendarWorkCreator> _calendarWorkCreatorMock;

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
            var totalEstimateWork = new EstimateWork("", 0, 0, 0, 12);
            var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork>() { totalEstimateWork }, constructionStartDate, 0, 0, 0);
            var windowsName = "BGTG\\kss";
            var energyAndWaterCreateViewModel = new EnergyAndWaterCreateViewModel()
            {
                EstimateFiles = estimateFiles,
            };
            var totalCalendarWork = new CalendarWork("tarstr", 1.213M, 2.234M, null, 10);
            _estimateServiceMock.Setup(x => x.Estimate).Returns(estimate);
            _calendarWorkCreatorMock.Setup(x => x.Create(totalEstimateWork, constructionStartDate)).Returns(totalCalendarWork);
            _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");
            var energyAndWater = new EnergyAndWater(0, 0, 0, 0, 0, 0);
            _energyAndWaterCreatorMock.Setup(x => x.Create(totalCalendarWork.TotalCostIncludingCAIW, constructionStartDate.Year)).Returns(energyAndWater);

            var result = _energyAndWaterService.Write(energyAndWaterCreateViewModel, windowsName);

            Assert.AreEqual(energyAndWater, result);
            _webHostEnvironmentMock.Verify(x => x.ContentRootPath, Times.Exactly(2));
            _estimateServiceMock.Verify(x => x.Read(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter), Times.Once);
            _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Exactly(3));
            _calendarWorkCreatorMock.Verify(x => x.Create(totalEstimateWork, constructionStartDate), Times.Once);
            _energyAndWaterCreatorMock.Verify(x => x.Create(totalCalendarWork.TotalCostIncludingCAIW, constructionStartDate.Year), Times.Once);
            _energyAndWaterWriterMock.Verify(
                x => x.Write(energyAndWater,
                    @"wwwroot\AppData\Templates\EnergyAndWaterTemplates\EnergyAndWaterTemplate.docx",
                    @"wwwroot\AppData\UserFiles\EnergyAndWaterFiles\EnergyAndWaterBGTGkss.docx"), Times.Once);
        }

        [Test]
        public void GetSavePath()
        {
            _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");
            var windowsName = "BGTG\\kss";

            var savePath = _energyAndWaterService.GetSavePath(windowsName);

            _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Once);
            Assert.AreEqual(@"wwwroot\AppData\UserFiles\EnergyAndWaterFiles\EnergyAndWaterBGTGkss.docx", savePath);
        }
    }
}
