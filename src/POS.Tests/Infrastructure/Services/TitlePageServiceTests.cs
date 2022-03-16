using System.IO;
using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;
using POS.DomainModels;
using POS.Infrastructure.Services;
using POS.Infrastructure.Writers.Base;
using POS.ViewModels;

namespace POS.Tests.Infrastructure.Services;

public class TitlePageServiceTests
{
    private TitlePageService _titlePageService = null!;
    private Mock<ITitlePageWriter> _titlePageWriterMock = null!;
    private Mock<IWebHostEnvironment> _webHostEnvironmentMock = null!;

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

        var templatePath = @"root\Infrastructure\Templates\TitlePageTemplates\Saiko.docx";

        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");

        var expectedMemoryStream = new MemoryStream();
        _titlePageWriterMock.Setup(x => x.Write(viewModel.ObjectCipher, viewModel.ObjectName, templatePath)).Returns(expectedMemoryStream);

        var actualMemoryStream = _titlePageService.Write(viewModel);

        Assert.AreSame(expectedMemoryStream, actualMemoryStream);
        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Once);
        _titlePageWriterMock.Verify(x => x.Write(viewModel.ObjectCipher, viewModel.ObjectName, templatePath), Times.Once);
    }
}