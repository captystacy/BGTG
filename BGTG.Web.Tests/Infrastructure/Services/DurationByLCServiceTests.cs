using System;
using System.Collections.Generic;
using BGTG.POS.DurationTools.DurationByLCTool;
using BGTG.POS.DurationTools.DurationByLCTool.Interfaces;
using BGTG.POS.EstimateTool;
using BGTG.Web.Infrastructure.Services;
using BGTG.Web.Infrastructure.Services.Interfaces;
using BGTG.Web.ViewModels.POSViewModels.DurationByLCViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace BGTG.Web.Tests.Infrastructure.Services
{
    public class DurationByLCServiceTests
    {
        private DurationByLCService _durationByLCService;
        private Mock<IEstimateService> _estimateServiceMock;
        private Mock<IDurationByLCCreator> _durationByLCCreatorMock;
        private Mock<IDurationByLCWriter> _durationByLCWriterMock;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;

        [SetUp]
        public void SetUp()
        {
            _estimateServiceMock = new Mock<IEstimateService>();
            _durationByLCCreatorMock = new Mock<IDurationByLCCreator>();
            _durationByLCWriterMock = new Mock<IDurationByLCWriter>();
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            _durationByLCService = new DurationByLCService(_estimateServiceMock.Object, _durationByLCCreatorMock.Object,
                _durationByLCWriterMock.Object, _webHostEnvironmentMock.Object);

        }

        [Test]
        public void Write()
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
            var windowsName = "BGTG\\kss";
            var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, 0, 100);
            _estimateServiceMock.Setup(x => x.Estimate).Returns(estimate);
            var durationByLC = new DurationByLC(0, 0, 0, durationByLCCreateViewModel.TechnologicalLaborCosts,
                0, 0, 0, 0, 0, 0, 0, 0, true, true);


            _durationByLCCreatorMock.Setup(x => x.Create(estimate.LaborCosts,
                    durationByLCCreateViewModel.TechnologicalLaborCosts,
                    durationByLCCreateViewModel.WorkingDayDuration, durationByLCCreateViewModel.Shift,
                    durationByLCCreateViewModel.NumberOfWorkingDays,
                    durationByLCCreateViewModel.NumberOfEmployees, durationByLCCreateViewModel.AcceptanceTimeIncluded))
                .Returns(durationByLC);

            _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");

            var result = _durationByLCService.Write(durationByLCCreateViewModel, windowsName);

            Assert.AreEqual(durationByLC, result);
            _estimateServiceMock.Verify(x => x.Read(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter), Times.Once);
            _durationByLCCreatorMock.Verify(x => x.Create(estimate.LaborCosts,
                durationByLCCreateViewModel.TechnologicalLaborCosts,
                durationByLCCreateViewModel.WorkingDayDuration, durationByLCCreateViewModel.Shift,
                durationByLCCreateViewModel.NumberOfWorkingDays,
                durationByLCCreateViewModel.NumberOfEmployees, durationByLCCreateViewModel.AcceptanceTimeIncluded), Times.Once);
            _durationByLCWriterMock.Verify(x => x.Write(durationByLC,
                @"wwwroot\AppData\Templates\DurationByLCTemplates\Rounding+Acceptance+Template.docx",
                @"wwwroot\AppData\UserFiles\DurationByLCFiles\DurationByLCBGTGkss.docx"), Times.Once);
            _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Exactly(2));
            _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Once);
        }


        [Test]
        public void GetSavePath()
        {
            _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");
            var windowsName = "BGTG\\kss";

            var savePath = _durationByLCService.GetSavePath(windowsName);

            _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Once);
            Assert.AreEqual(@"wwwroot\AppData\UserFiles\DurationByLCFiles\DurationByLCBGTGkss.docx", savePath);
        }
    }
}
