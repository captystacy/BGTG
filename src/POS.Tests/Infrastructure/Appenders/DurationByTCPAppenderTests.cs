using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Appenders;
using POS.Infrastructure.Helpers;
using POS.Models.DurationByTCPModels;
using POS.Tests.Helpers.Services.DocumentServices.WordService;
using Xunit;

namespace POS.Tests.Infrastructure.Appenders
{
    public class DurationByTCPAppenderTests
    {
        [Fact]
        public async Task ItShould_add_correct_interpolation_paragraphs()
        {
            // arrange

            var fixture = new Fixture();

            var interpolationDuration = fixture.Create<InterpolationDurationByTCP>();

            var calculationPipelineStandards = interpolationDuration.CalculationPipelineStandards.ToList();

            var section = MySectionHelper.GetMock();

            var sut = new DurationByTCPAppender();

            // act

            await sut.AppendAsync(section.Object, interpolationDuration);

            // assert

            section.Verify(x => x.AddParagraph(
                $"Общая нормативная продолжительность строительства сети из {interpolationDuration.PipelineMaterial} диаметром {interpolationDuration.PipelineDiameterPresentation} мм, определена по ТКП 45-1.03-212-2010 " +
                $"\"Нормы продолжительности строительства инженерных сетей и сооружений\", стр. {interpolationDuration.AppendixPage}, приложение {interpolationDuration.AppendixKey}, таблица {interpolationDuration.AppendixKey}.1. " +
                $"Общая протяженность проектируемой сети из {interpolationDuration.PipelineMaterial} – {interpolationDuration.PipelineLength} км."), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"Расчет продолжительности произведен с применением метода {EnumHelper<DurationCalculationType>.GetDisplayName(interpolationDuration.DurationCalculationType)} по ТКП (прил. {interpolationDuration.AppendixKey})."), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"Нормативная продолжительность строительства сети длиной {calculationPipelineStandards[0].PipelineLength} " +
                $"км. составляет {calculationPipelineStandards[0].Duration} мес."), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"Нормативная продолжительность строительства сети длиной {calculationPipelineStandards[1].PipelineLength} " +
                $"км. составляет {calculationPipelineStandards[1].Duration} мес."), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"({calculationPipelineStandards[1].Duration} - {calculationPipelineStandards[0].Duration}) / ({calculationPipelineStandards[1].PipelineLength} - {calculationPipelineStandards[0].PipelineLength}) = {interpolationDuration.DurationChange} мес.",
                Constants.ParagraphHorizontalCentered), Times.Once);

            section.Verify(x => x.AddParagraph(
                "2. Определяем увеличение объема, км:"), Times.Once);

            section.Verify(x => x.AddParagraph($"{interpolationDuration.PipelineLength} - {calculationPipelineStandards[0].PipelineLength} = {interpolationDuration.VolumeChange} км.",
                Constants.ParagraphHorizontalCentered), Times.Once);

            section.Verify(x => x.AddParagraph("3. Определяем нормативную продолжительность строительства, мес:"), Times.Once);

            section.Verify(x => x.AddParagraph($"{calculationPipelineStandards[0].Duration} + {interpolationDuration.DurationChange} ∙ {interpolationDuration.VolumeChange} = {interpolationDuration.Duration} мес.",
                Constants.ParagraphHorizontalCentered), Times.Once);

            section.Verify(x => x.AddParagraph($"4. Нормативная продолжительность строительства сети длиной {interpolationDuration.PipelineLength} км. составляет {interpolationDuration.Duration} мес."), Times.Once);

            section.Verify(x => x.AddParagraph($"Принимаем нормативную продолжительность строительства {interpolationDuration.RoundedDuration} мес, в т.ч. - {interpolationDuration.PreparatoryPeriod} мес. - подготовительный период."), Times.Once);
        }

        [Fact]
        public async Task ItShould_add_correct_extrapolation_ascending_paragraphs()
        {
            // arrange

            var fixture = new Fixture();

            var extrapolationDuration = fixture
                .Build<ExtrapolationDurationByTCP>()
                .With(x => x.DurationCalculationType, DurationCalculationType.ExtrapolationAscending)
                .Create();

            var calculationPipelineStandards = extrapolationDuration.CalculationPipelineStandards.ToList();

            var section = MySectionHelper.GetMock();

            var sut = new DurationByTCPAppender();

            // act

            await sut.AppendAsync(section.Object, extrapolationDuration);

            // assert

            section.Verify(x => x.AddParagraph(
                $"Общая нормативная продолжительность строительства сети из {extrapolationDuration.PipelineMaterial} диаметром {extrapolationDuration.PipelineDiameterPresentation} мм, определена по ТКП 45-1.03-212-2010 " +
                $"\"Нормы продолжительности строительства инженерных сетей и сооружений\", стр. {extrapolationDuration.AppendixPage}, приложение {extrapolationDuration.AppendixKey}, таблица {extrapolationDuration.AppendixKey}.1. " +
                $"Общая протяженность проектируемой сети из {extrapolationDuration.PipelineMaterial} – {extrapolationDuration.PipelineLength} км."), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"Расчет продолжительности произведен с применением метода {EnumHelper<DurationCalculationType>.GetDisplayName(extrapolationDuration.DurationCalculationType)} по ТКП (прил. {extrapolationDuration.AppendixKey})."), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"Нормативная продолжительность строительства сети длиной {calculationPipelineStandards[0].PipelineLength} " +
                $"км. составляет {calculationPipelineStandards[0].Duration} мес."), Times.Once);

            section.Verify(x => x.AddParagraph($"Определяем продолжительность строительства сетей длиной {extrapolationDuration.PipelineLength} км:"), Times.Once);

            section.Verify(x => x.AddParagraph(
                "1. Определяем изменение объема (уменьшение) объема, %:"), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"({extrapolationDuration.PipelineLength} - {calculationPipelineStandards[0].PipelineLength}) / {calculationPipelineStandards[0].PipelineLength} ∙ 100 = {extrapolationDuration.VolumeChangePercent} %."), Times.Once);

            section.Verify(x => x.AddParagraph(
                "2. Определяем изменение (уменьшение) нормы продолжительности строительства, %:"), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"{extrapolationDuration.VolumeChangePercent} ∙ 0,3 = {extrapolationDuration.StandardChangePercent} %,"), Times.Once);

            section.Verify(x => x.AddParagraph(
                "где 0,3 – коэффициент изменения продолжительности строительства на каждый процент изменения объема."), Times.Once);

            section.Verify(x => x.AddParagraph(
                "3. Определяем нормативную продолжительность строительства, мес:"), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"{calculationPipelineStandards[0].Duration} ∙ (100 - {extrapolationDuration.StandardChangePercent}) / 100 = {extrapolationDuration.Duration} мес."), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"4. Нормативная продолжительность строительства сети длиной {extrapolationDuration.PipelineLength} км. составляет {extrapolationDuration.Duration} мес."), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"Принимаем нормативную продолжительность строительства {extrapolationDuration.RoundedDuration} мес, в т.ч. - {extrapolationDuration.PreparatoryPeriod} мес. - подготовительный период."), Times.Once);
        }

        [Fact]
        public async Task ItShould_add_correct_extrapolation_descending_paragraphs()
        {
            // arrange

            var fixture = new Fixture();

            var extrapolationDuration = fixture
                .Build<ExtrapolationDurationByTCP>()
                .With(x => x.DurationCalculationType, DurationCalculationType.ExtrapolationDescending)
                .Create();

            var calculationPipelineStandards = extrapolationDuration.CalculationPipelineStandards.ToList();

            var section = MySectionHelper.GetMock();

            var sut = new DurationByTCPAppender();

            // act

            await sut.AppendAsync(section.Object, extrapolationDuration);

            // assert

            section.Verify(x => x.AddParagraph(
                $"Общая нормативная продолжительность строительства сети из {extrapolationDuration.PipelineMaterial} диаметром {extrapolationDuration.PipelineDiameterPresentation} мм, определена по ТКП 45-1.03-212-2010 " +
                $"\"Нормы продолжительности строительства инженерных сетей и сооружений\", стр. {extrapolationDuration.AppendixPage}, приложение {extrapolationDuration.AppendixKey}, таблица {extrapolationDuration.AppendixKey}.1. " +
                $"Общая протяженность проектируемой сети из {extrapolationDuration.PipelineMaterial} – {extrapolationDuration.PipelineLength} км."), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"Расчет продолжительности произведен с применением метода {EnumHelper<DurationCalculationType>.GetDisplayName(extrapolationDuration.DurationCalculationType)} по ТКП (прил. {extrapolationDuration.AppendixKey})."), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"Нормативная продолжительность строительства сети длиной {calculationPipelineStandards[0].PipelineLength} " +
                $"км. составляет {calculationPipelineStandards[0].Duration} мес."), Times.Once);

            section.Verify(x => x.AddParagraph($"Определяем продолжительность строительства сетей длиной {extrapolationDuration.PipelineLength} км:"), Times.Once);

            section.Verify(x => x.AddParagraph(
                "1. Определяем изменение объема (уменьшение) объема, %:"), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"({extrapolationDuration.PipelineLength} + {calculationPipelineStandards[0].PipelineLength}) / {calculationPipelineStandards[0].PipelineLength} ∙ 100 = {extrapolationDuration.VolumeChangePercent} %."), Times.Once);

            section.Verify(x => x.AddParagraph(
                "2. Определяем изменение (уменьшение) нормы продолжительности строительства, %:"), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"{extrapolationDuration.VolumeChangePercent} ∙ 0,3 = {extrapolationDuration.StandardChangePercent} %,"), Times.Once);

            section.Verify(x => x.AddParagraph(
                "где 0,3 – коэффициент изменения продолжительности строительства на каждый процент изменения объема."), Times.Once);

            section.Verify(x => x.AddParagraph(
                "3. Определяем нормативную продолжительность строительства, мес:"), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"{calculationPipelineStandards[0].Duration} ∙ (100 - {extrapolationDuration.StandardChangePercent}) / 100 = {extrapolationDuration.Duration} мес."), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"4. Нормативная продолжительность строительства сети длиной {extrapolationDuration.PipelineLength} км. составляет {extrapolationDuration.Duration} мес."), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"Принимаем нормативную продолжительность строительства {extrapolationDuration.RoundedDuration} мес, в т.ч. - {extrapolationDuration.PreparatoryPeriod} мес. - подготовительный период."), Times.Once);
        }

        [Fact]
        public async Task ItShould_add_correct_stepwise_extrapolation_ascending_paragraphs()
        {
            // arrange

            var fixture = new Fixture();

            var stepwiseExtrapolationDuration = fixture
                .Build<StepwiseExtrapolationDurationByTCP>()
                .With(x => x.DurationCalculationType, DurationCalculationType.StepwiseExtrapolationAscending)
                .Create();

            var calculationPipelineStandards = stepwiseExtrapolationDuration.CalculationPipelineStandards.ToList();

            var section = MySectionHelper.GetMock();

            var sut = new DurationByTCPAppender();

            // act

            await sut.AppendAsync(section.Object, stepwiseExtrapolationDuration);

            // assert

            section.Verify(x => x.AddParagraph(
                $"Общая нормативная продолжительность строительства сети из {stepwiseExtrapolationDuration.PipelineMaterial} диаметром {stepwiseExtrapolationDuration.PipelineDiameterPresentation} мм, определена по ТКП 45-1.03-212-2010 " +
                $"\"Нормы продолжительности строительства инженерных сетей и сооружений\", стр. {stepwiseExtrapolationDuration.AppendixPage}, приложение {stepwiseExtrapolationDuration.AppendixKey}, таблица {stepwiseExtrapolationDuration.AppendixKey}.1. " +
                $"Общая протяженность проектируемой сети из {stepwiseExtrapolationDuration.PipelineMaterial} – {stepwiseExtrapolationDuration.PipelineLength} км."), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"Расчет продолжительности произведен с применением метода {EnumHelper<DurationCalculationType>.GetDisplayName(stepwiseExtrapolationDuration.DurationCalculationType)} по ТКП (прил. {stepwiseExtrapolationDuration.AppendixKey})."), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"Нормативная продолжительность строительства сети длиной {calculationPipelineStandards[0].PipelineLength} " +
                $"км. составляет {calculationPipelineStandards[0].Duration} мес."), Times.Once);

            section.Verify(x => x.AddParagraph($"Определяем продолжительность строительства сетей длиной {stepwiseExtrapolationDuration.PipelineLength} км:"), Times.Once);

            section.Verify(x => x.AddParagraph($"Определяем продолжительность строительства сети длиной {stepwiseExtrapolationDuration.StepwisePipelineStandard.PipelineLength} км:"), Times.Once);

            section.Verify(x => x.AddParagraph($"{calculationPipelineStandards[0].Duration} ∙ (100 - 100 ∙ 0,3) / 100 = {stepwiseExtrapolationDuration.StepwiseDuration} мес.", Constants.ParagraphHorizontalCentered), Times.Once);

            section.Verify(x => x.AddParagraph(
                "1. Определяем изменение объема (уменьшение) объема, %:"), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"({stepwiseExtrapolationDuration.PipelineLength} - {calculationPipelineStandards[0].PipelineLength}) / {calculationPipelineStandards[0].PipelineLength} ∙ 100 = {stepwiseExtrapolationDuration.VolumeChangePercent} %."), Times.Once);

            section.Verify(x => x.AddParagraph(
                "2. Определяем изменение (уменьшение) нормы продолжительности строительства, %:"), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"{stepwiseExtrapolationDuration.VolumeChangePercent} ∙ 0,3 = {stepwiseExtrapolationDuration.StandardChangePercent} %,"), Times.Once);

            section.Verify(x => x.AddParagraph(
                "где 0,3 – коэффициент изменения продолжительности строительства на каждый процент изменения объема."), Times.Once);

            section.Verify(x => x.AddParagraph(
                "3. Определяем нормативную продолжительность строительства, мес:"), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"{calculationPipelineStandards[0].Duration} ∙ (100 - {stepwiseExtrapolationDuration.StandardChangePercent}) / 100 = {stepwiseExtrapolationDuration.Duration} мес."), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"4. Нормативная продолжительность строительства сети длиной {stepwiseExtrapolationDuration.PipelineLength} км. составляет {stepwiseExtrapolationDuration.Duration} мес."), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"Принимаем нормативную продолжительность строительства {stepwiseExtrapolationDuration.RoundedDuration} мес, в т.ч. - {stepwiseExtrapolationDuration.PreparatoryPeriod} мес. - подготовительный период."), Times.Once);
        }

        [Fact]
        public async Task ItShould_add_correct_stepwise_extrapolation_descending_paragraphs()
        {
            // arrange

            var fixture = new Fixture();

            var stepwiseExtrapolationDuration = fixture
                .Build<StepwiseExtrapolationDurationByTCP>()
                .With(x => x.DurationCalculationType, DurationCalculationType.StepwiseExtrapolationDescending)
                .Create();

            var calculationPipelineStandards = stepwiseExtrapolationDuration.CalculationPipelineStandards.ToList();

            var section = MySectionHelper.GetMock();

            var sut = new DurationByTCPAppender();

            // act

            await sut.AppendAsync(section.Object, stepwiseExtrapolationDuration);

            // assert

            section.Verify(x => x.AddParagraph(
                $"Общая нормативная продолжительность строительства сети из {stepwiseExtrapolationDuration.PipelineMaterial} диаметром {stepwiseExtrapolationDuration.PipelineDiameterPresentation} мм, определена по ТКП 45-1.03-212-2010 " +
                $"\"Нормы продолжительности строительства инженерных сетей и сооружений\", стр. {stepwiseExtrapolationDuration.AppendixPage}, приложение {stepwiseExtrapolationDuration.AppendixKey}, таблица {stepwiseExtrapolationDuration.AppendixKey}.1. " +
                $"Общая протяженность проектируемой сети из {stepwiseExtrapolationDuration.PipelineMaterial} – {stepwiseExtrapolationDuration.PipelineLength} км."), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"Расчет продолжительности произведен с применением метода {EnumHelper<DurationCalculationType>.GetDisplayName(stepwiseExtrapolationDuration.DurationCalculationType)} по ТКП (прил. {stepwiseExtrapolationDuration.AppendixKey})."), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"Нормативная продолжительность строительства сети длиной {calculationPipelineStandards[0].PipelineLength} " +
                $"км. составляет {calculationPipelineStandards[0].Duration} мес."), Times.Once);

            section.Verify(x => x.AddParagraph($"Определяем продолжительность строительства сетей длиной {stepwiseExtrapolationDuration.PipelineLength} км:"), Times.Once);

            section.Verify(x => x.AddParagraph($"Определяем продолжительность строительства сети длиной {stepwiseExtrapolationDuration.StepwisePipelineStandard.PipelineLength} км:"), Times.Once);

            section.Verify(x => x.AddParagraph($"{calculationPipelineStandards[0].Duration} ∙ (100 + 100 ∙ 0,3) / 100 = {stepwiseExtrapolationDuration.StepwiseDuration} мес.", Constants.ParagraphHorizontalCentered), Times.Once);

            section.Verify(x => x.AddParagraph(
                "1. Определяем изменение объема (уменьшение) объема, %:"), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"({stepwiseExtrapolationDuration.PipelineLength} + {calculationPipelineStandards[0].PipelineLength}) / {calculationPipelineStandards[0].PipelineLength} ∙ 100 = {stepwiseExtrapolationDuration.VolumeChangePercent} %."), Times.Once);

            section.Verify(x => x.AddParagraph(
                "2. Определяем изменение (уменьшение) нормы продолжительности строительства, %:"), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"{stepwiseExtrapolationDuration.VolumeChangePercent} ∙ 0,3 = {stepwiseExtrapolationDuration.StandardChangePercent} %,"), Times.Once);

            section.Verify(x => x.AddParagraph(
                "где 0,3 – коэффициент изменения продолжительности строительства на каждый процент изменения объема."), Times.Once);

            section.Verify(x => x.AddParagraph(
                "3. Определяем нормативную продолжительность строительства, мес:"), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"{calculationPipelineStandards[0].Duration} ∙ (100 - {stepwiseExtrapolationDuration.StandardChangePercent}) / 100 = {stepwiseExtrapolationDuration.Duration} мес."), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"4. Нормативная продолжительность строительства сети длиной {stepwiseExtrapolationDuration.PipelineLength} км. составляет {stepwiseExtrapolationDuration.Duration} мес."), Times.Once);

            section.Verify(x => x.AddParagraph(
                $"Принимаем нормативную продолжительность строительства {stepwiseExtrapolationDuration.RoundedDuration} мес, в т.ч. - {stepwiseExtrapolationDuration.PreparatoryPeriod} мес. - подготовительный период."), Times.Once);
        }
    }
}