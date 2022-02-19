using BGTG.POS;
using BGTG.POS.ProjectTool;
using BGTG.POS.TableOfContentsTool.Base;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.POSServices;
using BGTG.Web.ViewModels.POSViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace BGTG.Web.Tests.Infrastructure.Services.POS;

public class TableOfContentsServiceTests
{
    private TableOfContentsService _tableOfContentsService = null!;
    private Mock<ITableOfContentsWriter> _tableOfContentsWriterMock = null!;
    private Mock<IWebHostEnvironment> _webHostEnvironmentMock = null!;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock = null!;

    [SetUp]
    public void SetUp()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        IdentityHelper.Instance.Configure(_httpContextAccessorMock.Object);
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

        var templatePath = @"root\AppData\Templates\POSTemplates\TableOfContentsTemplates\ECP\Saiko\Unknown.docx";
        var savePath = @"root\AppData\UserFiles\POSFiles\TableOfContentsFiles\BGTGkss.docx";

        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");
        IdentityFake.Setup(_httpContextAccessorMock, "BGTG\\kss");

        _tableOfContentsService.Write(viewModel);

        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Exactly(3));

        _tableOfContentsWriterMock.Verify(x => x.Write(viewModel.ObjectCipher, templatePath, savePath), Times.Once);
    }

    [Test]
    public void GetSavePath()
    {
        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");
        IdentityFake.Setup(_httpContextAccessorMock, "BGTG\\kss");

        var savePath = _tableOfContentsService.GetSavePath();

        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Once);
        Assert.AreEqual(@"root\AppData\UserFiles\POSFiles\TableOfContentsFiles\BGTGkss.docx", savePath);
    }
}