using POS.DomainModels.DocumentDomainModels;
using POS.DomainModels.DurationByTCPDomainModels;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Services;
using POS.Infrastructure.Writers.Base;
using System.Globalization;

namespace POS.Infrastructure.Writers;

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

    #endregion

    #region Stepwise Extrapolation

    private const string StepwiseExtrapolationDurationCalculationTypeName = "ступенчатой экстраполяции";
    private const string StepwiseExtrapolationDescendingDurationCalculationTypeParagraphName = "В.1";
    private const string StepwiseExtrapolationAscendingDurationCalculationTypeParagraphName = "В.2";

    private const string StepwiseDurationPattern = "%SD%";
    private const string StepwisePipelineStandardPipelineLength = "%SPSPL%";
    private const string StepwisePipelineStandardDuration = "%SPSD%";

    #endregion

    public MemoryStream Write(DurationByTCP durationByTCP, string templatePath)
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

        using var document = DocumentService.Load(templatePath);
        ReplacePatternsWithActualValues(document, durationByTCP);

        var memoryStream = new MemoryStream();
        document.SaveAs(memoryStream);

        return memoryStream;
    }

    private void ReplacePatternsWithActualValues(MyDocument document, DurationByTCP durationByTCP)
    {
        document.ReplaceText(PipelineMaterialPattern, durationByTCP.PipelineMaterial);
        document.ReplaceText(PipelineDiameterPresentationPattern, durationByTCP.PipelineDiameterPresentation);
        document.ReplaceText(PipelineLengthPattern, durationByTCP.PipelineLength.ToString(AppConstants.DecimalFormat));
        document.ReplaceText(DurationPattern, durationByTCP.Duration.ToString(AppConstants.DecimalFormat));
        document.ReplaceText(RoundedDurationPattern,
            durationByTCP.RoundedDuration.ToString(AppConstants.DecimalFormat));
        document.ReplaceText(PreparatoryPeriodPattern,
            durationByTCP.PreparatoryPeriod.ToString(AppConstants.DecimalFormat));
        document.ReplaceText(AppendixKeyPattern, durationByTCP.AppendixKey.ToString());
        document.ReplaceText(AppendixPagePattern, durationByTCP.AppendixPage.ToString());

        ReplaceDurationCalculationTypeAndParagraph(document, durationByTCP.DurationCalculationType);

        ReplaceSpecificCalculationTypeValues(document, durationByTCP);
    }

    private void ReplaceSpecificCalculationTypeValues(MyDocument document, DurationByTCP durationByTCP)
    {
        switch (durationByTCP)
        {
            case InterpolationDurationByTCP interpolationDuration:
                var calculationPipelineStandards = interpolationDuration.CalculationPipelineStandards.ToArray();

                document.ReplaceText(InterpolationCalculationPipelineStandardPipelineLengthPattern0,
                    calculationPipelineStandards[0].PipelineLength.ToString(AppConstants.DecimalFormat));
                document.ReplaceText(InterpolationCalculationPipelineStandardPipelineLengthPattern1,
                    calculationPipelineStandards[1].PipelineLength.ToString(AppConstants.DecimalFormat));
                document.ReplaceText(InterpolationCalculationPipelineStandardDurationPattern0,
                    calculationPipelineStandards[0].Duration.ToString(AppConstants.DecimalFormat));
                document.ReplaceText(InterpolationCalculationPipelineStandardDurationPattern1,
                    calculationPipelineStandards[1].Duration.ToString(AppConstants.DecimalFormat));
                document.ReplaceText(DurationChangePattern,
                    interpolationDuration.DurationChange.ToString(AppConstants.DecimalFormat));
                document.ReplaceText(VolumeChangePattern,
                    interpolationDuration.VolumeChange.ToString(AppConstants.DecimalFormat));
                break;
            case ExtrapolationDurationByTCP extrapolationDuration:
                var calculationPipelineStandard = extrapolationDuration.CalculationPipelineStandards.First();

                document.ReplaceText(ExtrapolationCalculationPipelineStandardPipelineLengthPattern,
                    calculationPipelineStandard.PipelineLength.ToString(AppConstants.DecimalFormat));
                document.ReplaceText(ExtrapolationCalculationPipelineStandardDurationPattern,
                    calculationPipelineStandard.Duration.ToString(AppConstants.DecimalFormat));

                document.ReplaceText(VolumeChangePercentPattern,
                    extrapolationDuration.VolumeChangePercent.ToString(AppConstants.DecimalFormat));
                document.ReplaceText(StandardChangePercentPattern,
                    extrapolationDuration.StandardChangePercent.ToString(AppConstants.DecimalFormat));

                if (extrapolationDuration is StepwiseExtrapolationDurationByTCP stepwiseExtrapolationDurationByTCP)
                {
                    document.ReplaceText(StepwiseDurationPattern,
                        stepwiseExtrapolationDurationByTCP.StepwiseDuration.ToString(AppConstants.DecimalFormat));
                    document.ReplaceText(StepwisePipelineStandardPipelineLength,
                        stepwiseExtrapolationDurationByTCP.StepwisePipelineStandard.PipelineLength.ToString(AppConstants
                            .DecimalFormat));
                    document.ReplaceText(StepwisePipelineStandardDuration,
                        stepwiseExtrapolationDurationByTCP.StepwisePipelineStandard.Duration.ToString(AppConstants
                            .DecimalFormat));
                }

                break;
        }
    }

    private void ReplaceDurationCalculationTypeAndParagraph(MyDocument document,
        DurationCalculationType durationCalculationType)
    {
        switch (durationCalculationType)
        {
            case DurationCalculationType.Interpolation:
                document.ReplaceText(DurationCalculationTypePattern, InterpolationDurationCalculationTypeName);
                document.ReplaceText(DurationCalculationTypeParagraphPattern,
                    InterpolationDurationCalculationTypeParagraphName);
                break;
            case DurationCalculationType.ExtrapolationAscending:
                document.ReplaceText(DurationCalculationTypePattern, ExtrapolationDurationCalculationTypeName);
                document.ReplaceText(DurationCalculationTypeParagraphPattern,
                    ExtrapolationAscendingDurationCalculationTypeParagraphName);
                break;
            case DurationCalculationType.ExtrapolationDescending:
                document.ReplaceText(DurationCalculationTypePattern, ExtrapolationDurationCalculationTypeName);
                document.ReplaceText(DurationCalculationTypeParagraphPattern,
                    ExtrapolationDescendingDurationCalculationTypeParagraphName);
                break;
            case DurationCalculationType.StepwiseExtrapolationAscending:
                document.ReplaceText(DurationCalculationTypePattern, StepwiseExtrapolationDurationCalculationTypeName);
                document.ReplaceText(DurationCalculationTypeParagraphPattern,
                    StepwiseExtrapolationAscendingDurationCalculationTypeParagraphName);
                break;
            case DurationCalculationType.StepwiseExtrapolationDescending:
                document.ReplaceText(DurationCalculationTypePattern, StepwiseExtrapolationDurationCalculationTypeName);
                document.ReplaceText(DurationCalculationTypeParagraphPattern,
                    StepwiseExtrapolationDescendingDurationCalculationTypeParagraphName);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}