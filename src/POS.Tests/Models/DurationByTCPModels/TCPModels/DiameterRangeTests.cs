using POS.Models.DurationByTCPModels.TCPModels;
using Xunit;

namespace POS.Tests.Models.DurationByTCPModels.TCPModels
{
    public class DiameterRangeTests
    {
        private DiameterRange _sut = null!;

        [Theory]
        [InlineData(0, true)]
        [InlineData(200, true)]
        [InlineData(499, true)]
        [InlineData(500, false)]
        public void ItShould_return_correct_answers(int value, bool expectedIsInRange)
        {
            _sut = new DiameterRange(0, 500, "до 500");

            var actualIsInRange = _sut.IsInRange(value);

            Assert.Equal(expectedIsInRange, actualIsInRange);
        }
    }
}
