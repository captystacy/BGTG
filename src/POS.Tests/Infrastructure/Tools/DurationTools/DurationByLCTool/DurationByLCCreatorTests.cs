using Moq;
using NUnit.Framework;
using POS.Infrastructure.Tools.DurationTools.Base;
using POS.Infrastructure.Tools.DurationTools.DurationByLCTool;

namespace POS.Tests.Infrastructure.Tools.DurationTools.DurationByLCTool;

public class DurationByLCCreatorTests
{
    private DurationByLCCreator _durationByLCCreator = null!;
    private Mock<IDurationRounder> _durationRounder = null!;

    [SetUp]
    public void SetUp()
    {
        _durationRounder = new Mock<IDurationRounder>();
        _durationByLCCreator = new DurationByLCCreator(_durationRounder.Object);
    }

    [Test]
    public void Create_004Duration_CreateCorrectDurationByLC()
    {
        var estimateLaborCosts = 43M;
        var technologicalLaborCosts = 0M;
        var totalLaborCosts = 43M;
        var workingDayDuration = 8M;
        var shift = 1.5M;
        var numberOfWorkingDaysInMonth = 21.5M;
        var numberOfEmployees = 4;
        var acceptanceTimeIncluded = true;
        var duration = 0.04M;
        var totalDuration = 0.6M;
        var preparatoryPeriod = 0.06M;
        var roundedDuration = 0.1M;
        var acceptanceTime = 0.5M;
        var roundingIncluded = false;

        _durationRounder.Setup(x => x.GetRoundedDuration(0.0416666666666666666666666667M)).Returns(roundedDuration);
        _durationRounder.Setup(x => x.GetRoundedPreparatoryPeriod(totalDuration)).Returns(preparatoryPeriod);

        var expectedDurationByLC = new DurationByLC(duration, totalLaborCosts, estimateLaborCosts,
            technologicalLaborCosts, workingDayDuration,
            shift, numberOfWorkingDaysInMonth,
            numberOfEmployees, totalDuration, preparatoryPeriod, roundedDuration, acceptanceTime,
            acceptanceTimeIncluded, roundingIncluded);


        var actualDurationByLC = _durationByLCCreator.Create(estimateLaborCosts, technologicalLaborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, acceptanceTimeIncluded);

        Assert.AreEqual(expectedDurationByLC, actualDurationByLC);
        _durationRounder.Verify(x => x.GetRoundedDuration(0.0416666666666666666666666667M), Times.Once);
        _durationRounder.Verify(x => x.GetRoundedPreparatoryPeriod(totalDuration), Times.Once);
    }

    [Test]
    public void Create_038Duration_CreateCorrectDurationByLC()
    {
        var estimateLaborCosts = 390M;
        var technologicalLaborCosts = 0M;
        var totalLaborCosts = 390M;
        var workingDayDuration = 8M;
        var shift = 1.5M;
        var numberOfWorkingDaysInMonth = 21.5M;
        var numberOfEmployees = 4;
        var acceptanceTimeIncluded = true;
        var duration = 0.38M;
        var totalDuration = 1M;
        var preparatoryPeriod = 0.1M;
        var roundedDuration = 0.5M;
        var acceptanceTime = 0.5M;
        var roundingIncluded = true;

        _durationRounder.Setup(x => x.GetRoundedDuration(0.377906976744186046511627907M)).Returns(roundedDuration);
        _durationRounder.Setup(x => x.GetRoundedPreparatoryPeriod(totalDuration)).Returns(preparatoryPeriod);

        var expectedDurationByLC = new DurationByLC(duration, totalLaborCosts, estimateLaborCosts,
            technologicalLaborCosts, workingDayDuration,
            shift, numberOfWorkingDaysInMonth,
            numberOfEmployees, totalDuration, preparatoryPeriod, roundedDuration, acceptanceTime,
            acceptanceTimeIncluded, roundingIncluded);

        var actualDurationByLC = _durationByLCCreator.Create(estimateLaborCosts, technologicalLaborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, acceptanceTimeIncluded);

        Assert.AreEqual(expectedDurationByLC, actualDurationByLC);
        _durationRounder.Verify(x => x.GetRoundedDuration(0.377906976744186046511627907M), Times.Once);
        _durationRounder.Verify(x => x.GetRoundedPreparatoryPeriod(totalDuration), Times.Once);
    }

    [Test]
    public void Create_074Duration_CreateCorrectDurationByLC()
    {
        var estimateLaborCosts = 1524M;
        var technologicalLaborCosts = 0M;
        var totalLaborCosts = 1524M;
        var workingDayDuration = 8M;
        var shift = 1.5M;
        var numberOfWorkingDaysInMonth = 21.5M;
        var numberOfEmployees = 8;
        var acceptanceTimeIncluded = true;
        var duration = 0.74M;
        var totalDuration = 1M;
        var preparatoryPeriod = 0.1M;
        var roundedDuration = 0.5M;
        var acceptanceTime = 0.5M;
        var roundingIncluded = true;

        _durationRounder.Setup(x => x.GetRoundedDuration(0.7383720930232558139534883721M)).Returns(roundedDuration);
        _durationRounder.Setup(x => x.GetRoundedPreparatoryPeriod(totalDuration)).Returns(preparatoryPeriod);

        var expectedDurationByLC = new DurationByLC(duration, totalLaborCosts, estimateLaborCosts,
            technologicalLaborCosts, workingDayDuration,
            shift, numberOfWorkingDaysInMonth,
            numberOfEmployees, totalDuration, preparatoryPeriod, roundedDuration, acceptanceTime,
            acceptanceTimeIncluded, roundingIncluded);

        var actualDurationByLC = _durationByLCCreator.Create(estimateLaborCosts, technologicalLaborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, acceptanceTimeIncluded);

        Assert.AreEqual(expectedDurationByLC, actualDurationByLC);
        _durationRounder.Verify(x => x.GetRoundedDuration(0.7383720930232558139534883721M), Times.Once);
        _durationRounder.Verify(x => x.GetRoundedPreparatoryPeriod(totalDuration), Times.Once);
    }

    [Test]
    public void Create_354DurationWithTechnologicalLaborCosts_CreateCorrectDurationByLC()
    {
        var estimateLaborCosts = 2850.11M;
        var technologicalLaborCosts = 500M;
        var totalLaborCosts = 3350.11M;
        var workingDayDuration = 11;
        var shift = 1;
        var numberOfWorkingDaysInMonth = 21.5M;
        var numberOfEmployees = 4;
        var acceptanceTimeIncluded = false;
        var duration = 3.54M;
        var totalDuration = 3.5M;
        var preparatoryPeriod = 0.3M;
        var roundedDuration = 3.5M;
        var acceptanceTime = 0M;
        var roundingIncluded = true;

        _durationRounder.Setup(x => x.GetRoundedDuration(3.541342494714587737843551797M)).Returns(roundedDuration);
        _durationRounder.Setup(x => x.GetRoundedPreparatoryPeriod(totalDuration)).Returns(preparatoryPeriod);

        var expectedDurationByLC = new DurationByLC(duration, totalLaborCosts, estimateLaborCosts,
            technologicalLaborCosts, workingDayDuration,
            shift, numberOfWorkingDaysInMonth,
            numberOfEmployees, totalDuration, preparatoryPeriod, roundedDuration, acceptanceTime,
            acceptanceTimeIncluded, roundingIncluded);

        var actualDurationByLC = _durationByLCCreator.Create(estimateLaborCosts, technologicalLaborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, acceptanceTimeIncluded);

        Assert.AreEqual(expectedDurationByLC, actualDurationByLC);
        _durationRounder.Verify(x => x.GetRoundedDuration(3.541342494714587737843551797M), Times.Once);
        _durationRounder.Verify(x => x.GetRoundedPreparatoryPeriod(totalDuration), Times.Once);
    }
}