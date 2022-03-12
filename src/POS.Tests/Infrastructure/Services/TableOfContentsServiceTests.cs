﻿using System.IO;
using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;
using POS.Infrastructure.Services;
using POS.Infrastructure.Tools;
using POS.Infrastructure.Tools.ProjectTool;
using POS.Infrastructure.Tools.TableOfContentsTool;
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
            ChiefProjectEngineer = ChiefProjectEngineer.Saiko
        };

        var templatePath = @"root\Templates\TableOfContentsTemplates\ECP\Saiko\Unknown.docx";

        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");

        var expectedMemoryStream = new MemoryStream();
        _tableOfContentsWriterMock.Setup(x => x.Write(viewModel.ObjectCipher, templatePath)).Returns(expectedMemoryStream);

        var actualMemoryStream = _tableOfContentsService.Write(viewModel);

        Assert.AreSame(expectedMemoryStream, actualMemoryStream);
        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Exactly(2));

        _tableOfContentsWriterMock.Verify(x => x.Write(viewModel.ObjectCipher, templatePath), Times.Once);
    }
}