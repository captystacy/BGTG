using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;
using POS.Infrastructure.Services;
using POS.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Http;
using POS.DomainModels;
using POS.Infrastructure.Writers.Base;

namespace POS.Tests.Infrastructure.Services;

public class ProjectServiceTests
{
    private ProjectService _projectService = null!;
    private Mock<IECPProjectWriter> _ecpProjectWriterMock = null!;
    private Mock<IWebHostEnvironment> _webHostEnvironmentMock = null!;

    [SetUp]
    public void SetUp()
    {
        _ecpProjectWriterMock = new Mock<IECPProjectWriter>();
        _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        _projectService = new ProjectService(_ecpProjectWriterMock.Object, _webHostEnvironmentMock.Object);
    }

    [Test]
    public void Write()
    {
        var calculationFiles = new FormFileCollection
        {
            new FormFile(new MemoryStream(), default, default, default, "Продолжительность по трудозатратам"),
            new FormFile(new MemoryStream(), default, default, default, "Календарный план"),
            new FormFile(new MemoryStream(), default, default, default, "Энергия и вода")
        };

        var viewModel = new ProjectViewModel
        {
            CalculationFiles = calculationFiles,
            ObjectCipher = "5.5-20.548",
            ChiefProjectEngineer = ChiefProjectEngineer.Saiko,
            ProjectTemplate = ProjectTemplate.ECP,
            HouseholdTownIncluded = true,
        };

        var templatePath = @"root\Infrastructure\Templates\ProjectTemplates\ECP\Saiko\Unknown\Employees4\HouseholdTown+.doc";

        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");
        _ecpProjectWriterMock
            .Setup(x => x.GetNumberOfEmployees(It.IsAny<Stream>()))
            .Returns(4);

        var expectedMemoryStream = new MemoryStream();
        _ecpProjectWriterMock
            .Setup(x => x.Write(It.IsAny<Stream>(), It.IsAny<Stream>(), It.IsAny<Stream>(), viewModel.ObjectCipher, templatePath))
            .Returns(expectedMemoryStream);

        var actualMemoryStream = _projectService.Write(viewModel);

        Assert.AreSame(expectedMemoryStream, actualMemoryStream);
        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Once);
        _ecpProjectWriterMock.Verify(
            x => x.Write(It.IsAny<Stream>(), It.IsAny<Stream>(), It.IsAny<Stream>(), viewModel.ObjectCipher, templatePath), Times.Once);
        _ecpProjectWriterMock.Verify(x => x.GetNumberOfEmployees(It.IsAny<Stream>()), Times.Once);
    }
}