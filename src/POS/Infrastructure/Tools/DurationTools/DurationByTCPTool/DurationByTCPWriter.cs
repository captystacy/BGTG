using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.Base;
using POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.Models;
using System.Globalization;

namespace POS.Infrastructure.Tools.DurationTools.DurationByTCPTool;

public class DurationByTCPWriter : IDurationByTCPWriter
{
    private readonly IDocumentService _documentService;
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

    public DurationByTCPWriter(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    public MemoryStream Write(DurationByTCP durationByTCP, string templatePath)
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

        _documentService.Load(templatePath);
        ReplacePatternsWithActualValues(durationByTCP);

        var memoryStream = new MemoryStream();
        _documentService.SaveAs(memoryStream);
        _documentService.Dispose();
        return memoryStream;
    }

    private void ReplacePatternsWithActualValues(DurationByTCP durationByTCP)
    {
        _documentService.ReplaceText(PipelineMaterialPattern, durationByTCP.PipelineMaterial);
        _documentService.ReplaceText(PipelineDiameterPresentationPattern, durationByTCP.PipelineDiameterPresentation);
        _documentService.ReplaceText(PipelineLengthPattern, durationByTCP.PipelineLength.ToString(AppData.DecimalFormat));
        _documentService.ReplaceText(DurationPattern, durationByTCP.Duration.ToString(AppData.DecimalFormat));
        _documentService.ReplaceText(RoundedDurationPattern, durationByTCP.RoundedDuration.ToString(AppData.DecimalFormat));
        _documentService.ReplaceText(PreparatoryPeriodPattern, durationByTCP.PreparatoryPeriod.ToString(AppData.DecimalFormat));
        _documentService.ReplaceText(AppendixKeyPattern, durationByTCP.AppendixKey.ToString());
        _documentService.ReplaceText(AppendixPagePattern, durationByTCP.AppendixPage.ToString());

        ReplaceDurationCalculationTypeAndParagraph(durationByTCP.DurationCalculationType);

        ReplaceSpecificCalculationTypeValues(durationByTCP);
    }

    private void ReplaceSpecificCalculationTypeValues(DurationByTCP durationByTCP)
    {
        switch (durationByTCP)
        {
            case InterpolationDurationByTCP interpolationDuration:
                var calculationPipelineStandards = interpolationDuration.CalculationPipelineStandards.ToArray();

                _documentService.ReplaceText(InterpolationCalculationPipelineStandardPipelineLengthPattern0,
                    calculationPipelineStandards[0].PipelineLength.ToString(AppData.DecimalFormat));
                _documentService.ReplaceText(InterpolationCalculationPipelineStandardPipelineLengthPattern1,
                    calculationPipelineStandards[1].PipelineLength.ToString(AppData.DecimalFormat));
                _documentService.ReplaceText(InterpolationCalculationPipelineStandardDurationPattern0,
                    calculationPipelineStandards[0].Duration.ToString(AppData.DecimalFormat));
                _documentService.ReplaceText(InterpolationCalculationPipelineStandardDurationPattern1,
                    calculationPipelineStandards[1].Duration.ToString(AppData.DecimalFormat));
                _documentService.ReplaceText(DurationChangePattern,
                    interpolationDuration.DurationChange.ToString(AppData.DecimalFormat));
                _documentService.ReplaceText(VolumeChangePattern,
                    interpolationDuration.VolumeChange.ToString(AppData.DecimalFormat));
                break;
            case ExtrapolationDurationByTCP extrapolationDuration:
                var calculationPipelineStandard = extrapolationDuration.CalculationPipelineStandards.First();

                _documentService.ReplaceText(ExtrapolationCalculationPipelineStandardPipelineLengthPattern,
                    calculationPipelineStandard.PipelineLength.ToString(AppData.DecimalFormat));
                _documentService.ReplaceText(ExtrapolationCalculationPipelineStandardDurationPattern,
                    calculationPipelineStandard.Duration.ToString(AppData.DecimalFormat));

                _documentService.ReplaceText(VolumeChangePercentPattern,
                    extrapolationDuration.VolumeChangePercent.ToString(AppData.DecimalFormat));
                _documentService.ReplaceText(StandardChangePercentPattern,
                    extrapolationDuration.StandardChangePercent.ToString(AppData.DecimalFormat));

                if (extrapolationDuration is StepwiseExtrapolationDurationByTCP stepwiseExtrapolationDurationByTCP)
                {
                    _documentService.ReplaceText(StepwiseDurationPattern,
                        stepwiseExtrapolationDurationByTCP.StepwiseDuration.ToString(AppData.DecimalFormat));
                    _documentService.ReplaceText(StepwisePipelineStandardPipelineLength,
                        stepwiseExtrapolationDurationByTCP.StepwisePipelineStandard.PipelineLength.ToString(
                            AppData.DecimalFormat));
                    _documentService.ReplaceText(StepwisePipelineStandardDuration,
                        stepwiseExtrapolationDurationByTCP.StepwisePipelineStandard.Duration.ToString(
                            AppData.DecimalFormat));
                }

                break;
        }
    }

    private void ReplaceDurationCalculationTypeAndParagraph(DurationCalculationType durationCalculationType)
    {
        switch (durationCalculationType)
        {
            case DurationCalculationType.Interpolation:
                _documentService.ReplaceText(DurationCalculationTypePattern, InterpolationDurationCalculationTypeName);
                _documentService.ReplaceText(DurationCalculationTypeParagraphPattern,
                    InterpolationDurationCalculationTypeParagraphName);
                break;
            case DurationCalculationType.ExtrapolationAscending:
                _documentService.ReplaceText(DurationCalculationTypePattern, ExtrapolationDurationCalculationTypeName);
                _documentService.ReplaceText(DurationCalculationTypeParagraphPattern,
                    ExtrapolationAscendingDurationCalculationTypeParagraphName);
                break;
            case DurationCalculationType.ExtrapolationDescending:
                _documentService.ReplaceText(DurationCalculationTypePattern, ExtrapolationDurationCalculationTypeName);
                _documentService.ReplaceText(DurationCalculationTypeParagraphPattern,
                    ExtrapolationDescendingDurationCalculationTypeParagraphName);
                break;
            case DurationCalculationType.StepwiseExtrapolationAscending:
                _documentService.ReplaceText(DurationCalculationTypePattern, StepwiseExtrapolationDurationCalculationTypeName);
                _documentService.ReplaceText(DurationCalculationTypeParagraphPattern,
                    StepwiseExtrapolationAscendingDurationCalculationTypeParagraphName);
                break;
            case DurationCalculationType.StepwiseExtrapolationDescending:
                _documentService.ReplaceText(DurationCalculationTypePattern, StepwiseExtrapolationDurationCalculationTypeName);
                _documentService.ReplaceText(DurationCalculationTypeParagraphPattern,
                    StepwiseExtrapolationDescendingDurationCalculationTypeParagraphName);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}