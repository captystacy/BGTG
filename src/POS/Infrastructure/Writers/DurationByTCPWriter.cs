using System.Globalization;
using POS.DomainModels;
using POS.DomainModels.DurationByTCPDomainModels;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;

namespace POS.Infrastructure.Writers;

public class DurationByTCPWriter : IDurationByTCPWriter
{
    private readonly IWordDocumentService _wordDocumentService;
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

    public DurationByTCPWriter(IWordDocumentService wordDocumentService)
    {
        _wordDocumentService = wordDocumentService;
    }

    public MemoryStream Write(DurationByTCP durationByTCP, string templatePath)
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

        _wordDocumentService.Load(templatePath);
        ReplacePatternsWithActualValues(durationByTCP);

        var memoryStream = new MemoryStream();
        _wordDocumentService.SaveAs(memoryStream, MyFileFormat.DocX);
        _wordDocumentService.DisposeLastDocument();
        return memoryStream;
    }

    private void ReplacePatternsWithActualValues(DurationByTCP durationByTCP)
    {
        _wordDocumentService.ReplaceTextInDocument(PipelineMaterialPattern, durationByTCP.PipelineMaterial);
        _wordDocumentService.ReplaceTextInDocument(PipelineDiameterPresentationPattern, durationByTCP.PipelineDiameterPresentation);
        _wordDocumentService.ReplaceTextInDocument(PipelineLengthPattern, durationByTCP.PipelineLength.ToString(AppConstants.DecimalFormat));
        _wordDocumentService.ReplaceTextInDocument(DurationPattern, durationByTCP.Duration.ToString(AppConstants.DecimalFormat));
        _wordDocumentService.ReplaceTextInDocument(RoundedDurationPattern, durationByTCP.RoundedDuration.ToString(AppConstants.DecimalFormat));
        _wordDocumentService.ReplaceTextInDocument(PreparatoryPeriodPattern, durationByTCP.PreparatoryPeriod.ToString(AppConstants.DecimalFormat));
        _wordDocumentService.ReplaceTextInDocument(AppendixKeyPattern, durationByTCP.AppendixKey.ToString());
        _wordDocumentService.ReplaceTextInDocument(AppendixPagePattern, durationByTCP.AppendixPage.ToString());

        ReplaceDurationCalculationTypeAndParagraph(durationByTCP.DurationCalculationType);

        ReplaceSpecificCalculationTypeValues(durationByTCP);
    }

    private void ReplaceSpecificCalculationTypeValues(DurationByTCP durationByTCP)
    {
        switch (durationByTCP)
        {
            case InterpolationDurationByTCP interpolationDuration:
                var calculationPipelineStandards = interpolationDuration.CalculationPipelineStandards.ToArray();

                _wordDocumentService.ReplaceTextInDocument(InterpolationCalculationPipelineStandardPipelineLengthPattern0,
                    calculationPipelineStandards[0].PipelineLength.ToString(AppConstants.DecimalFormat));
                _wordDocumentService.ReplaceTextInDocument(InterpolationCalculationPipelineStandardPipelineLengthPattern1,
                    calculationPipelineStandards[1].PipelineLength.ToString(AppConstants.DecimalFormat));
                _wordDocumentService.ReplaceTextInDocument(InterpolationCalculationPipelineStandardDurationPattern0,
                    calculationPipelineStandards[0].Duration.ToString(AppConstants.DecimalFormat));
                _wordDocumentService.ReplaceTextInDocument(InterpolationCalculationPipelineStandardDurationPattern1,
                    calculationPipelineStandards[1].Duration.ToString(AppConstants.DecimalFormat));
                _wordDocumentService.ReplaceTextInDocument(DurationChangePattern,
                    interpolationDuration.DurationChange.ToString(AppConstants.DecimalFormat));
                _wordDocumentService.ReplaceTextInDocument(VolumeChangePattern,
                    interpolationDuration.VolumeChange.ToString(AppConstants.DecimalFormat));
                break;
            case ExtrapolationDurationByTCP extrapolationDuration:
                var calculationPipelineStandard = extrapolationDuration.CalculationPipelineStandards.First();

                _wordDocumentService.ReplaceTextInDocument(ExtrapolationCalculationPipelineStandardPipelineLengthPattern,
                    calculationPipelineStandard.PipelineLength.ToString(AppConstants.DecimalFormat));
                _wordDocumentService.ReplaceTextInDocument(ExtrapolationCalculationPipelineStandardDurationPattern,
                    calculationPipelineStandard.Duration.ToString(AppConstants.DecimalFormat));

                _wordDocumentService.ReplaceTextInDocument(VolumeChangePercentPattern,
                    extrapolationDuration.VolumeChangePercent.ToString(AppConstants.DecimalFormat));
                _wordDocumentService.ReplaceTextInDocument(StandardChangePercentPattern,
                    extrapolationDuration.StandardChangePercent.ToString(AppConstants.DecimalFormat));

                if (extrapolationDuration is StepwiseExtrapolationDurationByTCP stepwiseExtrapolationDurationByTCP)
                {
                    _wordDocumentService.ReplaceTextInDocument(StepwiseDurationPattern,
                        stepwiseExtrapolationDurationByTCP.StepwiseDuration.ToString(AppConstants.DecimalFormat));
                    _wordDocumentService.ReplaceTextInDocument(StepwisePipelineStandardPipelineLength,
                        stepwiseExtrapolationDurationByTCP.StepwisePipelineStandard.PipelineLength.ToString(
                            AppConstants.DecimalFormat));
                    _wordDocumentService.ReplaceTextInDocument(StepwisePipelineStandardDuration,
                        stepwiseExtrapolationDurationByTCP.StepwisePipelineStandard.Duration.ToString(
                            AppConstants.DecimalFormat));
                }

                break;
        }
    }

    private void ReplaceDurationCalculationTypeAndParagraph(DurationCalculationType durationCalculationType)
    {
        switch (durationCalculationType)
        {
            case DurationCalculationType.Interpolation:
                _wordDocumentService.ReplaceTextInDocument(DurationCalculationTypePattern, InterpolationDurationCalculationTypeName);
                _wordDocumentService.ReplaceTextInDocument(DurationCalculationTypeParagraphPattern,
                    InterpolationDurationCalculationTypeParagraphName);
                break;
            case DurationCalculationType.ExtrapolationAscending:
                _wordDocumentService.ReplaceTextInDocument(DurationCalculationTypePattern, ExtrapolationDurationCalculationTypeName);
                _wordDocumentService.ReplaceTextInDocument(DurationCalculationTypeParagraphPattern,
                    ExtrapolationAscendingDurationCalculationTypeParagraphName);
                break;
            case DurationCalculationType.ExtrapolationDescending:
                _wordDocumentService.ReplaceTextInDocument(DurationCalculationTypePattern, ExtrapolationDurationCalculationTypeName);
                _wordDocumentService.ReplaceTextInDocument(DurationCalculationTypeParagraphPattern,
                    ExtrapolationDescendingDurationCalculationTypeParagraphName);
                break;
            case DurationCalculationType.StepwiseExtrapolationAscending:
                _wordDocumentService.ReplaceTextInDocument(DurationCalculationTypePattern, StepwiseExtrapolationDurationCalculationTypeName);
                _wordDocumentService.ReplaceTextInDocument(DurationCalculationTypeParagraphPattern,
                    StepwiseExtrapolationAscendingDurationCalculationTypeParagraphName);
                break;
            case DurationCalculationType.StepwiseExtrapolationDescending:
                _wordDocumentService.ReplaceTextInDocument(DurationCalculationTypePattern, StepwiseExtrapolationDurationCalculationTypeName);
                _wordDocumentService.ReplaceTextInDocument(DurationCalculationTypeParagraphPattern,
                    StepwiseExtrapolationDescendingDurationCalculationTypeParagraphName);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}