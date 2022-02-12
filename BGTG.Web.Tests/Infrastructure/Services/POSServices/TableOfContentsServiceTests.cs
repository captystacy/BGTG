using BGTG.POS;
using BGTG.POS.ProjectTool;
using BGTG.POS.TableOfContentsTool.Interfaces;
using BGTG.Web.Infrastructure.Services.POSServices;
using BGTG.Web.ViewModels.POSViewModels;
using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;

namespace BGTG.Web.Tests.Infrastructure.Services.POSServices
{
    public class TableOfContentsServiceTests
    {
        private TableOfContentsService _tableOfContentsService;
        private Mock<ITableOfContentsWriter> _tableOfContentsWriterMock;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;

        [SetUp]
        public void SetUp()
        {
            _tableOfContentsWriterMock = new Mock<ITableOfContentsWriter>();
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            _tableOfContentsService = new TableOfContentsService(_tableOfContentsWriterMock.Object, _webHostEnvironmentMock.Object);
        }

        [Test]
        public void Write()
        {
            var viewModel = new TableOfContentsViewModel
            {
                ObjectCipher = "5.5-20.548",
                ProjectTemplate = ProjectTemplate.ECP,
                ChiefProjectEngineer = ChiefProjectEngineer.Saiko
            };
            var identityName = "BGTG\\kss";

            var templatePath = @"wwwroot\AppData\Templates\TableOfContentsTemplates\ECP\Saiko\Unknown.docx";
            var savePath = @"wwwroot\AppData\UserFiles\TableOfContentsFiles\BGTGkss.docx";

            _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");

            _tableOfContentsService.Write(viewModel, identityName);

            _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Exactly(3));

            _tableOfContentsWriterMock.Verify(x => x.Write(viewModel.ObjectCipher, templatePath, savePath), Times.Once);
        }

        [Test]
        public void GetSavePath()
        {
            _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");
            var identityName = "BGTG\\kss";

            var savePath = _tableOfContentsService.GetSavePath(identityName);

            _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Once);
            Assert.AreEqual(@"wwwroot\AppData\UserFiles\TableOfContentsFiles\BGTGkss.docx", savePath);
        }
    }
}
