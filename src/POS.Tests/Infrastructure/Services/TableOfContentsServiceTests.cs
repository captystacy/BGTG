using System.IO;
using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;
using POS.DomainModels;
using POS.Infrastructure.Services;
using POS.Infrastructure.Writers.Base;
using POS.ViewModels;

namespace POS.Tests.Infrastructure.Services;

public class TableOfContentsServiceTests
{
    private TableOfContentsService _tableOfContentsService = null!;
    private Mock<ITableOfContentsWriter> _tableOfContentsWriterMock = null!;
    private Mock<IWebHostEnvironment> _webHostEnvironmentMock = null!;

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
            ChiefProjectEngineer = ChiefProjectEngineer.Saiko,
            ProjectEngineer = ProjectEngineer.Kapitan
        };

        var templatePath = @"root\Infrastructure\Templates\TableOfContentsTemplates\ECP\Saiko\Kapitan.doc";

        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");

        var expectedMemoryStream = new MemoryStream();
        _tableOfContentsWriterMock.Setup(x => x.Write(viewModel.ObjectCipher, templatePath)).Returns(expectedMemoryStream);

        var actualMemoryStream = _tableOfContentsService.Write(viewModel);

        Assert.AreSame(expectedMemoryStream, actualMemoryStream);
        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Once);

        _tableOfContentsWriterMock.Verify(x => x.Write(viewModel.ObjectCipher, templatePath), Times.Once);
    }
}