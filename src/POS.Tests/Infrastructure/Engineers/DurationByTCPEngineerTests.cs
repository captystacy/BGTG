using NUnit.Framework;
using POS.DomainModels.DurationByTCPDomainModels;
using POS.DomainModels.DurationByTCPDomainModels.TCPDomainModels;
using POS.Infrastructure.Engineers;

namespace POS.Tests.Infrastructure.Engineers;

public class DurationByTCPEngineerTests
{
    private DurationByTCPEngineer _durationByTCPEngineer = null!;

    [SetUp]
    public void SetUp()
    {
        _durationByTCPEngineer = new DurationByTCPEngineer();
    }

    [Test]
    public void DefineCalculationType_PipelineLengthBelowStandards_FirstStandardAndExtrapolationAscending()
    {
        var pipelineLength = 0.05M;

        var pipelineStandards = new[]
        {
            new PipelineStandard(0.1M, 1M, 0.3M),
            new PipelineStandard(0.5M, 2M, 0.3M),
            new PipelineStandard(1M, 2.5M, 0.3M),
            new PipelineStandard(1.5M, 4M, 0.5M),
        };

        var expectedCalculationPipelineStandards = new[]
        {
            new PipelineStandard(0.1M, 1M, 0.3M),
        };

        var expectedDurationCalculationType = DurationCalculationType.ExtrapolationAscending;

        _durationByTCPEngineer.DefineCalculationType(pipelineStandards, pipelineLength);

        Assert.AreEqual(expectedDurationCalculationType, _durationByTCPEngineer.DurationCalculationType);
        Assert.That(_durationByTCPEngineer.CalculationPipelineStandards, Is.EquivalentTo(expectedCalculationPipelineStandards));
    }

    [Test]
    public void DefineCalculationType_PipelineLengthBelowStandards_FirstStandardAndExtrapolationStepwiseAscending()
    {
        var pipelineLength = 0.049M;

        var pipelineStandards = new[]
        {
            new PipelineStandard(0.1M, 1M, 0.3M),
            new PipelineStandard(0.5M, 2M, 0.3M),
            new PipelineStandard(1M, 2.5M, 0.3M),
            new PipelineStandard(1.5M, 4M, 0.5M),
        };

        var expectedCalculationPipelineStandards = new[]
        {
            new PipelineStandard(0.1M, 1M, 0.3M),
        };

        var expectedDurationCalculationType = DurationCalculationType.StepwiseExtrapolationAscending;

        _durationByTCPEngineer.DefineCalculationType(pipelineStandards, pipelineLength);

        Assert.AreEqual(expectedDurationCalculationType, _durationByTCPEngineer.DurationCalculationType);
        Assert.That(_durationByTCPEngineer.CalculationPipelineStandards, Is.EquivalentTo(expectedCalculationPipelineStandards));
    }

    [Test]
    public void DefineCalculationType_PipelineLengthAboveStandards_LastStandardAndExtrapolationDescending()
    {
        var pipelineLength = 3M;

        var pipelineStandards = new[]
        {
            new PipelineStandard(0.1M, 1M, 0.3M),
            new PipelineStandard(0.5M, 2M, 0.3M),
            new PipelineStandard(1M, 2.5M, 0.3M),
            new PipelineStandard(1.5M, 4M, 0.5M),
        };

        var expectedCalculationPipelineStandards = new[]
        {
            new PipelineStandard(1.5M, 4M, 0.5M),
        };

        var expectedDurationCalculationType = DurationCalculationType.ExtrapolationDescending;

        _durationByTCPEngineer.DefineCalculationType(pipelineStandards, pipelineLength);

        Assert.AreEqual(expectedDurationCalculationType, _durationByTCPEngineer.DurationCalculationType);
        Assert.That(_durationByTCPEngineer.CalculationPipelineStandards, Is.EquivalentTo(expectedCalculationPipelineStandards));
    }

    [Test]
    public void DefineCalculationType_PipelineLengthAboveStandards_LastStandardAndExtrapolationStepwiseDescending()
    {
        var pipelineLength = 3.01M;

        var pipelineStandards = new[]
        {
            new PipelineStandard(0.1M, 1M, 0.3M),
            new PipelineStandard(0.5M, 2M, 0.3M),
            new PipelineStandard(1M, 2.5M, 0.3M),
            new PipelineStandard(1.5M, 4M, 0.5M),
        };

        var expectedCalculationPipelineStandards = new[]
        {
            new PipelineStandard(1.5M, 4M, 0.5M),
        };

        var expectedDurationCalculationType = DurationCalculationType.StepwiseExtrapolationDescending;

        _durationByTCPEngineer.DefineCalculationType(pipelineStandards, pipelineLength);

        Assert.AreEqual(expectedDurationCalculationType, _durationByTCPEngineer.DurationCalculationType);
        Assert.That(_durationByTCPEngineer.CalculationPipelineStandards, Is.EquivalentTo(expectedCalculationPipelineStandards));
    }

    [Test]
    public void DefineCalculationType_PipelineLengthBetweenStandards_PreviousAndNextStandardsAndInterpolation()
    {
        var pipelineLength = 0.25M;

        var pipelineStandards = new[]
        {
            new PipelineStandard(0.1M, 1M, 0.3M),
            new PipelineStandard(0.5M, 2M, 0.3M),
            new PipelineStandard(1M, 2.5M, 0.3M),
            new PipelineStandard(1.5M, 4M, 0.5M),
        };

        var expectedCalculationPipelineStandards = new[]
        {
            new PipelineStandard(0.1M, 1M, 0.3M),
            new PipelineStandard(0.5M, 2M, 0.3M),
        };

        var expectedDurationCalculationType = DurationCalculationType.Interpolation;

        _durationByTCPEngineer.DefineCalculationType(pipelineStandards, pipelineLength);

        Assert.AreEqual(expectedDurationCalculationType, _durationByTCPEngineer.DurationCalculationType);
        Assert.That(_durationByTCPEngineer.CalculationPipelineStandards, Is.EquivalentTo(expectedCalculationPipelineStandards));
    }
}