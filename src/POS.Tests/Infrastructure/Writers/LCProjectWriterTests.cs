using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using POS.DomainModels;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Creators.Base;
using POS.Infrastructure.Replacers;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;
using POS.ViewModels;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace POS.Tests.Infrastructure.Writers;

public class LCProjectWriterTests
{
    private Mock<IWordDocumentService> _documentServiceMock = null!;
    private Mock<IEmployeesNeedCreator> _employeesNeedCreatorMock = null!;
    private Mock<IEngineerReplacer> _engineerReplacerMock = null!;
    private Mock<IWebHostEnvironment> _webHostEnvironmentMock = null!;
    private LCProjectWriter _lcProjectWriter = null!;

    [SetUp]
    public void SetUp()
    {
        _documentServiceMock = new Mock<IWordDocumentService>();
        _employeesNeedCreatorMock = new Mock<IEmployeesNeedCreator>();
        _engineerReplacerMock = new Mock<IEngineerReplacer>();
        _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        _lcProjectWriter = new LCProjectWriter(_documentServiceMock.Object, _employeesNeedCreatorMock.Object, _engineerReplacerMock.Object, _webHostEnvironmentMock.Object);
    }

    [Test]
    public void Write()
    {
        // arrange
        var viewModel = new ProjectViewModel
        {
            CalculationFiles = new FormFileCollection
            {
                new FormFile(new MemoryStream(), default, default, default, "Продолжительность по трудозатратам"),
                new FormFile(new MemoryStream(), default, default, default, "Календарный план"),
                new FormFile(new MemoryStream(), default, default, default, "Энергия и вода")
            },
            ObjectCipher = "5.5-20.548",
            ProjectTemplate = ProjectTemplate.ECP,
            ProjectEngineer = Engineer.Kapitan,
            NormalInspectionEngineer = Engineer.Prishep,
            ChiefEngineer = Engineer.Selivanova,
            ChiefProjectEngineer = Engineer.Saiko,
        };
        var paragraphTextInDocument = "Принимаем продолжительность строительства равную 0,6 мес, в том числе подготовительный период – 0,06 мес, приемка объекта в эксплуатацию – 0,5 мес.";
        _documentServiceMock.Setup(x => x.ParagraphTextInDocument)
            .Returns(paragraphTextInDocument);
        _documentServiceMock.Setup(x => x.ParagraphsCountInDocument)
            .Returns(5);
        _documentServiceMock.SetupSequence(x => x.ParagraphTextInCell)
            .Returns("16")
            .Returns("4")
            .Returns("1,5")
            .Returns("август 2022");

        var employeesNeed = new EmployeesNeed(6, 5, 1, 4, 4.2M, 0.3M, 1, 2.58M, 1, 1, 12);
        _employeesNeedCreatorMock.Setup(x => x.Create(4, 1.5M)).Returns(employeesNeed);

        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");
        var templatePath = @"root\Infrastructure\Templates\ProjectTemplates\ECPProjectTemplate.doc";

        // act

        var memoryStream = _lcProjectWriter.Write(viewModel);

        // assert

        _documentServiceMock.Verify(x => x.Load(templatePath), Times.Once);
        _documentServiceMock.Verify(x => x.Load(It.IsAny<Stream>()), Times.Exactly(3));
        _documentServiceMock.Verify(x => x.SaveAs(memoryStream, MyFileFormat.Doc, 0), Times.Once);
        _documentServiceMock.Verify(x => x.DisposeLastDocument(), Times.Exactly(4));
        Assert.NotNull(memoryStream);

        _engineerReplacerMock.Verify(x => x.ReplaceEngineerSecondNameAndSignature(viewModel.ProjectEngineer, TypeOfEngineer.ProjectEngineer), Times.Once);
        _engineerReplacerMock.Verify(x => x.ReplaceEngineerSecondNameAndSignature(viewModel.NormalInspectionEngineer, TypeOfEngineer.NormalInspectionProjectEngineer), Times.Once);
        _engineerReplacerMock.Verify(x => x.ReplaceEngineerSecondNameAndSignature(viewModel.ChiefEngineer, TypeOfEngineer.ChiefEngineer), Times.Once);
        _engineerReplacerMock.Verify(x => x.ReplaceEngineerSecondNameAndSignature(viewModel.ChiefProjectEngineer, TypeOfEngineer.ChiefProjectEngineer), Times.Once);

        
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%CIPHER%", viewModel.ObjectCipher), Times.Once);
        _documentServiceMock.Verify(
            x => x.ReplaceTextInDocument("%DATE%", DateTime.Now.ToString(AppConstants.DateTimeMonthAndYearShortFormat)),
            Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%DURATION_BY_LC_FIRST_PARAGRAPH%", paragraphTextInDocument), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextWithTable("%DURATION_BY_LC_TABLE%"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextWithTable("%DURATION_BY_LC_DESCRIPTION_TABLE%"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%DURATION_BY_LC_PENULTIMATE_PARAGRAPH%", paragraphTextInDocument), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%DURATION_BY_LC_LAST_PARAGRAPH%", paragraphTextInDocument), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%TD%", "0,6"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%PP%", "0,06"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%AT%", "0,5"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%TLC%", "16"), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%TNOE%", "6"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%NOWE%", "5"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%NOM%", "1"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%FR%", "4"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%DR%", "4,2"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%WR%", "0,3"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%WC%", "1"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%SR%", "2,58"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%SM%", "1"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%T%", "1"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%FP%", "12"), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceTextWithTable("%CALENDAR_PLAN_PREPARATORY_TABLE%"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextWithTable("%CALENDAR_PLAN_MAIN_TABLE%"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%CONSTRUCTION_START_DATE%", "август 2022"), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceTextInDocument("%CY%", "2022"), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceTextWithTable("%ENERGY_AND_WATER_TABLE%"), Times.Once);
    }
}