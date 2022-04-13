using Calabonga.OperationResults;
using POS.Infrastructure.Calculators.Base;
using POS.Infrastructure.Rounders;
using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;
using POS.Models;
using POS.ViewModels;

namespace POS.Infrastructure.Calculators
{
    public class DurationByLCCalculator : IDurationByLCCalculator
    {
        private readonly IDurationRounder _durationRounder;
        private readonly IEstimateService _estimateService;

        private const decimal HalfMonthAcceptanceTime = 0.5M;
        private const int OneMonthAcceptanceTime = 1;

        public DurationByLCCalculator(IDurationRounder durationRounder, IEstimateService estimateService)
        {
            _durationRounder = durationRounder;
            _estimateService = estimateService;
        }

        public async Task<OperationResult<DurationByLC>> Calculate(DurationByLCViewModel viewModel)
        {
            var operation = OperationResult.CreateResult<DurationByLC>();

            var getLaborCostsOperation = await _estimateService.GetLaborCosts(viewModel.EstimateFiles);

            if (!getLaborCostsOperation.Ok)
            {
                operation.AddError(getLaborCostsOperation.GetMetadataMessages());
                return operation;
            }

            var estimateLaborCosts = getLaborCostsOperation.Result;

            var totalLaborCosts = estimateLaborCosts + viewModel.TechnologicalLaborCosts;

            var duration = totalLaborCosts / viewModel.WorkingDayDuration / viewModel.Shift / viewModel.NumberOfWorkingDays / viewModel.NumberOfEmployees;

            decimal roundedDuration;
            decimal totalDuration;
            decimal acceptanceTime;
            bool roundingIncluded;

            if (duration < 1)
            {
                acceptanceTime = viewModel.AcceptanceTimeIncluded ? HalfMonthAcceptanceTime : 0;
                if (duration < 0.25M)
                {
                    roundingIncluded = false;
                    roundedDuration = await _durationRounder.GetRoundedDuration(duration);
                    totalDuration = viewModel.AcceptanceTimeIncluded
                        ? roundedDuration + HalfMonthAcceptanceTime
                        : roundedDuration;
                }
                else
                {
                    roundingIncluded = true;
                    roundedDuration = await _durationRounder.GetRoundedDuration(duration);
                    totalDuration = viewModel.AcceptanceTimeIncluded
                        ? roundedDuration + HalfMonthAcceptanceTime
                        : roundedDuration;
                }
            }
            else
            {
                acceptanceTime = viewModel.AcceptanceTimeIncluded ? OneMonthAcceptanceTime : 0;
                roundingIncluded = true;
                roundedDuration = await _durationRounder.GetRoundedDuration(duration);
                totalDuration = viewModel.AcceptanceTimeIncluded
                    ? roundedDuration + OneMonthAcceptanceTime
                    : roundedDuration;
            }

            var preparatoryPeriod = await _durationRounder.GetRoundedPreparatoryPeriod(totalDuration);

            operation.Result = new DurationByLC
            {
                Duration = decimal.Round(duration, 2),
                TotalLaborCosts = totalLaborCosts,
                EstimateLaborCosts = estimateLaborCosts,
                TechnologicalLaborCosts = viewModel.TechnologicalLaborCosts,
                WorkingDayDuration = viewModel.WorkingDayDuration,
                Shift = viewModel.Shift,
                NumberOfWorkingDays = viewModel.NumberOfWorkingDays,
                NumberOfEmployees = viewModel.NumberOfEmployees,
                TotalDuration = totalDuration,
                PreparatoryPeriod = preparatoryPeriod,
                RoundedDuration = roundedDuration,
                AcceptanceTime = acceptanceTime,
                AcceptanceTimeIncluded = viewModel.AcceptanceTimeIncluded,
                RoundingIncluded = roundingIncluded
            };

            return operation;
        }
    }
}