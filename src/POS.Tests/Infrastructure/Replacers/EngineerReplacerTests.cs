using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;
using POS.DomainModels;
using POS.Infrastructure.Replacers;
using POS.Infrastructure.Services.Base;

namespace POS.Tests.Infrastructure.Replacers;

public class EngineerReplacerTests
{
    private IEngineerReplacer _engineerReplacer = null!;
    private Mock<IWebHostEnvironment> _webHostEnvironmentMock = null!;
    private Mock<IWordDocumentService> _documentServiceMock = null!;

    [SetUp]
    public void SetUp()
    {
        _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        _documentServiceMock = new Mock<IWordDocumentService>();
        _engineerReplacer = new EngineerReplacer(_documentServiceMock.Object, _webHostEnvironmentMock.Object);
    }


    [Test]
    public void ReplaceEngineerSecondNameAndSignature_ChiefProjectEngineer()
    {
        // arrange

        var engineer = Engineer.Saiko;
        var typeOfEngineer = TypeOfEngineer.ChiefProjectEngineer;
        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");
        var signaturePath = @"root\Infrastructure\Templates\EngineerSignatures\Saiko.png";

        // act

        _engineerReplacer.ReplaceEngineerSecondNameAndSignature(engineer, typeOfEngineer);

        // assert
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%CPEFN%", "С.М. Сайко"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%CPESN%", "Сайко"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextWithImage("%CPES%", signaturePath), Times.Once);
    }

    [Test]
    public void ReplaceEngineerSecondNameAndSignature_ProjectEngineer()
    {
        // arrange

        var engineer = Engineer.Kapitan;
        var typeOfEngineer = TypeOfEngineer.ProjectEngineer;
        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");
        var signaturePath = @"root\Infrastructure\Templates\EngineerSignatures\Kapitan.png";

        // act

        _engineerReplacer.ReplaceEngineerSecondNameAndSignature(engineer, typeOfEngineer);

        // assert
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%PESN%", "Капитан"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextWithImage("%PES%", signaturePath), Times.Once);
    }

    [Test]
    public void ReplaceEngineerSecondNameAndSignature_NormalInspectionEngineer()
    {
        // arrange

        var engineer = Engineer.Prishep;
        var typeOfEngineer = TypeOfEngineer.NormalInspectionProjectEngineer;
        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");
        var signaturePath = @"root\Infrastructure\Templates\EngineerSignatures\Prishep.png";

        // act

        _engineerReplacer.ReplaceEngineerSecondNameAndSignature(engineer, typeOfEngineer);

        // assert
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%NIESN%", "Прищеп"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextWithImage("%NIES%", signaturePath), Times.Once);
    }

    [Test]
    public void ReplaceEngineerSecondNameAndSignature_ChiefEngineer()
    {
        // arrange

        var engineer = Engineer.Selivanova;
        var typeOfEngineer = TypeOfEngineer.ChiefEngineer;
        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");
        var signaturePath = @"root\Infrastructure\Templates\EngineerSignatures\Selivanova.png";

        // act

        _engineerReplacer.ReplaceEngineerSecondNameAndSignature(engineer, typeOfEngineer);

        // assert
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%CESN%", "Селиванова"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextWithImage("%CES%", signaturePath), Times.Once);
    }


    [Test]
    public void ReplaceEngineerSecondNameAndSignature_UnknownEngineer()
    {
        // arrange

        var engineer = Engineer.Unknown;
        var typeOfEngineer = TypeOfEngineer.ProjectEngineer;
        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");
        var signaturePath = @"root\Infrastructure\Templates\EngineerSignatures\Kapitan.png";

        // act

        _engineerReplacer.ReplaceEngineerSecondNameAndSignature(engineer, typeOfEngineer);

        // assert
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%PESN%", ""), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextWithImage("%PES%", signaturePath), Times.Never);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%PES%", ""), Times.Once);
    }
}