using Calabonga.OperationResults;
using Moq;
using POS.Infrastructure.Calculators.Base;
using POS.Models;
using POS.ViewModels;

namespace POS.Tests.Helpers.Calculators
{
    public static class DurationByLCCalculatorHelper
    {
        public static Mock<IDurationByLCCalculator> GetMock()
        {
            return new Mock<IDurationByLCCalculator>();
        }

        public static Mock<IDurationByLCCalculator> GetMock(DurationByLCViewModel viewModel, DurationByLC durationByLC)
        {
            var durationByLCCalculator = GetMock();

            durationByLCCalculator
                .Setup(x => x.Calculate(viewModel))
                .ReturnsAsync(new OperationResult<DurationByLC> { Result = durationByLC });

            return durationByLCCalculator;
        }
    }
}
