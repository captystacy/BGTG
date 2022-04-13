using Calabonga.OperationResults;
using Moq;
using POS.Infrastructure.Calculators.Base;
using POS.Models.DurationByTCPModels;
using POS.ViewModels;

namespace POS.Tests.Helpers.Calculators
{
    public static class DurationByTCPCalculatorHelper
    {
        public static Mock<IDurationByTCPCalculator> GetMock()
        {
            return new Mock<IDurationByTCPCalculator>();
        }

        public static Mock<IDurationByTCPCalculator> GetMock(DurationByTCPViewModel viewModel, DurationByTCP durationByTCP)
        {
            var durationByTCPCalculator = GetMock();

            durationByTCPCalculator.Setup(x => x.Calculate(viewModel)).ReturnsAsync(new OperationResult<DurationByTCP> { Result = durationByTCP });

            return durationByTCPCalculator;
        }
    }
}
