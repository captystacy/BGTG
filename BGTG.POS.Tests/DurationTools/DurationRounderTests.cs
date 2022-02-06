using BGTG.POS.DurationTools;
using NUnit.Framework;

namespace BGTG.POS.Tests.DurationTools
{
    public class DurationRounderTests
    {
        private DurationRounder _durationRounder;

        [SetUp]
        public void SetUp()
        {
            _durationRounder = new DurationRounder();
        }

        [TestCase(0.04, 0.1)]
        [TestCase(0.14, 0.2)]
        [TestCase(0.24, 0.3)]
        public void GetRoundedDuration_DurationsLessThanQuarter_CorrectDurations(decimal duration, decimal expectedRoundedDuration)
        {
            var actualRoundedDuration = _durationRounder.GetRoundedDuration(duration);

            Assert.AreEqual(expectedRoundedDuration, actualRoundedDuration);
        }

        [TestCase(0.25, 0.5)]
        [TestCase(0.74, 0.5)]
        [TestCase(0.75, 1)]
        [TestCase(1.24, 1)]
        [TestCase(1.25, 1.5)]
        public void GetRoundedDuration_DurationsMoreThanQuarter_CorrectDurations(decimal duration, decimal expectedRoundedDuration)
        {
            var actualRoundedDuration = _durationRounder.GetRoundedDuration(duration);

            Assert.AreEqual(expectedRoundedDuration, actualRoundedDuration);
        }

        [TestCase(0.1, 0.01)]
        [TestCase(1, 0.1)]
        [TestCase(1.1, 0.1)]
        [TestCase(1.5, 0.1)]
        [TestCase(1.9, 0.1)]
        public void GetRoundedPreparatoryPeriod_VariousTotalDurations_CorrectDurations(decimal totalDuration, decimal expectedRoundedPreparatoryPeriod)
        {
            var actualRoundedPreparatoryPeriod = _durationRounder.GetRoundedPreparatoryPeriod(totalDuration);

            Assert.AreEqual(expectedRoundedPreparatoryPeriod, actualRoundedPreparatoryPeriod);
        }
    }
}
