using BGTGWeb.Services;
using BGTGWeb.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;
using POS.DurationLogic.DurationByTCP;
using POS.DurationLogic.DurationByTCP.Interfaces;

namespace BGTGWebTests.Services
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

            var userFullName = "BGTG\\kss";
            var durationByTCPViewModel = new DurationByTCPViewModel();

            _webHostEnvironmentMock.Setup(x => x.WebRootPath).Returns("www");

            _durationByTCPCreatorMock.Setup(x => x.Create(durationByTCPViewModel.PipelineMaterial,
                durationByTCPViewModel.PipelineDiameter, durationByTCPViewModel.PipelineLength, durationByTCPViewModel.AppendixKey,
                durationByTCPViewModel.PipelineCategoryName)).Returns(durationByTCP);

            var isWritten = _durationByTCPService.Write(durationByTCPViewModel, userFullName);

            Assert.True(isWritten);
            _webHostEnvironmentMock.VerifyGet(x => x.WebRootPath, Times.Exactly(2));
            _durationByTCPCreatorMock.Verify(x => x.Create(durationByTCPViewModel.PipelineMaterial,
                durationByTCPViewModel.PipelineDiameter, durationByTCPViewModel.PipelineLength, durationByTCPViewModel.AppendixKey,
                durationByTCPViewModel.PipelineCategoryName), Times.Once);
            _durationByTCPWriterMock.Verify(x => x.Write(durationByTCP,
                @"www\Templates\DurationByTCPTemplates\InterpolationTemplate.docx",
                @"www\UsersFiles\DurationByTCP\DurationByTCPBGTGkss.docx"), Times.Once);
        }

        [Test]
        public void GetSavePath()
        {
            _webHostEnvironmentMock.Setup(x => x.WebRootPath).Returns("www");
            var userFullName = "BGTG\\kss";

            var savePath = _durationByTCPService.GetSavePath(userFullName);

            _webHostEnvironmentMock.VerifyGet(x => x.WebRootPath, Times.Once);
            Assert.AreEqual(@"www\UsersFiles\DurationByTCP\DurationByTCPBGTGkss.docx", savePath);
        }

        [Test]
        public void GetFileName()
        {
            var fileName = _durationByTCPService.GetFileName();

            Assert.AreEqual(@"ПРОД.docx", fileName);
        }
    }
}
