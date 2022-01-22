using System;
using System.Collections.Generic;
using BGTG.POS.Tools.DurationTools.DurationByLCTool;
using BGTG.POS.Tools.DurationTools.DurationByLCTool.Interfaces;
using BGTG.POS.Tools.EstimateTool;
using BGTG.POS.Web.Infrastructure.Services;
using BGTG.POS.Web.Infrastructure.Services.Interfaces;
using BGTG.POS.Web.ViewModels.DurationByLCViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace BGTG.POS.Web.Tests.Infrastructure.Services
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
            var durationByLCViewModel = new DurationByLCViewModel()
            {
                AcceptanceTimeIncluded = true,
                NumberOfEmployees = 4,
                NumberOfWorkingDays = 21.5M,
                Shift = 1.5M,
                WorkingDayDuration = 8,
                TechnologicalLaborCosts = 110,
            };
            var userFullName = "BGTG\\kss";
            var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, 0, "", 100);
            _estimateServiceMock.Setup(x => x.Estimate).Returns(estimate);
            var durationByLC = new DurationByLC(0, 0, 0, durationByLCViewModel.TechnologicalLaborCosts,
                0, 0, 0, 0, 0, 0, 0, 0, true, true);


            _durationByLCCreatorMock.Setup(x => x.Create(estimate.LaborCosts,
                    durationByLCViewModel.TechnologicalLaborCosts,
                    durationByLCViewModel.WorkingDayDuration, durationByLCViewModel.Shift,
                    durationByLCViewModel.NumberOfWorkingDays,
                    durationByLCViewModel.NumberOfEmployees, durationByLCViewModel.AcceptanceTimeIncluded))
                .Returns(durationByLC);

            _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");

            _durationByLCService.Write(estimateFiles, durationByLCViewModel, userFullName);

            _estimateServiceMock.Verify(x => x.Read(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter), Times.Once);
            _durationByLCCreatorMock.Verify(x => x.Create(estimate.LaborCosts,
                durationByLCViewModel.TechnologicalLaborCosts,
                durationByLCViewModel.WorkingDayDuration, durationByLCViewModel.Shift,
                durationByLCViewModel.NumberOfWorkingDays,
                durationByLCViewModel.NumberOfEmployees, durationByLCViewModel.AcceptanceTimeIncluded), Times.Once);
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
            var userFullName = "BGTG\\kss";

            var savePath = _durationByLCService.GetSavePath(userFullName);

            _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Once);
            Assert.AreEqual(@"wwwroot\AppData\UserFiles\DurationByLCFiles\DurationByLCBGTGkss.docx", savePath);
        }

        [Test]
        public void GetFileName()
        {
            var objectCipher = "5.5-20.548";
            var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, 0, objectCipher, 0);
            _estimateServiceMock.Setup(x => x.Estimate).Returns(estimate);

            var fileName = _durationByLCService.GetFileName();

            _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Once);
            Assert.AreEqual($"{objectCipher}ПРОД.docx", fileName);
        }
    }
}
