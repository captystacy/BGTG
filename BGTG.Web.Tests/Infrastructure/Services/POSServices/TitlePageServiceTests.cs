using BGTG.POS;
using BGTG.POS.TitlePageTool.Interfaces;
using BGTG.Web.Infrastructure.Services.POSServices;
using BGTG.Web.ViewModels.POSViewModels;
using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;

namespace BGTG.Web.Tests.Infrastructure.Services.POSServices
{
    public class TitlePageServiceTests
    {
        private TitlePageService _titlePageService;
        private Mock<ITitlePageWriter> _titlePageWriterMock;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;

        [SetUp]
        public void SetUp()
        {
            _titlePageWriterMock = new Mock<ITitlePageWriter>();
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            _titlePageService = new TitlePageService(_titlePageWriterMock.Object, _webHostEnvironmentMock.Object);
        }

        [Test]
        public void Write()
        {
            var viewModel = new TitlePageViewModel
            {
                ChiefProjectEngineer = ChiefProjectEngineer.Saiko,
                ObjectCipher = "5.5-20.548",
                ObjectName = "Электроснабжение станции катодной защиты (СКЗ)№36 аг.Снов Несвижского района"
            };
            var identityName = "BGTG\\kss";

            var templatePath = @"wwwroot\AppData\Templates\TitlePageTemplates\Saiko.docx";
            var savePath = @"wwwroot\AppData\UserFiles\TitlePageFiles\BGTGkss.docx";

            _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");

            _titlePageService.Write(viewModel, identityName);

            _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Exactly(2));

            _titlePageWriterMock.Verify(x => x.Write(viewModel.ObjectCipher, viewModel.ObjectName, templatePath, savePath), Times.Once);
        }

        [Test]
        public void GetSavePath()
        {
            _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");
            var identityName = "BGTG\\kss";

            var savePath = _titlePageService.GetSavePath(identityName);

            _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Once);
            Assert.AreEqual(@"wwwroot\AppData\UserFiles\TitlePageFiles\BGTGkss.docx", savePath);
        }
    }
}
