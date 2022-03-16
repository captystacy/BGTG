using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using POS.DomainModels;
using POS.DomainModels.EstimateDomainModels;
using POS.Infrastructure.Creators.Base;
using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;
using POS.ViewModels;

namespace POS.Tests.Infrastructure.Services;

public class DurationByLCServiceTests
{
    private DurationByLCService _durationByLCService = null!;
    private Mock<IEstimateService> _estimateServiceMock = null!;
    private Mock<IDurationByLCCreator> _durationByLCCreatorMock = null!;
    private Mock<IDurationByLCWriter> _durationByLCWriterMock = null!;
    private Mock<IWebHostEnvironment> _webHostEnvironmentMock = null!;

    [SetUp]
    public void SetUp()
    {
        _estimateServiceMock = new Mock<IEstimateService>();
        _durationByLCCreatorMock = new Mock<IDurationByLCCreator>();
        _durationByLCWriterMock = new Mock<IDurationByLCWriter>();
        _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        _durationByLCService = new DurationByLCService(_estimateServiceMock.Object, _durationByLCCreatorMock.Object,
            _durationByLCWriterMock.Object, _webHostEnvironmentMock.Object);

    }

    [Test]
    public void Write()
    {
        var estimateFiles = new FormFileCollection();
        var durationByLCCreateViewModel = new DurationByLCViewModel
        {
            EstimateFiles = estimateFiles,
            AcceptanceTimeIncluded = true,
            NumberOfEmployees = 4,
            NumberOfWorkingDays = 21.5M,
            Shift = 1.5M,
            WorkingDayDuration = 8,
            TechnologicalLaborCosts = 110,
        };
        var estimate = new Estimate(default!, default!, default, default, default, default);
        _estimateServiceMock.Setup(x => x.Estimate).Returns(estimate);
        var durationByLC = new DurationByLC(default, default, default,
            durationByLCCreateViewModel.TechnologicalLaborCosts, default, default, default, default, default, default,
            default, default, true, true);

        var templatePath = @"root\Infrastructure\Templates\DurationByLCTemplates\Rounding+Acceptance+.docx";

        _durationByLCCreatorMock.Setup(x => x.Create(estimate.LaborCosts,
                durationByLCCreateViewModel.TechnologicalLaborCosts,
                durationByLCCreateViewModel.WorkingDayDuration, durationByLCCreateViewModel.Shift,
                durationByLCCreateViewModel.NumberOfWorkingDays,
                durationByLCCreateViewModel.NumberOfEmployees, durationByLCCreateViewModel.AcceptanceTimeIncluded))
            .Returns(durationByLC);

        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");

        var expectedMemoryStream = new MemoryStream();
        _durationByLCWriterMock.Setup(x => x.Write(durationByLC, templatePath)).Returns(expectedMemoryStream);
        var actualMemoryStream = _durationByLCService.Write(durationByLCCreateViewModel);

        Assert.AreSame(expectedMemoryStream, actualMemoryStream);
        _estimateServiceMock.Verify(x => x.Read(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter), Times.Once);
        _durationByLCCreatorMock.Verify(x => x.Create(estimate.LaborCosts,
            durationByLCCreateViewModel.TechnologicalLaborCosts,
            durationByLCCreateViewModel.WorkingDayDuration, durationByLCCreateViewModel.Shift,
            durationByLCCreateViewModel.NumberOfWorkingDays,
            durationByLCCreateViewModel.NumberOfEmployees, durationByLCCreateViewModel.AcceptanceTimeIncluded), Times.Once);
        _durationByLCWriterMock.Verify(x => x.Write(durationByLC, templatePath), Times.Once);
        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Once);
        _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Once);
    }
}