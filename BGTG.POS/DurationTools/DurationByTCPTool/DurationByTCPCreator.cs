using System;
using System.Linq;
using BGTG.POS.DurationTools.Base;
using BGTG.POS.DurationTools.DurationByTCPTool.Base;
using BGTG.POS.DurationTools.DurationByTCPTool.TCP;
using BGTG.POS.DurationTools.DurationByTCPTool.TCP.Interfaces;

namespace BGTG.POS.DurationTools.DurationByTCPTool
{
    public class DurationByTCPCreator : IDurationByTCPCreator
    {
        private readonly IDurationByTCPEngineer _durationByTCPEngineer;
        private readonly ITCP212Helper _tcp212Helper;
        private readonly IDurationRounder _durationRounder;

        private const decimal DurationChangeCoef = 0.3M;

        public DurationByTCPCreator(IDurationByTCPEngineer durationByTCPEngineer, ITCP212Helper tcp212Helper, IDurationRounder durationRounder)
        {
            _durationByTCPEngineer = durationByTCPEngineer;
            _tcp212Helper = tcp212Helper;
            _durationRounder = durationRounder;
        }

        public DurationByTCP? Create(string pipelineMaterial, int pipelineDiameter, decimal pipelineLength, char appendixKey, string pipelineCategoryName)
        {
            var appendix = _tcp212Helper.GetAppendix(appendixKey);
            var pipelineCharacteristic = _tcp212Helper.GetPipelineCharacteristic(appendix, pipelineMaterial, pipelineDiameter, pipelineCategoryName);

            if (pipelineCharacteristic is null)
            {
                return null;
            }

            _durationByTCPEngineer.DefineCalculationType(pipelineCharacteristic.PipelineStandards, pipelineLength);

            var appendixPage = appendix.Page;
            return _durationByTCPEngineer.DurationCalculationType switch
            {
                DurationCalculationType.Interpolation => CalculateInterpolation(pipelineMaterial, pipelineDiameter, pipelineCharacteristic.DiameterRange.Presentation, pipelineLength, appendixKey, appendixPage),
                DurationCalculationType.ExtrapolationAscending => CalculateExtrapolationAscending(pipelineMaterial, pipelineDiameter, pipelineCharacteristic.DiameterRange.Presentation, pipelineLength, appendixKey, appendixPage),
                DurationCalculationType.ExtrapolationDescending => CalculateExtrapolationDescending(pipelineMaterial, pipelineDiameter, pipelineCharacteristic.DiameterRange.Presentation, pipelineLength, appendixKey, appendixPage),
                DurationCalculationType.StepwiseExtrapolationAscending => CalculateExtrapolationStepwiseAscending(pipelineMaterial, pipelineDiameter, pipelineCharacteristic.DiameterRange.Presentation, pipelineLength, appendixKey, appendixPage),
                DurationCalculationType.StepwiseExtrapolationDescending => CalculateExtrapolationStepwiseDescending(pipelineMaterial, pipelineDiameter, pipelineCharacteristic.DiameterRange.Presentation, pipelineLength, appendixKey, appendixPage),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private InterpolationDurationByTCP CalculateInterpolation(string pipelineMaterial, int pipelineDiameter,
            string pipelineDiameterPresentation, decimal pipelineLength, char appendixKey, int appendixPage)
        {
            var calculationPipelineStandards = _durationByTCPEngineer.CalculationPipelineStandards.ToArray();

            var durationChange = decimal.Round(
                (calculationPipelineStandards[1].Duration - calculationPipelineStandards[0].Duration) /
                (calculationPipelineStandards[1].PipelineLength - calculationPipelineStandards[0].PipelineLength), 1);

            var volumeChange = decimal.Round(pipelineLength - calculationPipelineStandards[0].PipelineLength, 1);

            var duration = decimal.Round((calculationPipelineStandards[0].Duration + durationChange * volumeChange), 1);

            var roundedDuration = _durationRounder.GetRoundedDuration(duration);

            var preparatoryPeriod = calculationPipelineStandards[0].PreparatoryPeriod == 0
                ? _durationRounder.GetRoundedPreparatoryPeriod(roundedDuration)
                : calculationPipelineStandards[0].PreparatoryPeriod;

            return new InterpolationDurationByTCP(pipelineMaterial, pipelineDiameter, pipelineDiameterPresentation,
                pipelineLength,
                _durationByTCPEngineer.CalculationPipelineStandards,
                _durationByTCPEngineer.DurationCalculationType, duration, roundedDuration, preparatoryPeriod,
                durationChange, volumeChange, appendixKey, appendixPage);
        }

        private ExtrapolationDurationByTCP CalculateExtrapolationAscending(string pipelineMaterial,
            int pipelineDiameter, string pipelineDiameterPresentation, decimal pipelineLength, char appendixKey, int appendixPage)
        {
            var calculationPipelineStandard = _durationByTCPEngineer.CalculationPipelineStandards.First();

            var volumeChangePercent = decimal.Round(
                (calculationPipelineStandard.PipelineLength - pipelineLength) /
                calculationPipelineStandard.PipelineLength * 100, 1);

            var standardChangePercent = decimal.Round(volumeChangePercent * DurationChangeCoef, 1);

            var duration = decimal.Round(calculationPipelineStandard.Duration * (100 - standardChangePercent) / 100, 1);

            var roundedDuration = _durationRounder.GetRoundedDuration(duration);

            var preparatoryPeriod = calculationPipelineStandard.PreparatoryPeriod == 0
                ? _durationRounder.GetRoundedPreparatoryPeriod(roundedDuration)
                : calculationPipelineStandard.PreparatoryPeriod;

            return new ExtrapolationDurationByTCP(pipelineMaterial, pipelineDiameter, pipelineDiameterPresentation,
                pipelineLength,
                _durationByTCPEngineer.CalculationPipelineStandards,
                _durationByTCPEngineer.DurationCalculationType, duration, roundedDuration, preparatoryPeriod
                , volumeChangePercent, standardChangePercent, appendixKey, appendixPage);
        }

        private ExtrapolationDurationByTCP CalculateExtrapolationDescending(string pipelineMaterial,
            int pipelineDiameter, string pipelineDiameterPresentation, decimal pipelineLength, char appendixKey, int appendixPage)
        {
            var calculationPipelineStandard = _durationByTCPEngineer.CalculationPipelineStandards.First();

            var volumeChangePercent = decimal.Round(
                (pipelineLength - calculationPipelineStandard.PipelineLength) /
                calculationPipelineStandard.PipelineLength * 100, 1);

            var standardChangePercent = decimal.Round(volumeChangePercent * DurationChangeCoef, 1);

            var duration = decimal.Round(calculationPipelineStandard.Duration * (100 + standardChangePercent) / 100, 1);

            var roundedDuration = _durationRounder.GetRoundedDuration(duration);

            var preparatoryPeriod = calculationPipelineStandard.PreparatoryPeriod == 0
                ? _durationRounder.GetRoundedPreparatoryPeriod(roundedDuration)
                : calculationPipelineStandard.PreparatoryPeriod;

            return new ExtrapolationDurationByTCP(pipelineMaterial, pipelineDiameter, pipelineDiameterPresentation,
                pipelineLength,
                _durationByTCPEngineer.CalculationPipelineStandards,
                _durationByTCPEngineer.DurationCalculationType, duration, roundedDuration, preparatoryPeriod,
                volumeChangePercent, standardChangePercent, appendixKey, appendixPage);
        }

        private StepwiseExtrapolationDurationByTCP CalculateExtrapolationStepwiseAscending(string pipelineMaterial,
            int pipelineDiameter, string pipelineDiameterPresentation, decimal pipelineLength, char appendixKey, int appendixPage)
        {
            var calculationPipelineStandard = _durationByTCPEngineer.CalculationPipelineStandards.First();

            var stepwiseDuration = decimal.Round(calculationPipelineStandard.Duration * (100 - 50 * DurationChangeCoef) / 100, 1);

            var stepwisePipelineStandard = new PipelineStandard(calculationPipelineStandard.PipelineLength / 2, stepwiseDuration, 0);

            var volumeChangePercent = decimal.Round(
                (stepwisePipelineStandard.PipelineLength - pipelineLength) /
                stepwisePipelineStandard.PipelineLength * 100, 1);

            var standardChangePercent = decimal.Round(volumeChangePercent * DurationChangeCoef, 1);

            var duration = decimal.Round(stepwisePipelineStandard.Duration * (100 - standardChangePercent) / 100, 1);

            var roundedDuration = _durationRounder.GetRoundedDuration(duration);

            var preparatoryPeriod = _durationRounder.GetRoundedPreparatoryPeriod(roundedDuration);

            return new StepwiseExtrapolationDurationByTCP(pipelineMaterial, pipelineDiameter, pipelineDiameterPresentation,
                pipelineLength,
                _durationByTCPEngineer.CalculationPipelineStandards,
                _durationByTCPEngineer.DurationCalculationType, duration, roundedDuration, preparatoryPeriod
                , volumeChangePercent, standardChangePercent,
                stepwiseDuration, stepwisePipelineStandard, appendixKey, appendixPage);
        }

        private StepwiseExtrapolationDurationByTCP CalculateExtrapolationStepwiseDescending(string pipelineMaterial,
            int pipelineDiameter, string pipelineDiameterPresentation, decimal pipelineLength, char appendixKey, int appendixPage)
        {
            var calculationPipelineStandard = _durationByTCPEngineer.CalculationPipelineStandards.First();

            var stepwiseDuration = decimal.Round(calculationPipelineStandard.Duration * (100 + 100 * DurationChangeCoef) / 100, 1);

            var stepwisePipelineStandard = new PipelineStandard(calculationPipelineStandard.PipelineLength * 2, stepwiseDuration, 0);

            var volumeChangePercent = decimal.Round(
                (pipelineLength - stepwisePipelineStandard.PipelineLength) /
                stepwisePipelineStandard.PipelineLength * 100, 1);

            var standardChangePercent = decimal.Round(volumeChangePercent * DurationChangeCoef, 1);

            var duration = decimal.Round(stepwisePipelineStandard.Duration * (100 + standardChangePercent) / 100, 1);

            var roundedDuration = _durationRounder.GetRoundedDuration(duration);

            var preparatoryPeriod = _durationRounder.GetRoundedPreparatoryPeriod(roundedDuration);

            return new StepwiseExtrapolationDurationByTCP(pipelineMaterial, pipelineDiameter, pipelineDiameterPresentation,
                pipelineLength,
                _durationByTCPEngineer.CalculationPipelineStandards,
                _durationByTCPEngineer.DurationCalculationType, duration, roundedDuration, preparatoryPeriod,
                volumeChangePercent, standardChangePercent,
                stepwiseDuration, stepwisePipelineStandard, appendixKey, appendixPage);
        }
    }
}
