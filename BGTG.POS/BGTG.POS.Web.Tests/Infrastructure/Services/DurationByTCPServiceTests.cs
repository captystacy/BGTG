using BGTG.POS.Tools.DurationTools.DurationByTCPTool;
using BGTG.POS.Tools.DurationTools.DurationByTCPTool.Interfaces;
using BGTG.POS.Web.Infrastructure.Services;
using BGTG.POS.Web.ViewModels.DurationByTCPViewModels;
using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;

namespace BGTG.POS.Web.Tests.Infrastructure.Services
{
    public class DurationByTCPServiceTests
    {
        private DurationByTCPService _durationByTCPService;
        private Mock<IDurationByTCPCreator> _durationByTCPCreatorMock;
        private Mock<IDurationByTCPWriter> _durationByTCPWriterMock;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;

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
            var durationByTCP = new InterpolationDurationByTCP("", 0, "", 0, null,
                DurationCalculationType.Interpolation, 0, 0, 0, 0, 0, null);

            var durationByTCPViewModel = new DurationByTCPViewModel() { UserFullName = "BGTG\\kss" };

            _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");

            _durationByTCPCreatorMock.Setup(x => x.Create(durationByTCPViewModel.PipelineMaterial,
                durationByTCPViewModel.PipelineDiameter, durationByTCPViewModel.PipelineLength, durationByTCPViewModel.AppendixKey,
                durationByTCPViewModel.PipelineCategoryName)).Returns(durationByTCP);

            var isWritten = _durationByTCPService.Write(durationByTCPViewModel);

            Assert.True(isWritten);
            _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Exactly(2));
            _durationByTCPCreatorMock.Verify(x => x.Create(durationByTCPViewModel.PipelineMaterial,
                durationByTCPViewModel.PipelineDiameter, durationByTCPViewModel.PipelineLength, durationByTCPViewModel.AppendixKey,
                durationByTCPViewModel.PipelineCategoryName), Times.Once);
            _durationByTCPWriterMock.Verify(x => x.Write(durationByTCP,
                @"wwwroot\AppData\Templates\DurationByTCPTemplates\InterpolationTemplate.docx",
                @"wwwroot\AppData\UserFiles\DurationByTCPFiles\DurationByTCPBGTGkss.docx"), Times.Once);
        }

        [Test]
        public void GetSavePath()
        {
            _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");
            var userFullName = "BGTG\\kss";

            var savePath = _durationByTCPService.GetSavePath(userFullName);

            _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Once);
            Assert.AreEqual(@"wwwroot\AppData\UserFiles\DurationByTCPFiles\DurationByTCPBGTGkss.docx", savePath);
        }

        [Test]
        public void GetFileName()
        {
            var fileName = _durationByTCPService.GetFileName();

            Assert.AreEqual(@"ПРОД.docx", fileName);
        }
    }
}
