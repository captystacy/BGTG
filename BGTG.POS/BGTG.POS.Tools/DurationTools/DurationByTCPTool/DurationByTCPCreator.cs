using System;
using System.Linq;
using BGTG.POS.Tools.DurationTools.DurationByTCPTool.Interfaces;
using BGTG.POS.Tools.DurationTools.DurationByTCPTool.TCP;
using BGTG.POS.Tools.DurationTools.DurationByTCPTool.TCP.Interfaces;
using BGTG.POS.Tools.DurationTools.Interfaces;

namespace BGTG.POS.Tools.DurationTools.DurationByTCPTool
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

        public DurationByTCP Create(string pipelineMaterial, int pipelineDiameter, decimal pipelineLength, char appendixKey, string pipelineCategoryName)
        {
            var appendix = _tcp212Helper.GetAppendix(appendixKey);
            var pipelineCharacteristic = _tcp212Helper.GetPipelineCharacteristic(appendix, pipelineMaterial, pipelineDiameter, pipelineCategoryName);

            if (pipelineCharacteristic == null)
            {
                return null;
            }

            _durationByTCPEngineer.DefineCalculationType(pipelineCharacteristic.PipelineStandards, pipelineLength);

            return _durationByTCPEngineer.DurationCalculationType switch
            {
                DurationCalculationType.Interpolation => CalculateInterpolation(pipelineMaterial, pipelineDiameter, pipelineCharacteristic.DiameterRange.Presentation, pipelineLength, appendix),
                DurationCalculationType.ExtrapolationAscending => CalculateExtrapolationAscending(pipelineMaterial, pipelineDiameter, pipelineCharacteristic.DiameterRange.Presentation, pipelineLength, appendix),
                DurationCalculationType.ExtrapolationDescending => CalculateExtrapolationDescending(pipelineMaterial, pipelineDiameter, pipelineCharacteristic.DiameterRange.Presentation, pipelineLength, appendix),
                DurationCalculationType.StepwiseExtrapolationAscending => CalculateExtrapolationStepwiseAscending(pipelineMaterial, pipelineDiameter, pipelineCharacteristic.DiameterRange.Presentation, pipelineLength, appendix),
                DurationCalculationType.StepwiseExtrapolationDescending => CalculateExtrapolationStepwiseDescending(pipelineMaterial, pipelineDiameter, pipelineCharacteristic.DiameterRange.Presentation, pipelineLength, appendix),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private InterpolationDurationByTCP CalculateInterpolation(string pipelineMaterial, int pipelineDiameter,
            string pipelineDiameterPresentation, decimal pipelineLength, Appendix appendix)
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
                durationChange, volumeChange, appendix);
        }

        private ExtrapolationDurationByTCP CalculateExtrapolationAscending(string pipelineMaterial,
            int pipelineDiameter, string pipelineDiameterPresentation, decimal pipelineLength, Appendix appendix)
        {
            var calculationPipelineStandard = _durationByTCPEngineer.CalculationPipelineStandards.Single();

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
                , volumeChangePercent, standardChangePercent, appendix);
        }

        private ExtrapolationDurationByTCP CalculateExtrapolationDescending(string pipelineMaterial,
            int pipelineDiameter, string pipelineDiameterPresentation, decimal pipelineLength, Appendix appendix)
        {
            var calculationPipelineStandard = _durationByTCPEngineer.CalculationPipelineStandards.Single();

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
                volumeChangePercent, standardChangePercent, appendix);
        }

        private StepwiseExtrapolationDurationByTCP CalculateExtrapolationStepwiseAscending(string pipelineMaterial,
            int pipelineDiameter, string pipelineDiameterPresentation, decimal pipelineLength, Appendix appendix)
        {
            var calculationPipelineStandard = _durationByTCPEngineer.CalculationPipelineStandards.Single();

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
                stepwiseDuration, stepwisePipelineStandard, appendix);
        }

        private StepwiseExtrapolationDurationByTCP CalculateExtrapolationStepwiseDescending(string pipelineMaterial,
            int pipelineDiameter, string pipelineDiameterPresentation, decimal pipelineLength, Appendix appendix)
        {
            var calculationPipelineStandard = _durationByTCPEngineer.CalculationPipelineStandards.Single();

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
                stepwiseDuration, stepwisePipelineStandard, appendix);
        }
    }
}
