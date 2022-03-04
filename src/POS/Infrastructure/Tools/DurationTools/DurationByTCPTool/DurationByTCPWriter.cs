using System.Globalization;
using POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.Base;
using POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.Models;
using Xceed.Words.NET;

namespace POS.Infrastructure.Tools.DurationTools.DurationByTCPTool;

public class DurationByTCPWriter : IDurationByTCPWriter
{
    private const string PipelineMaterialPattern = "%PM%";
    private const string PipelineDiameterPresentationPattern = "%PDP%";
    private const string PipelineLengthPattern = "%PL%";
    private const string DurationCalculationTypePattern = "%DCT%";
    private const string DurationCalculationTypeParagraphPattern = "%DCTP%";
    private const string DurationPattern = "%D%";
    private const string RoundedDurationPattern = "%RD%";
    private const string PreparatoryPeriodPattern = "%PP%";

    private const string AppendixKeyPattern = "%AK%";
    private const string AppendixPagePattern = "%AP%";

    #region Interpolation
    private const string InterpolationDurationCalculationTypeName = "интерполяции";
    private const string InterpolationDurationCalculationTypeParagraphName = "А";

    private const string DurationChangePattern = "%DC%";
    private const string VolumeChangePattern = "%VC%";

    private const string InterpolationCalculationPipelineStandardPipelineLengthPattern0 = "%ICPSPL0%";
    private const string InterpolationCalculationPipelineStandardPipelineLengthPattern1 = "%ICPSPL1%";
    private const string InterpolationCalculationPipelineStandardDurationPattern0 = "%ICPSD0%";
    private const string InterpolationCalculationPipelineStandardDurationPattern1 = "%ICPSD1%";
    #endregion

    #region Extrapolation

    private const string ExtrapolationDurationCalculationTypeName = "экстраполяции";
    private const string ExtrapolationDescendingDurationCalculationTypeParagraphName = "Б.1";
    private const string ExtrapolationAscendingDurationCalculationTypeParagraphName = "Б.2";

    private const string ExtrapolationCalculationPipelineStandardPipelineLengthPattern = "%ECPSPL%";
    private const string ExtrapolationCalculationPipelineStandardDurationPattern = "%ECPSD%";
    private const string VolumeChangePercentPattern = "%VCP%";
    private const string StandardChangePercentPattern = "%SCP%";

    #region Stepwise Extrapolation
    private const string StepwiseExtrapolationDurationCalculationTypeName = "ступенчатой экстраполяции";
    private const string StepwiseExtrapolationDescendingDurationCalculationTypeParagraphName = "В.1";
    private const string StepwiseExtrapolationAscendingDurationCalculationTypeParagraphName = "В.2";

    private const string StepwiseDurationPattern = "%SD%";
    private const string StepwisePipelineStandardPipelineLength = "%SPSPL%";
    private const string StepwisePipelineStandardDuration = "%SPSD%";
    #endregion
    #endregion

    public void Write(DurationByTCP durationByTCP, string templatePath)
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

        using var document = DocX.Load(templatePath);
        ReplacePatternsWithActualValues(document, durationByTCP);
    }

    private void ReplacePatternsWithActualValues(DocX document, DurationByTCP durationByTCP)
    {
        document.ReplaceText(PipelineMaterialPattern, durationByTCP.PipelineMaterial);
        document.ReplaceText(PipelineDiameterPresentationPattern, durationByTCP.PipelineDiameterPresentation);
        document.ReplaceText(PipelineLengthPattern, durationByTCP.PipelineLength.ToString(AppData.DecimalFormat));
        document.ReplaceText(DurationPattern, durationByTCP.Duration.ToString(AppData.DecimalFormat));
        document.ReplaceText(RoundedDurationPattern, durationByTCP.RoundedDuration.ToString(AppData.DecimalFormat));
        document.ReplaceText(PreparatoryPeriodPattern, durationByTCP.PreparatoryPeriod.ToString(AppData.DecimalFormat));
        document.ReplaceText(AppendixKeyPattern, durationByTCP.AppendixKey.ToString());
        document.ReplaceText(AppendixPagePattern, durationByTCP.AppendixPage.ToString());

        ReplaceDurationCalculationTypeAndParagraph(document, durationByTCP.DurationCalculationType);

        ReplaceSpecificCalculationTypeValues(document, durationByTCP);
    }

    private void ReplaceSpecificCalculationTypeValues(DocX document, DurationByTCP durationByTCP)
    {
        switch (durationByTCP)
        {
            case InterpolationDurationByTCP interpolationDuration:
                var calculationPipelineStandards = interpolationDuration.CalculationPipelineStandards.ToArray();

                document.ReplaceText(InterpolationCalculationPipelineStandardPipelineLengthPattern0, calculationPipelineStandards[0].PipelineLength.ToString(AppData.DecimalFormat));
                document.ReplaceText(InterpolationCalculationPipelineStandardPipelineLengthPattern1, calculationPipelineStandards[1].PipelineLength.ToString(AppData.DecimalFormat));
                document.ReplaceText(InterpolationCalculationPipelineStandardDurationPattern0, calculationPipelineStandards[0].Duration.ToString(AppData.DecimalFormat));
                document.ReplaceText(InterpolationCalculationPipelineStandardDurationPattern1, calculationPipelineStandards[1].Duration.ToString(AppData.DecimalFormat));
                document.ReplaceText(DurationChangePattern, interpolationDuration.DurationChange.ToString(AppData.DecimalFormat));
                document.ReplaceText(VolumeChangePattern, interpolationDuration.VolumeChange.ToString(AppData.DecimalFormat));
                break;
            case ExtrapolationDurationByTCP extrapolationDuration:
                var calculationPipelineStandard = extrapolationDuration.CalculationPipelineStandards.First();

                document.ReplaceText(ExtrapolationCalculationPipelineStandardPipelineLengthPattern, calculationPipelineStandard.PipelineLength.ToString(AppData.DecimalFormat));
                document.ReplaceText(ExtrapolationCalculationPipelineStandardDurationPattern, calculationPipelineStandard.Duration.ToString(AppData.DecimalFormat));

                document.ReplaceText(VolumeChangePercentPattern, extrapolationDuration.VolumeChangePercent.ToString(AppData.DecimalFormat));
                document.ReplaceText(StandardChangePercentPattern, extrapolationDuration.StandardChangePercent.ToString(AppData.DecimalFormat));

                if (extrapolationDuration is StepwiseExtrapolationDurationByTCP stepwiseExtrapolationDurationByTCP)
                {
                    document.ReplaceText(StepwiseDurationPattern, stepwiseExtrapolationDurationByTCP.StepwiseDuration.ToString(AppData.DecimalFormat));
                    document.ReplaceText(StepwisePipelineStandardPipelineLength, stepwiseExtrapolationDurationByTCP.StepwisePipelineStandard.PipelineLength.ToString(AppData.DecimalFormat));
                    document.ReplaceText(StepwisePipelineStandardDuration, stepwiseExtrapolationDurationByTCP.StepwisePipelineStandard.Duration.ToString(AppData.DecimalFormat));
                }
                break;
        }
    }

    private void ReplaceDurationCalculationTypeAndParagraph(DocX document, DurationCalculationType durationCalculationType)
    {
        switch (durationCalculationType)
        {
            case DurationCalculationType.Interpolation:
                document.ReplaceText(DurationCalculationTypePattern, InterpolationDurationCalculationTypeName);
                document.ReplaceText(DurationCalculationTypeParagraphPattern, InterpolationDurationCalculationTypeParagraphName);
                break;
            case DurationCalculationType.ExtrapolationAscending:
                document.ReplaceText(DurationCalculationTypePattern, ExtrapolationDurationCalculationTypeName);
                document.ReplaceText(DurationCalculationTypeParagraphPattern, ExtrapolationAscendingDurationCalculationTypeParagraphName);
                break;
            case DurationCalculationType.ExtrapolationDescending:
                document.ReplaceText(DurationCalculationTypePattern, ExtrapolationDurationCalculationTypeName);
                document.ReplaceText(DurationCalculationTypeParagraphPattern, ExtrapolationDescendingDurationCalculationTypeParagraphName);
                break;
            case DurationCalculationType.StepwiseExtrapolationAscending:
                document.ReplaceText(DurationCalculationTypePattern, StepwiseExtrapolationDurationCalculationTypeName);
                document.ReplaceText(DurationCalculationTypeParagraphPattern, StepwiseExtrapolationAscendingDurationCalculationTypeParagraphName);
                break;
            case DurationCalculationType.StepwiseExtrapolationDescending:
                document.ReplaceText(DurationCalculationTypePattern, StepwiseExtrapolationDurationCalculationTypeName);
                document.ReplaceText(DurationCalculationTypeParagraphPattern, StepwiseExtrapolationDescendingDurationCalculationTypeParagraphName);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}