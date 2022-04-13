using System.Globalization;
using System.Threading.Tasks;
using Calabonga.OperationResults;
using Moq;
using POS.Infrastructure.Calculators;
using POS.Infrastructure.Rounders;
using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;
using POS.Models;
using POS.ViewModels;
using Xunit;

namespace POS.Tests.Infrastructure.Calculators
{
    public class DurationByLCCalculatorTests
    {
        public DurationByLCCalculatorTests()
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
        }

        [Theory]
        [InlineData(43, 0, 8, 1.5, 4, 0.6, 0.06, 0.1, false, true, 0.5, 0.04, "0,0416666666666666666666666667")]
        [InlineData(390, 0, 8, 1.5, 4, 1, 0.1, 0.5, true, true, 0.5, 0.38, "0,377906976744186046511627907")]
        [InlineData(1524, 0, 8, 1.5, 8, 1, 0.1, 0.5, true, true, 0.5, 0.74, "0,7383720930232558139534883721")]
        [InlineData(2850, 500.11, 11, 1, 4, 3.5, 0.3, 3.5, true, false, 0, 3.54, "3,541342494714587737843551797")]
        public async Task ItShould_calculate_correct_duration(int estimateLaborCosts, decimal technologicalLaborCosts, decimal workingDayDuration,
            decimal shift, int numberOfEmployees, decimal totalDuration, decimal preparatoryPeriod, decimal roundedDuration,
            bool roundingIncluded, bool acceptanceTimeIncluded, decimal acceptanceTime, decimal duration, string getRoundedDurationOperationResultStr)
        {
            // arrange

            var totalLaborCosts = estimateLaborCosts + technologicalLaborCosts;
            var numberOfWorkingDays = 21.5M;

            var viewModel = new DurationByLCViewModel
            {
                WorkingDayDuration = workingDayDuration,
                Shift = shift,
                NumberOfWorkingDays = numberOfWorkingDays,
                NumberOfEmployees = numberOfEmployees,
                TechnologicalLaborCosts = technologicalLaborCosts,
                AcceptanceTimeIncluded = acceptanceTimeIncluded,
            };

            var getLaborCostsOperation = new OperationResult<int>
            {
                Result = estimateLaborCosts
            };

            var estimateService = new Mock<IEstimateService>();

            estimateService.Setup(x => x.GetLaborCosts(viewModel.EstimateFiles)).ReturnsAsync(getLaborCostsOperation);

            var getRoundedDurationOperationResult = decimal.Parse(getRoundedDurationOperationResultStr);

            var durationRounder = new Mock<IDurationRounder>();

            durationRounder.Setup(x => x.GetRoundedDuration(getRoundedDurationOperationResult)).ReturnsAsync(roundedDuration);
            durationRounder.Setup(x => x.GetRoundedPreparatoryPeriod(totalDuration)).ReturnsAsync(preparatoryPeriod);

            var expectedDurationByLC = new DurationByLC
            {
                Duration = duration,
                TotalLaborCosts = totalLaborCosts,
                EstimateLaborCosts = estimateLaborCosts,
                TechnologicalLaborCosts = technologicalLaborCosts,
                WorkingDayDuration = workingDayDuration,
                AcceptanceTimeIncluded = acceptanceTimeIncluded,
                RoundedDuration = roundedDuration,
                TotalDuration = totalDuration,
                RoundingIncluded = roundingIncluded,
                PreparatoryPeriod = preparatoryPeriod,
                AcceptanceTime = acceptanceTime,
                NumberOfEmployees = numberOfEmployees,
                NumberOfWorkingDays = numberOfWorkingDays,
                Shift = shift,
            };

            var sut = new DurationByLCCalculator(durationRounder.Object, estimateService.Object);

            // act

            var createOperation = await sut.Calculate(viewModel);

            // assert

            Assert.True(createOperation.Ok);
            estimateService.Verify(x => x.GetLaborCosts(viewModel.EstimateFiles), Times.Once);
            Assert.Equal(expectedDurationByLC, createOperation.Result);
            durationRounder.Verify(x => x.GetRoundedDuration(getRoundedDurationOperationResult), Times.Once);
            durationRounder.Verify(x => x.GetRoundedPreparatoryPeriod(totalDuration), Times.Once);
        }
    }
}