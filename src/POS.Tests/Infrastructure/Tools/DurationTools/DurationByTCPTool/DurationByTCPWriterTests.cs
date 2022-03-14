using System.IO;
using Moq;
using NUnit.Framework;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Tools.DurationTools.DurationByTCPTool;
using POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.Models;
using POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.TCP;
using POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.TCP.Models;
using Xceed.Words.NET;

namespace POS.Tests.Infrastructure.Tools.DurationTools.DurationByTCPTool;

public class DurationByTCPWriterTests
{
    private DurationByTCPWriter _durationByTCPWriter = null!;
    private Mock<IDocumentService> _documentServiceMock = null!;

    [SetUp]
    public void SetUp()
    {
        _documentServiceMock = new Mock<IDocumentService>();
        _durationByTCPWriter = new DurationByTCPWriter(_documentServiceMock.Object);
    }

    [Test]
    public void Write_InterpolationDuration()
    {
        var pipelineMaterial = "стальных";
        var pipelineDiameter = 0;
        var pipelineDiameterPresentation = "до 500";
        var pipelineLength = 13M;
        var appendix = TCP212.AppendixA;
        var durationCalculationType = DurationCalculationType.Interpolation;
        var duration = 10.8M;
        var roundedDuration = 11M;
        var preparatoryPeriod = 1.1M;
        var durationChange = 0.6M;
        var volumeChange = 3M;

        var calculationPipelineStandards = new[]
        {
            new PipelineStandard(10M, 9M, 0),
            new PipelineStandard(15M, 12M, 0),
        };

        var interpolationDuration = new InterpolationDurationByTCP(pipelineMaterial, pipelineDiameter,
            pipelineDiameterPresentation, pipelineLength, calculationPipelineStandards, durationCalculationType,
            duration,
            roundedDuration, preparatoryPeriod, durationChange, volumeChange, appendix.Key, appendix.Page);

        var templatePath = "Interpolation.docx";

        var memoryStream = _durationByTCPWriter.Write(interpolationDuration, templatePath);

        _documentServiceMock.Verify(x => x.Load(templatePath));
        _documentServiceMock.Verify(x => x.ReplaceText("%PM%", pipelineMaterial), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%PDP%", pipelineDiameterPresentation), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%PL%", pipelineLength.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%D%", duration.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%RD%", roundedDuration.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%AK%", appendix.Key.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%AP%", appendix.Page.ToString()), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceText("%DCT%", "интерполяции"));
        _documentServiceMock.Verify(x => x.ReplaceText("%DCTP%", "А"));

        _documentServiceMock.Verify(x =>
            x.ReplaceText("%ICPSPL0%", calculationPipelineStandards[0].PipelineLength.ToString()), Times.Once);
        _documentServiceMock.Verify(x =>
            x.ReplaceText("%ICPSPL1%", calculationPipelineStandards[1].PipelineLength.ToString()), Times.Once);
        _documentServiceMock.Verify(x =>
            x.ReplaceText("%ICPSD0%", calculationPipelineStandards[0].Duration.ToString()), Times.Once);
        _documentServiceMock.Verify(x =>
            x.ReplaceText("%ICPSD1%", calculationPipelineStandards[1].Duration.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%DC%", durationChange.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%VC%", volumeChange.ToString()), Times.Once);

        _documentServiceMock.Verify(x => x.SaveAs(It.IsAny<Stream>(), 0), Times.Once);
        _documentServiceMock.Verify(x => x.Dispose(), Times.Once);
        Assert.NotNull(memoryStream);
    }

    [Test]
    public void Write_ExtrapolationAscendingDuration()
    {
        var pipelineMaterial = "стальных";
        var pipelineDiameter = 0;
        var pipelineDiameterPresentation = "до 500";
        var pipelineLength = 25000M;
        var appendix = TCP212.AppendixA;
        var durationCalculationType = DurationCalculationType.ExtrapolationAscending;
        var duration = 11.4M;
        var roundedDuration = 11.5M;
        var preparatoryPeriod = 1.1M;
        var volumeChangePercent = 16.7M;
        var standardChangePercent = 5M;

        var calculationPipelineStandards = new[]
        {
            new PipelineStandard(30000M, 12M, 0),
        };

        var extrapolationAscendingDuration = new ExtrapolationDurationByTCP(pipelineMaterial, pipelineDiameter,
            pipelineDiameterPresentation, pipelineLength, calculationPipelineStandards, durationCalculationType,
            duration,
            roundedDuration, preparatoryPeriod, volumeChangePercent, standardChangePercent, appendix.Key,
            appendix.Page);

        var templatePath = "ExtrapolationAscending.docx";

        var memoryStream = _durationByTCPWriter.Write(extrapolationAscendingDuration, templatePath);

        _documentServiceMock.Verify(x => x.Load(templatePath));
        _documentServiceMock.Verify(x => x.ReplaceText("%PM%", pipelineMaterial), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%PDP%", pipelineDiameterPresentation), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%PL%", pipelineLength.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%D%", duration.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%RD%", roundedDuration.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%AK%", appendix.Key.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%AP%", appendix.Page.ToString()), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceText("%DCT%", "экстраполяции"));
        _documentServiceMock.Verify(x => x.ReplaceText("%DCTP%", "Б.2"));

        _documentServiceMock.Verify(x => x.ReplaceText("%ECPSPL%", calculationPipelineStandards[0].PipelineLength.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%ECPSD%", calculationPipelineStandards[0].Duration.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%VCP%", volumeChangePercent.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%SCP%", standardChangePercent.ToString()), Times.Once);

        _documentServiceMock.Verify(x => x.SaveAs(It.IsAny<Stream>(), 0), Times.Once);
        _documentServiceMock.Verify(x => x.Dispose(), Times.Once);
        Assert.NotNull(memoryStream);
    }

    [Test]
    public void Write_ExtrapolationDescendingDuration()
    {
        var pipelineMaterial = "стальных";
        var pipelineDiameter = 0;
        var pipelineDiameterPresentation = "до 500";
        var pipelineLength = 62700M;
        var appendix = TCP212.AppendixA;
        var durationCalculationType = DurationCalculationType.ExtrapolationDescending;
        var duration = 15M;
        var roundedDuration = 15M;
        var preparatoryPeriod = 1.5M;
        var volumeChangePercent = 25.4M;
        var standardChangePercent = 7.6M;

        var calculationPipelineStandards = new[]
        {
            new PipelineStandard(50000M, 14M, 0),
        };

        var extrapolationAscendingDuration = new ExtrapolationDurationByTCP(pipelineMaterial, pipelineDiameter,
            pipelineDiameterPresentation, pipelineLength, calculationPipelineStandards, durationCalculationType,
            duration,
            roundedDuration, preparatoryPeriod, volumeChangePercent, standardChangePercent, appendix.Key,
            appendix.Page);

        var templatePath = "ExtrapolationDescending.docx";

        var memoryStream = _durationByTCPWriter.Write(extrapolationAscendingDuration, templatePath);

        _documentServiceMock.Verify(x => x.Load(templatePath));
        _documentServiceMock.Verify(x => x.ReplaceText("%PM%", pipelineMaterial), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%PDP%", pipelineDiameterPresentation), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%PL%", pipelineLength.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%D%", duration.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%RD%", roundedDuration.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%AK%", appendix.Key.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%AP%", appendix.Page.ToString()), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceText("%DCT%", "экстраполяции"));
        _documentServiceMock.Verify(x => x.ReplaceText("%DCTP%", "Б.1"));

        _documentServiceMock.Verify(x => x.ReplaceText("%ECPSPL%", calculationPipelineStandards[0].PipelineLength.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%ECPSD%", calculationPipelineStandards[0].Duration.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%VCP%", volumeChangePercent.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%SCP%", standardChangePercent.ToString()), Times.Once);

        _documentServiceMock.Verify(x => x.SaveAs(It.IsAny<Stream>(), 0), Times.Once);
        _documentServiceMock.Verify(x => x.Dispose(), Times.Once);
        Assert.NotNull(memoryStream);
    }

    [Test]
    public void Write_StepwiseExtrapolationAscendingDuration()
    {
        var pipelineMaterial = "стальных";
        var pipelineDiameter = 0;
        var pipelineDiameterPresentation = "до 500";
        var pipelineLength = 7000M;
        var appendix = TCP212.AppendixA;
        var durationCalculationType = DurationCalculationType.StepwiseExtrapolationAscending;
        var duration = 13.9M;
        var roundedDuration = 14M;
        var preparatoryPeriod = 1.1M;
        var volumeChangePercent = 30M;
        var standardChangePercent = 9M;
        var stepwiseDuration = 15.3M;

        var calculationPipelineStandards = new[]
        {
            new PipelineStandard(20000M, 18M, 0),
        };

        var stepwisePipelineStandard = new PipelineStandard(10000M, stepwiseDuration, 0);

        var stepwiseExtrapolationDurationByTCP = new StepwiseExtrapolationDurationByTCP(pipelineMaterial,
            pipelineDiameter,
            pipelineDiameterPresentation, pipelineLength, calculationPipelineStandards, durationCalculationType,
            duration,
            roundedDuration, preparatoryPeriod, volumeChangePercent, standardChangePercent, stepwiseDuration,
            stepwisePipelineStandard, appendix.Key, appendix.Page);

        var templatePath = "StepwiseExtrapolationAscending.docx";

        var memoryStream = _durationByTCPWriter.Write(stepwiseExtrapolationDurationByTCP, templatePath);

        _documentServiceMock.Verify(x => x.Load(templatePath));
        _documentServiceMock.Verify(x => x.ReplaceText("%PM%", pipelineMaterial), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%PDP%", pipelineDiameterPresentation), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%PL%", pipelineLength.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%D%", duration.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%RD%", roundedDuration.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%AK%", appendix.Key.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%AP%", appendix.Page.ToString()), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceText("%DCT%", "ступенчатой экстраполяции"));
        _documentServiceMock.Verify(x => x.ReplaceText("%DCTP%", "В.2"));

        _documentServiceMock.Verify(x => x.ReplaceText("%ECPSPL%", calculationPipelineStandards[0].PipelineLength.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%ECPSD%", calculationPipelineStandards[0].Duration.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%VCP%", volumeChangePercent.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%SCP%", standardChangePercent.ToString()), Times.Once);


        _documentServiceMock.Verify(x => x.ReplaceText("%SD%", stepwiseDuration.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%SPSPL%", stepwisePipelineStandard.PipelineLength.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%SPSD%", stepwisePipelineStandard.Duration.ToString()), Times.Once);

        _documentServiceMock.Verify(x => x.SaveAs(It.IsAny<Stream>(), 0), Times.Once);
        _documentServiceMock.Verify(x => x.Dispose(), Times.Once);
        Assert.NotNull(memoryStream);
    }

    [Test]
    public void Write_StepwiseExtrapolationDescendingDuration()
    {
        var pipelineMaterial = "стальных";
        var pipelineDiameter = 0;
        var pipelineDiameterPresentation = "до 500";
        var pipelineLength = 22000M;
        var appendix = TCP212.AppendixA;
        var durationCalculationType = DurationCalculationType.StepwiseExtrapolationDescending;
        var duration = 29.5M;
        var roundedDuration = 29.5M;
        var preparatoryPeriod = 2.9M;
        var volumeChangePercent = 10M;
        var standardChangePercent = 3M;
        var stepwiseDuration = 28.6M;

        var calculationPipelineStandards = new[]
        {
            new PipelineStandard(10000M, 22M, 0),
        };

        var stepwisePipelineStandard = new PipelineStandard(20000M, stepwiseDuration, 0);

        var stepwiseExtrapolationDurationByTCP = new StepwiseExtrapolationDurationByTCP(pipelineMaterial,
            pipelineDiameter,
            pipelineDiameterPresentation, pipelineLength, calculationPipelineStandards, durationCalculationType,
            duration,
            roundedDuration, preparatoryPeriod, volumeChangePercent, standardChangePercent, stepwiseDuration,
            stepwisePipelineStandard, appendix.Key, appendix.Page);

        var templatePath = "StepwiseExtrapolationDescending.docx";

        var memoryStream = _durationByTCPWriter.Write(stepwiseExtrapolationDurationByTCP, templatePath);

        _documentServiceMock.Verify(x => x.Load(templatePath));
        _documentServiceMock.Verify(x => x.ReplaceText("%PM%", pipelineMaterial), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%PDP%", pipelineDiameterPresentation), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%PL%", pipelineLength.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%D%", duration.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%RD%", roundedDuration.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%AK%", appendix.Key.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%AP%", appendix.Page.ToString()), Times.Once);

        _documentServiceMock.Verify(x => x.ReplaceText("%DCT%", "ступенчатой экстраполяции"));
        _documentServiceMock.Verify(x => x.ReplaceText("%DCTP%", "В.1"));

        _documentServiceMock.Verify(x => x.ReplaceText("%ECPSPL%", calculationPipelineStandards[0].PipelineLength.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%ECPSD%", calculationPipelineStandards[0].Duration.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%VCP%", volumeChangePercent.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%SCP%", standardChangePercent.ToString()), Times.Once);


        _documentServiceMock.Verify(x => x.ReplaceText("%SD%", stepwiseDuration.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%SPSPL%", stepwisePipelineStandard.PipelineLength.ToString()), Times.Once);
        _documentServiceMock.Verify(x => x.ReplaceText("%SPSD%", stepwisePipelineStandard.Duration.ToString()), Times.Once);

        _documentServiceMock.Verify(x => x.SaveAs(It.IsAny<Stream>(), 0), Times.Once);
        _documentServiceMock.Verify(x => x.Dispose(), Times.Once);
        Assert.NotNull(memoryStream);
    }
}