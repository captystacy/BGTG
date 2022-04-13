using Calabonga.OperationResults;
using POS.Infrastructure.Calculators.Base;
using POS.Infrastructure.Engineers;
using POS.Infrastructure.Rounders;
using POS.Models.DurationByTCPModels;
using POS.Models.DurationByTCPModels.TCPModels;
using POS.ViewModels;

namespace POS.Infrastructure.Calculators
{
    public class DurationByTCPCalculator : IDurationByTCPCalculator
    {
        private readonly IDurationByTCPEngineer _durationByTCPEngineer;
        private readonly IDurationRounder _durationRounder;

        private const decimal DurationChangeCoef = 0.3M;

        public DurationByTCPCalculator(IDurationByTCPEngineer durationByTCPEngineer, IDurationRounder durationRounder)
        {
            _durationByTCPEngineer = durationByTCPEngineer;
            _durationRounder = durationRounder;
        }

        public async Task<OperationResult<DurationByTCP>> Calculate(DurationByTCPViewModel viewModel)
        {
            var operation = OperationResult.CreateResult<DurationByTCP>();

            var appendix = _durationByTCPEngineer.GetAppendix(viewModel.AppendixKey);
            var pipelineCharacteristic = _durationByTCPEngineer.GetPipelineCharacteristic(appendix, viewModel.PipelineMaterial, viewModel.PipelineDiameter, viewModel.PipelineCategoryName);

            if (pipelineCharacteristic is null)
            {
                operation.AddError("Pipeline characteristic was not found");
                return operation;
            }

            _durationByTCPEngineer.DefineCalculationType(pipelineCharacteristic.PipelineStandards, viewModel.PipelineLength);

            var durationByTCP = _durationByTCPEngineer.DurationCalculationType switch
            {
                DurationCalculationType.Interpolation => await CalculateInterpolation(viewModel.PipelineLength),
                DurationCalculationType.ExtrapolationAscending => await CalculateExtrapolationAscending(viewModel.PipelineLength),
                DurationCalculationType.ExtrapolationDescending => await CalculateExtrapolationDescending(viewModel.PipelineLength),
                DurationCalculationType.StepwiseExtrapolationAscending => await CalculateExtrapolationStepwiseAscending(viewModel.PipelineLength),
                DurationCalculationType.StepwiseExtrapolationDescending => await CalculateExtrapolationStepwiseDescending(viewModel.PipelineLength),
                _ => throw new ArgumentOutOfRangeException()
            };

            durationByTCP.PipelineMaterial = viewModel.PipelineMaterial;
            durationByTCP.PipelineDiameter = viewModel.PipelineDiameter;
            durationByTCP.PipelineDiameterPresentation = pipelineCharacteristic.DiameterRange.Presentation;
            durationByTCP.DurationCalculationType = _durationByTCPEngineer.DurationCalculationType;
            durationByTCP.CalculationPipelineStandards = _durationByTCPEngineer.CalculationPipelineStandards;
            durationByTCP.PipelineLength = viewModel.PipelineLength;
            durationByTCP.AppendixKey = viewModel.AppendixKey;
            durationByTCP.AppendixPage = appendix.Page;

            operation.Result = durationByTCP;

            return operation;
        }

        private async Task<DurationByTCP> CalculateInterpolation(decimal pipelineLength)
        {
            var calculationPipelineStandards = _durationByTCPEngineer.CalculationPipelineStandards.ToArray();

            var durationChange = decimal.Round(
                (calculationPipelineStandards[1].Duration - calculationPipelineStandards[0].Duration) /
                (calculationPipelineStandards[1].PipelineLength - calculationPipelineStandards[0].PipelineLength), 1);

            var volumeChange = decimal.Round(pipelineLength - calculationPipelineStandards[0].PipelineLength, 1);

            var duration = decimal.Round((calculationPipelineStandards[0].Duration + durationChange * volumeChange), 1);

            var roundedDuration = await _durationRounder.GetRoundedDuration(duration);

            var preparatoryPeriod = calculationPipelineStandards[0].PreparatoryPeriod == 0
                ? await _durationRounder.GetRoundedPreparatoryPeriod(roundedDuration)
                : calculationPipelineStandards[0].PreparatoryPeriod;

            return new InterpolationDurationByTCP
            {
                Duration = duration,
                RoundedDuration = roundedDuration,
                PreparatoryPeriod = preparatoryPeriod,
                PipelineLength = pipelineLength,
                DurationChange = durationChange,
                VolumeChange = volumeChange
            };
        }

        private async Task<DurationByTCP> CalculateExtrapolationAscending(decimal pipelineLength)
        {
            var calculationPipelineStandard = _durationByTCPEngineer.CalculationPipelineStandards.First();

            var volumeChangePercent = decimal.Round(
                (calculationPipelineStandard.PipelineLength - pipelineLength) /
                calculationPipelineStandard.PipelineLength * 100, 1);

            var standardChangePercent = decimal.Round(volumeChangePercent * DurationChangeCoef, 1);

            var duration = decimal.Round(calculationPipelineStandard.Duration * (100 - standardChangePercent) / 100, 1);

            var roundedDuration = await _durationRounder.GetRoundedDuration(duration);

            var preparatoryPeriod = calculationPipelineStandard.PreparatoryPeriod == 0
                ? await _durationRounder.GetRoundedPreparatoryPeriod(roundedDuration)
                : calculationPipelineStandard.PreparatoryPeriod;

            return new ExtrapolationDurationByTCP
            {
                Duration = duration,
                PreparatoryPeriod = preparatoryPeriod,
                RoundedDuration = roundedDuration,
                PipelineLength = pipelineLength,
                StandardChangePercent = standardChangePercent,
                VolumeChangePercent = volumeChangePercent
            };
        }

        private async Task<DurationByTCP> CalculateExtrapolationDescending(decimal pipelineLength)
        {
            var calculationPipelineStandard = _durationByTCPEngineer.CalculationPipelineStandards.First();

            var volumeChangePercent = decimal.Round(
                (pipelineLength - calculationPipelineStandard.PipelineLength) /
                calculationPipelineStandard.PipelineLength * 100, 1);

            var standardChangePercent = decimal.Round(volumeChangePercent * DurationChangeCoef, 1);

            var duration = decimal.Round(calculationPipelineStandard.Duration * (100 + standardChangePercent) / 100, 1);

            var roundedDuration = await _durationRounder.GetRoundedDuration(duration);

            var preparatoryPeriod = calculationPipelineStandard.PreparatoryPeriod == 0
                ? await _durationRounder.GetRoundedPreparatoryPeriod(roundedDuration)
                : calculationPipelineStandard.PreparatoryPeriod;

            return new ExtrapolationDurationByTCP
            {
                Duration = duration,
                PreparatoryPeriod = preparatoryPeriod,
                PipelineLength = pipelineLength,
                RoundedDuration = roundedDuration,
                StandardChangePercent = standardChangePercent,
                VolumeChangePercent = volumeChangePercent,
            };
        }

        private async Task<DurationByTCP> CalculateExtrapolationStepwiseAscending(decimal pipelineLength)
        {
            var calculationPipelineStandard = _durationByTCPEngineer.CalculationPipelineStandards.First();

            var stepwiseDuration = decimal.Round(calculationPipelineStandard.Duration * (100 - 50 * DurationChangeCoef) / 100, 1);

            var stepwisePipelineStandard = new PipelineStandard(calculationPipelineStandard.PipelineLength / 2, stepwiseDuration, 0);

            var volumeChangePercent = decimal.Round(
                (stepwisePipelineStandard.PipelineLength - pipelineLength) /
                stepwisePipelineStandard.PipelineLength * 100, 1);

            var standardChangePercent = decimal.Round(volumeChangePercent * DurationChangeCoef, 1);

            var duration = decimal.Round(stepwisePipelineStandard.Duration * (100 - standardChangePercent) / 100, 1);

            var roundedDuration = await _durationRounder.GetRoundedDuration(duration);

            var preparatoryPeriod = await _durationRounder.GetRoundedPreparatoryPeriod(roundedDuration);

            return new StepwiseExtrapolationDurationByTCP
            {
                Duration = duration,
                PipelineLength = pipelineLength,
                RoundedDuration = roundedDuration,
                PreparatoryPeriod = preparatoryPeriod,
                StepwisePipelineStandard = stepwisePipelineStandard,
                VolumeChangePercent = volumeChangePercent,
                StandardChangePercent = standardChangePercent,
                StepwiseDuration = stepwiseDuration,
            };
        }

        private async Task<DurationByTCP> CalculateExtrapolationStepwiseDescending(decimal pipelineLength)
        {
            var calculationPipelineStandard = _durationByTCPEngineer.CalculationPipelineStandards.First();

            var stepwiseDuration = decimal.Round(calculationPipelineStandard.Duration * (100 + 100 * DurationChangeCoef) / 100, 1);

            var stepwisePipelineStandard = new PipelineStandard(calculationPipelineStandard.PipelineLength * 2, stepwiseDuration, 0);

            var volumeChangePercent = decimal.Round(
                (pipelineLength - stepwisePipelineStandard.PipelineLength) /
                stepwisePipelineStandard.PipelineLength * 100, 1);

            var standardChangePercent = decimal.Round(volumeChangePercent * DurationChangeCoef, 1);

            var duration = decimal.Round(stepwisePipelineStandard.Duration * (100 + standardChangePercent) / 100, 1);

            var roundedDuration = await _durationRounder.GetRoundedDuration(duration);

            var preparatoryPeriod = await _durationRounder.GetRoundedPreparatoryPeriod(roundedDuration);

            return new StepwiseExtrapolationDurationByTCP
            {
                VolumeChangePercent = volumeChangePercent,
                StandardChangePercent = standardChangePercent,
                StepwisePipelineStandard = stepwisePipelineStandard,
                Duration = duration,
                PreparatoryPeriod = preparatoryPeriod,
                PipelineLength = pipelineLength,
                RoundedDuration = roundedDuration,
                StepwiseDuration = stepwiseDuration
            };
        }
    }
}