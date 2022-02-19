using BGTG.POS;
using BGTG.POS.TitlePageTool.Base;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.POSServices;
using BGTG.Web.ViewModels.POSViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace BGTG.Web.Tests.Infrastructure.Services.POS;

public class TitlePageServiceTests
{
    private TitlePageService _titlePageService = null!;
    private Mock<ITitlePageWriter> _titlePageWriterMock = null!;
    private Mock<IWebHostEnvironment> _webHostEnvironmentMock = null!;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock = null!;

    [SetUp]
    public void SetUp()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        IdentityHelper.Instance.Configure(_httpContextAccessorMock.Object);
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

        IdentityFake.Setup(_httpContextAccessorMock, "BGTG\\kss");

        var templatePath = @"root\AppData\Templates\POSTemplates\TitlePageTemplates\Saiko.docx";
        var savePath = @"root\AppData\UserFiles\POSFiles\TitlePageFiles\BGTGkss.docx";

        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");

        _titlePageService.Write(viewModel);

        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Exactly(2));

        _titlePageWriterMock.Verify(x => x.Write(viewModel.ObjectCipher, viewModel.ObjectName, templatePath, savePath), Times.Once);
    }

    [Test]
    public void GetSavePath()
    {
        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");
        IdentityFake.Setup(_httpContextAccessorMock, "BGTG\\kss");

        var savePath = _titlePageService.GetSavePath();

        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Once);
        Assert.AreEqual(@"root\AppData\UserFiles\POSFiles\TitlePageFiles\BGTGkss.docx", savePath);
    }
}