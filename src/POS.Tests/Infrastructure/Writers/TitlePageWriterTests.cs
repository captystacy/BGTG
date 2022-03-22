using System;
using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;
using POS.DomainModels;
using POS.Infrastructure.Replacers;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers;
using POS.ViewModels;

namespace POS.Tests.Infrastructure.Writers;

public class TitlePageWriterTests
{
    private TitlePageWriter _titlePageWriter = null!;
    private Mock<IWordDocumentService> _documentServiceMock = null!;
    private Mock<IEngineerReplacer> _engineerReplacerMock = null!;
    private Mock<IWebHostEnvironment> _webHostEnvironmentMock = null!;

    [SetUp]
    public void SetUp()
    {
        _documentServiceMock = new Mock<IWordDocumentService>();
        _engineerReplacerMock = new Mock<IEngineerReplacer>();
        _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        _titlePageWriter = new TitlePageWriter(_documentServiceMock.Object, _engineerReplacerMock.Object, _webHostEnvironmentMock.Object);
    }

    [Test]
    public void Write_ConstructionObject548()
    {
        var viewModel = new TitlePageViewModel
        {
            ObjectCipher = "5.5-20.548",
            ObjectName = "Электроснабжение станции катодной защиты (СКЗ)№36 аг.Снов Несвижского района",
            ChiefProjectEngineer = Engineer.Saiko,
        };

        var templatePath = @"root\Infrastructure\Templates\TitlePageTemplates\TitlePageTemplate.doc";
        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");

        var objectName = "Электроснабжение станции катодной защиты (СКЗ)№36 аг.Снов Несвижского района";
        var objectCipher = "5.5-20.548";

        var memoryStream = _titlePageWriter.Write(viewModel);

        _engineerReplacerMock.Verify(x => x.ReplaceEngineerSecondNameAndSignature(Engineer.Saiko, TypeOfEngineer.ChiefProjectEngineer), Times.Once);
        _engineerReplacerMock.Verify(x => x.ReplaceEngineerSecondNameAndSignature(Engineer.Cherota, TypeOfEngineer.ChiefOrganizationEngineer), Times.Once);
        _documentServiceMock.Verify(x => x.Load(templatePath), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%NAME%", objectName), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%CIPHER%", objectCipher), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%YEAR%", DateTime.Now.Year.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.SaveAs(memoryStream, MyFileFormat.Doc, 0), Times.Once);
        _documentServiceMock.Verify(x => x.DisposeLastDocument(), Times.Once);
        Assert.NotNull(memoryStream);
    }
}