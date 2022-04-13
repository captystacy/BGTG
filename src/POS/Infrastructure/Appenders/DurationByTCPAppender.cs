using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Appenders.Base;
using POS.Infrastructure.Helpers;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Models.DurationByTCPModels;
using System.Globalization;

namespace POS.Infrastructure.Appenders
{
    public class DurationByTCPAppender : IDurationByTCPAppender
    {
        public Task AppendAsync(IMySection section, DurationByTCP durationByTCP)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
            section.ParagraphFormat = Constants.DefaultParagraph;
            section.CharacterFormat = Constants.DefaultFontSize;

            section.AddParagraph("Нормативная продолжительность строительства определена на основании ТКП 45-1.03-122-2015 \"Нормы продолжительности строительства зданий, сооружений и их комплексов. Основные положения\".");

            section.AddParagraph(
                $"Общая нормативная продолжительность строительства сети из {durationByTCP.PipelineMaterial} диаметром {durationByTCP.PipelineDiameterPresentation} мм, определена по ТКП 45-1.03-212-2010 " +
                $"\"Нормы продолжительности строительства инженерных сетей и сооружений\", стр. {durationByTCP.AppendixPage}, приложение {durationByTCP.AppendixKey}, таблица {durationByTCP.AppendixKey}.1. " +
                $"Общая протяженность проектируемой сети из {durationByTCP.PipelineMaterial} – {durationByTCP.PipelineLength} км.");

            section.AddParagraph($"Расчет продолжительности произведен с применением метода {EnumHelper<DurationCalculationType>.GetDisplayName(durationByTCP.DurationCalculationType)} по ТКП (прил. {durationByTCP.AppendixKey}).");

            switch (durationByTCP)
            {
                case InterpolationDurationByTCP interpolationDuration:
                    AddInterpolationParagraphs(section, interpolationDuration);
                    break;
                case ExtrapolationDurationByTCP extrapolationDuration:
                    AddExtrapolationParagraphs(section, extrapolationDuration);
                    break;
            }

            return Task.CompletedTask;
        }

        private void AddExtrapolationParagraphs(IMySection section, ExtrapolationDurationByTCP extrapolationDuration)
        {
            var calculationPipelineStandards = extrapolationDuration.CalculationPipelineStandards.ToList();

            section.AddParagraph($"Нормативная продолжительность строительства сети длиной {calculationPipelineStandards[0].PipelineLength} " +
                                  $"км. составляет {calculationPipelineStandards[0].Duration} мес.");

            section.AddParagraph($"Определяем продолжительность строительства сетей длиной {extrapolationDuration.PipelineLength} км:");

            if (extrapolationDuration is StepwiseExtrapolationDurationByTCP stepwiseExtrapolationDuration)
            {
                section.AddParagraph($"Определяем продолжительность строительства сети длиной {stepwiseExtrapolationDuration.StepwisePipelineStandard.PipelineLength} км:");

                switch (stepwiseExtrapolationDuration.DurationCalculationType)
                {
                    case DurationCalculationType.ExtrapolationAscending:
                    case DurationCalculationType.StepwiseExtrapolationAscending:
                        section.AddParagraph($"{calculationPipelineStandards[0].Duration} ∙ (100 - 100 ∙ 0,3) / 100 = {stepwiseExtrapolationDuration.StepwiseDuration} мес.", Constants.ParagraphHorizontalCentered);
                        break;
                    case DurationCalculationType.ExtrapolationDescending:
                    case DurationCalculationType.StepwiseExtrapolationDescending:
                        section.AddParagraph($"{calculationPipelineStandards[0].Duration} ∙ (100 + 100 ∙ 0,3) / 100 = {stepwiseExtrapolationDuration.StepwiseDuration} мес.", 
                            Constants.ParagraphHorizontalCentered);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                section.AddParagraph($"Нормативная продолжительность строительства сети длиной {stepwiseExtrapolationDuration.StepwisePipelineStandard.PipelineLength} км. составляет {stepwiseExtrapolationDuration.StepwisePipelineStandard.Duration} мес.");
            }

            section.AddParagraph("1. Определяем изменение объема (уменьшение) объема, %:");

            switch (extrapolationDuration.DurationCalculationType)
            {
                case DurationCalculationType.ExtrapolationAscending:
                case DurationCalculationType.StepwiseExtrapolationAscending:
                    section.AddParagraph($"({extrapolationDuration.PipelineLength} - {calculationPipelineStandards[0].PipelineLength}) / {calculationPipelineStandards[0].PipelineLength} ∙ 100 = {extrapolationDuration.VolumeChangePercent} %.");
                    break;
                case DurationCalculationType.ExtrapolationDescending:
                case DurationCalculationType.StepwiseExtrapolationDescending:
                    section.AddParagraph($"({extrapolationDuration.PipelineLength} + {calculationPipelineStandards[0].PipelineLength}) / {calculationPipelineStandards[0].PipelineLength} ∙ 100 = {extrapolationDuration.VolumeChangePercent} %.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            section.AddParagraph("2. Определяем изменение (уменьшение) нормы продолжительности строительства, %:");

            section.AddParagraph($"{extrapolationDuration.VolumeChangePercent} ∙ 0,3 = {extrapolationDuration.StandardChangePercent} %,");

            section.AddParagraph("где 0,3 – коэффициент изменения продолжительности строительства на каждый процент изменения объема.");

            section.AddParagraph("3. Определяем нормативную продолжительность строительства, мес:");

            section.AddParagraph($"{calculationPipelineStandards[0].Duration} ∙ (100 - {extrapolationDuration.StandardChangePercent}) / 100 = {extrapolationDuration.Duration} мес.");

            section.AddParagraph($"4. Нормативная продолжительность строительства сети длиной {extrapolationDuration.PipelineLength} км. составляет {extrapolationDuration.Duration} мес.");

            section.AddParagraph($"Принимаем нормативную продолжительность строительства {extrapolationDuration.RoundedDuration} мес, в т.ч. - {extrapolationDuration.PreparatoryPeriod} мес. - подготовительный период.");

        }

        private void AddInterpolationParagraphs(IMySection section, InterpolationDurationByTCP interpolationDuration)
        {
            var calculationPipelineStandards = interpolationDuration.CalculationPipelineStandards.ToList();

            section.AddParagraph($"Нормативная продолжительность строительства сети длиной {calculationPipelineStandards[0].PipelineLength} " +
                                  $"км. составляет {calculationPipelineStandards[0].Duration} мес.");

            section.AddParagraph($"Нормативная продолжительность строительства сети длиной {calculationPipelineStandards[1].PipelineLength} " +
                                  $"км. составляет {calculationPipelineStandards[1].Duration} мес.");

            section.AddParagraph($"Определяем продолжительность строительства сетей длиной {interpolationDuration.PipelineLength} км:");

            section.AddParagraph("1. Определяем увеличение продолжительности строительства на единицу увеличения объема, мес:");

            section.AddParagraph($"({calculationPipelineStandards[1].Duration} - {calculationPipelineStandards[0].Duration}) / ({calculationPipelineStandards[1].PipelineLength} - {calculationPipelineStandards[0].PipelineLength}) = {interpolationDuration.DurationChange} мес.",
                Constants.ParagraphHorizontalCentered);

            section.AddParagraph("2. Определяем увеличение объема, км:");

            section.AddParagraph($"{interpolationDuration.PipelineLength} - {calculationPipelineStandards[0].PipelineLength} = {interpolationDuration.VolumeChange} км.",
                Constants.ParagraphHorizontalCentered);

            section.AddParagraph("3. Определяем нормативную продолжительность строительства, мес:");

            section.AddParagraph($"{calculationPipelineStandards[0].Duration} + {interpolationDuration.DurationChange} ∙ {interpolationDuration.VolumeChange} = {interpolationDuration.Duration} мес.", Constants.ParagraphHorizontalCentered);

            section.AddParagraph($"4. Нормативная продолжительность строительства сети длиной {interpolationDuration.PipelineLength} км. составляет {interpolationDuration.Duration} мес.");

            section.AddParagraph($"Принимаем нормативную продолжительность строительства {interpolationDuration.RoundedDuration} мес, в т.ч. - {interpolationDuration.PreparatoryPeriod} мес. - подготовительный период.");
        }
    }
}