using POS.Infrastructure.Rounders;
using Xunit;

namespace POS.Tests.Infrastructure.Rounders
{
    public class DurationRounderTests
    {
        [Theory]
        [InlineData(0.04, 0.1)]
        [InlineData(0.14, 0.2)]
        [InlineData(0.24, 0.3)]
        public void ItShould_get_correct_rounded_duration_when_duration_is_less_than_quarter(decimal duration, decimal expectedRoundedDuration)
        {
            // arrange

            var sut = new DurationRounder();

            // act

            var getRoundedDurationOperation = sut.GetRoundedDuration(duration);

            // assert

            Assert.Equal(expectedRoundedDuration, getRoundedDurationOperation.Result);
        }

        [Theory]
        [InlineData(0.25, 0.5)]
        [InlineData(0.74, 0.5)]
        [InlineData(0.75, 1)]
        [InlineData(1.24, 1)]
        [InlineData(1.25, 1.5)]
        public void ItShould_get_correct_rounded_duration_when_duration_is_more_than_quarter(decimal duration, decimal expectedRoundedDuration)
        {
            // arrange

            var sut = new DurationRounder();

            // act

            var getRoundedDurationOperation = sut.GetRoundedDuration(duration);

            // assert

            Assert.Equal(expectedRoundedDuration, getRoundedDurationOperation.Result);
        }

        [Theory]
        [InlineData(0.1, 0.01)]
        [InlineData(1, 0.1)]
        [InlineData(1.1, 0.1)]
        [InlineData(1.5, 0.1)]
        [InlineData(1.9, 0.1)]
        public void ItShould_get_correct_rounded_preparatory_period(decimal totalDuration, decimal expectedRoundedPreparatoryPeriod)
        {
            // arrange

            var sut = new DurationRounder();

            // act

            var actualRoundedPreparatoryPeriod = sut.GetRoundedPreparatoryPeriod(totalDuration);

            // assert

            Assert.Equal(expectedRoundedPreparatoryPeriod, actualRoundedPreparatoryPeriod.Result);
        }
    }
}