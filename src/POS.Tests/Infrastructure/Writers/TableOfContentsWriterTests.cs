using Moq;
using NUnit.Framework;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers;
using System;
using Microsoft.AspNetCore.Hosting;
using POS.DomainModels;
using POS.Infrastructure.Replacers;
using POS.ViewModels;

namespace POS.Tests.Infrastructure.Writers;

public class TableOfContentsWriterTests
{
    private TableOfContentsWriter _tableOfContentsWriter = null!;
    private Mock<IWordDocumentService> _documentServiceMock = null!;
    private Mock<IWebHostEnvironment> _webHostEnvironmentMock = null!;
    private Mock<IEngineerReplacer> _engineerReplacerMock = null!;

    [SetUp]
    public void SetUp()
    {
        _documentServiceMock = new Mock<IWordDocumentService>();
        _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        _engineerReplacerMock = new Mock<IEngineerReplacer>();
        _tableOfContentsWriter = new TableOfContentsWriter(_documentServiceMock.Object, _webHostEnvironmentMock.Object, _engineerReplacerMock.Object);
    }

    [Test]
    public void Write_ConstructionObject548()
    {
        // arrange

        var viewModel = new TableOfContentsViewModel
        {
            ObjectCipher = "5.5-20.548",
            ProjectTemplate = ProjectTemplate.ECP,
            NormalInspectionEngineer = Engineer.Kapitan,
            ChiefProjectEngineer = Engineer.Saiko,
        };
        var templatePath = @"root\Infrastructure\Templates\TableOfContentsTemplates\ECPTableOfContentsTemplate.doc";

        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");

        // act

        var memoryStream = _tableOfContentsWriter.Write(viewModel);

        // assert

        _engineerReplacerMock.Verify(x => x.ReplaceEngineerSecondNameAndSignature(viewModel.ChiefProjectEngineer, TypeOfEngineer.ChiefProjectEngineer));
        _engineerReplacerMock.Verify(x => x.ReplaceEngineerSecondNameAndSignature(viewModel.NormalInspectionEngineer, TypeOfEngineer.NormalInspectionProjectEngineer));
        _documentServiceMock.Verify(x => x.Load(templatePath), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%CIPHER%", viewModel.ObjectCipher), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%DATE%", DateTime.Now.ToString(AppConstants.DateTimeMonthAndYearShortFormat)), Times.Once);

        _documentServiceMock.Verify(x => x.SaveAs(memoryStream, MyFileFormat.Doc, 0), Times.Once);
        _documentServiceMock.Verify(x => x.DisposeLastDocument(), Times.Once);

        Assert.NotNull(memoryStream);
    }
}