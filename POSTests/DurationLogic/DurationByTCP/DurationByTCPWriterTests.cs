using System.IO;
using NUnit.Framework;
using POS.DurationLogic.DurationByTCP;
using POS.DurationLogic.DurationByTCP.TCP;
using Xceed.Words.NET;

namespace POSTests.DurationLogic.DurationByTCP
{
    public class DurationByTCPWriterTests
    {
        private DurationByTCPWriter _durationByTCPWriter;
        private const string DurationByTCPTemplatesDirectory = @"..\..\..\DurationLogic\DurationByTCP\DurationByTCPTemplates";

        [SetUp]
        public void SetUp()
        {
            _durationByTCPWriter = new DurationByTCPWriter();
        }

        [Test]
        public void Write_InterpolationDuration_SaveCorrectDuration()
        {
            var pipelineMaterial = "стальных";
            var pipelineDiameter = 0;
            var pipelineDiameterPresentation = "до 500";
            var pipelineLength = 13M;
            var appendix = TCP212.AppendixA;
            var durationCalculationType = DurationCalculationType.Interpolation;
            var duration = 10.8M;
            var roundedDuration = 11M;
            var preparatoryPeriod = 1.1M;
            var durationChange = 0.6M;
            var volumeChange = 3M;

            var calculationPipelineStandards = new[]
            {
                new PipelineStandard(10M, 9M, 0),
                new PipelineStandard(15M, 12M, 0),
            };

            var interpolationDuration = new InterpolationDurationByTCP(pipelineMaterial, pipelineDiameter,
                pipelineDiameterPresentation, pipelineLength, calculationPipelineStandards, durationCalculationType, duration,
                roundedDuration, preparatoryPeriod, durationChange, volumeChange, appendix);

            var templateFileName = "InterpolationTemplate.docx";
            var templatePath = Path.Combine(DurationByTCPTemplatesDirectory, templateFileName);
            var saveFileName = "InterpolationDurationByTCP.docx";
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), saveFileName);

            _durationByTCPWriter.Write(interpolationDuration, templatePath, savePath);

            using var document = DocX.Load(savePath);
            StringAssert.Contains(interpolationDuration.PipelineMaterial, document.Text);
            StringAssert.Contains(interpolationDuration.VolumeChange.ToString(), document.Text);
            StringAssert.Contains(interpolationDuration.DurationChange.ToString(), document.Text);
            StringAssert.Contains(interpolationDuration.Duration.ToString(), document.Text);
            StringAssert.Contains(interpolationDuration.Appendix.Key.ToString(), document.Text);
            StringAssert.Contains(interpolationDuration.Appendix.Page.ToString(), document.Text);
            StringAssert.Contains(calculationPipelineStandards[0].PipelineLength.ToString(), document.Text);
            StringAssert.Contains(calculationPipelineStandards[0].Duration.ToString(), document.Text);
            StringAssert.Contains(calculationPipelineStandards[1].PipelineLength.ToString(), document.Text);
            StringAssert.Contains(calculationPipelineStandards[1].Duration.ToString(), document.Text);
            StringAssert.Contains("интерполяции", document.Text);
            StringAssert.Contains(interpolationDuration.PipelineDiameterPresentation, document.Text);
            StringAssert.Contains(interpolationDuration.PipelineLength.ToString(), document.Text);
            StringAssert.Contains(interpolationDuration.RoundedDuration.ToString(), document.Text);
            StringAssert.Contains(interpolationDuration.PreparatoryPeriod.ToString(), document.Text);
        }

        [Test]
        public void Write_ExtrapolationAscendingDuration_SaveCorrectDuration()
        {
            var pipelineMaterial = "стальных";
            var pipelineDiameter = 0;
            var pipelineDiameterPresentation = "до 500";
            var pipelineLength = 25000M;
            var appendix = TCP212.AppendixA;
            var durationCalculationType = DurationCalculationType.ExtrapolationAscending;
            var duration = 11.4M;
            var roundedDuration = 11.5M;
            var preparatoryPeriod = 1.1M;
            var volumeChangePercent = 16.7M;
            var standardChangePercent = 5M;

            var calculationPipelineStandards = new[]
            {
                new PipelineStandard(30000M, 12M, 0),
            };

            var extrapolationAscendingDuration = new ExtrapolationDurationByTCP(pipelineMaterial, pipelineDiameter,
                pipelineDiameterPresentation, pipelineLength, calculationPipelineStandards, durationCalculationType, duration,
                roundedDuration, preparatoryPeriod, volumeChangePercent, standardChangePercent, appendix);

            var templateFileName = "ExtrapolationAscendingTemplate.docx";
            var templatePath = Path.Combine(DurationByTCPTemplatesDirectory, templateFileName);
            var saveFileName = "ExtrapolationAscendingDurationByTCP.docx";
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), saveFileName);

            _durationByTCPWriter.Write(extrapolationAscendingDuration, templatePath, savePath);

            using var document = DocX.Load(savePath);
            StringAssert.Contains(extrapolationAscendingDuration.PipelineMaterial, document.Text);
            StringAssert.Contains(extrapolationAscendingDuration.VolumeChangePercent.ToString(), document.Text);
            StringAssert.Contains(extrapolationAscendingDuration.StandardChangePercent.ToString(), document.Text);
            StringAssert.Contains(extrapolationAscendingDuration.Duration.ToString(), document.Text);
            StringAssert.Contains(extrapolationAscendingDuration.Appendix.Key.ToString(), document.Text);
            StringAssert.Contains(extrapolationAscendingDuration.Appendix.Page.ToString(), document.Text);
            StringAssert.Contains(calculationPipelineStandards[0].PipelineLength.ToString(), document.Text);
            StringAssert.Contains(calculationPipelineStandards[0].Duration.ToString(), document.Text);
            StringAssert.Contains("экстраполяции", document.Text);
            StringAssert.Contains(extrapolationAscendingDuration.PipelineDiameterPresentation, document.Text);
            StringAssert.Contains(extrapolationAscendingDuration.PipelineLength.ToString(), document.Text);
            StringAssert.Contains(extrapolationAscendingDuration.RoundedDuration.ToString(), document.Text);
            StringAssert.Contains(extrapolationAscendingDuration.PreparatoryPeriod.ToString(), document.Text);
        }

        [Test]
        public void Write_ExtrapolationDescendingDuration_SaveCorrectDuration()
        {
            var pipelineMaterial = "стальных";
            var pipelineDiameter = 0;
            var pipelineDiameterPresentation = "до 500";
            var pipelineLength = 62700M;
            var appendix = TCP212.AppendixA;
            var durationCalculationType = DurationCalculationType.ExtrapolationDescending;
            var duration = 15M;
            var roundedDuration = 15M;
            var preparatoryPeriod = 1.5M;
            var volumeChangePercent = 25.4M;
            var standardChangePercent = 7.6M;

            var calculationPipelineStandards = new[]
            {
                new PipelineStandard(50000M, 14M, 0),
            };

            var extrapolationAscendingDuration = new ExtrapolationDurationByTCP(pipelineMaterial, pipelineDiameter,
                pipelineDiameterPresentation, pipelineLength, calculationPipelineStandards, durationCalculationType, duration,
                roundedDuration, preparatoryPeriod, volumeChangePercent, standardChangePercent, appendix);

            var templateFileName = "ExtrapolationDescendingTemplate.docx";
            var templatePath = Path.Combine(DurationByTCPTemplatesDirectory, templateFileName);
            var saveFileName = "ExtrapolationDescendingDurationByTCP.docx";
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), saveFileName);

            _durationByTCPWriter.Write(extrapolationAscendingDuration, templatePath, savePath);

            using var document = DocX.Load(savePath);
            StringAssert.Contains(extrapolationAscendingDuration.PipelineMaterial, document.Text);
            StringAssert.Contains(extrapolationAscendingDuration.VolumeChangePercent.ToString(), document.Text);
            StringAssert.Contains(extrapolationAscendingDuration.StandardChangePercent.ToString(), document.Text);
            StringAssert.Contains(extrapolationAscendingDuration.Duration.ToString(), document.Text);
            StringAssert.Contains(extrapolationAscendingDuration.Appendix.Key.ToString(), document.Text);
            StringAssert.Contains(extrapolationAscendingDuration.Appendix.Page.ToString(), document.Text);
            StringAssert.Contains(calculationPipelineStandards[0].PipelineLength.ToString(), document.Text);
            StringAssert.Contains(calculationPipelineStandards[0].Duration.ToString(), document.Text);
            StringAssert.Contains("экстраполяции", document.Text);
            StringAssert.Contains(extrapolationAscendingDuration.PipelineDiameterPresentation, document.Text);
            StringAssert.Contains(extrapolationAscendingDuration.PipelineLength.ToString(), document.Text);
            StringAssert.Contains(extrapolationAscendingDuration.RoundedDuration.ToString(), document.Text);
            StringAssert.Contains(extrapolationAscendingDuration.PreparatoryPeriod.ToString(), document.Text);
        }

        [Test]
        public void Write_StepwiseExtrapolationAscendingDuration_SaveCorrectDuration()
        {
            var pipelineMaterial = "стальных";
            var pipelineDiameter = 0;
            var pipelineDiameterPresentation = "до 500";
            var pipelineLength = 7000M;
            var appendix = TCP212.AppendixA;
            var durationCalculationType = DurationCalculationType.StepwiseExtrapolationAscending;
            var duration = 13.9M;
            var roundedDuration = 14M;
            var preparatoryPeriod = 1.1M;
            var volumeChangePercent = 30M;
            var standardChangePercent = 9M;
            var stepwiseDuration = 15.3M;

            var calculationPipelineStandards = new[]
            {
                new PipelineStandard(20000M, 18M, 0),
            };

            var stepwisePipelineStandard = new PipelineStandard(10000M, stepwiseDuration, 0);

            var stepwiseExtrapolationDurationByTCP = new StepwiseExtrapolationDurationByTCP(pipelineMaterial, pipelineDiameter,
                pipelineDiameterPresentation, pipelineLength, calculationPipelineStandards, durationCalculationType, duration,
                roundedDuration, preparatoryPeriod, volumeChangePercent, standardChangePercent, stepwiseDuration, stepwisePipelineStandard, appendix);

            var templateFileName = "StepwiseExtrapolationAscendingTemplate.docx";
            var templatePath = Path.Combine(DurationByTCPTemplatesDirectory, templateFileName);
            var saveFileName = "StepwiseExtrapolationAscendingDurationByTCP.docx";
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), saveFileName);

            _durationByTCPWriter.Write(stepwiseExtrapolationDurationByTCP, templatePath, savePath);

            using var document = DocX.Load(savePath);
            StringAssert.Contains(stepwiseExtrapolationDurationByTCP.PipelineMaterial, document.Text);
            StringAssert.Contains(stepwiseExtrapolationDurationByTCP.VolumeChangePercent.ToString(), document.Text);
            StringAssert.Contains(stepwiseExtrapolationDurationByTCP.StandardChangePercent.ToString(), document.Text);
            StringAssert.Contains(stepwiseExtrapolationDurationByTCP.Duration.ToString(), document.Text);
            StringAssert.Contains(stepwiseExtrapolationDurationByTCP.Appendix.Key.ToString(), document.Text);
            StringAssert.Contains(stepwiseExtrapolationDurationByTCP.Appendix.Page.ToString(), document.Text);
            StringAssert.Contains(calculationPipelineStandards[0].PipelineLength.ToString(), document.Text);
            StringAssert.Contains(calculationPipelineStandards[0].Duration.ToString(), document.Text);
            StringAssert.Contains(stepwisePipelineStandard.PipelineLength.ToString(), document.Text);
            StringAssert.Contains(stepwisePipelineStandard.Duration.ToString(), document.Text);
            StringAssert.Contains(stepwiseExtrapolationDurationByTCP.StepwiseDuration.ToString(), document.Text);
            StringAssert.Contains("ступенчатой экстраполяции", document.Text);
            StringAssert.Contains(stepwiseExtrapolationDurationByTCP.PipelineDiameterPresentation, document.Text);
            StringAssert.Contains(stepwiseExtrapolationDurationByTCP.PipelineLength.ToString(), document.Text);
            StringAssert.Contains(stepwiseExtrapolationDurationByTCP.RoundedDuration.ToString(), document.Text);
            StringAssert.Contains(stepwiseExtrapolationDurationByTCP.PreparatoryPeriod.ToString(), document.Text);
        }

        [Test]
        public void Write_StepwiseExtrapolationDescendingDuration_SaveCorrectDuration()
        {
            var pipelineMaterial = "стальных";
            var pipelineDiameter = 0;
            var pipelineDiameterPresentation = "до 500";
            var pipelineLength = 22000M;
            var appendix = TCP212.AppendixA;
            var durationCalculationType = DurationCalculationType.StepwiseExtrapolationAscending;
            var duration = 29.5M;
            var roundedDuration = 29.5M;
            var preparatoryPeriod = 2.9M;
            var volumeChangePercent = 10M;
            var standardChangePercent = 3M;
            var stepwiseDuration = 28.6M;

            var calculationPipelineStandards = new[]
            {
                new PipelineStandard(10000M, 22M, 0),
            };

            var stepwisePipelineStandard = new PipelineStandard(20000M, stepwiseDuration, 0);

            var stepwiseExtrapolationDurationByTCP = new StepwiseExtrapolationDurationByTCP(pipelineMaterial, pipelineDiameter,
                pipelineDiameterPresentation, pipelineLength, calculationPipelineStandards, durationCalculationType, duration,
                roundedDuration, preparatoryPeriod, volumeChangePercent, standardChangePercent, stepwiseDuration, stepwisePipelineStandard, appendix);

            var templateFileName = "StepwiseExtrapolationDescendingTemplate.docx";
            var templatePath = Path.Combine(DurationByTCPTemplatesDirectory, templateFileName);
            var saveFileName = "StepwiseExtrapolationDescendingDurationByTCP.docx";
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), saveFileName);

            _durationByTCPWriter.Write(stepwiseExtrapolationDurationByTCP, templatePath, savePath);

            using var document = DocX.Load(savePath);
            StringAssert.Contains(stepwiseExtrapolationDurationByTCP.PipelineMaterial, document.Text);
            StringAssert.Contains(stepwiseExtrapolationDurationByTCP.VolumeChangePercent.ToString(), document.Text);
            StringAssert.Contains(stepwiseExtrapolationDurationByTCP.StandardChangePercent.ToString(), document.Text);
            StringAssert.Contains(stepwiseExtrapolationDurationByTCP.Duration.ToString(), document.Text);
            StringAssert.Contains(stepwiseExtrapolationDurationByTCP.Appendix.Key.ToString(), document.Text);
            StringAssert.Contains(stepwiseExtrapolationDurationByTCP.Appendix.Page.ToString(), document.Text);
            StringAssert.Contains(calculationPipelineStandards[0].PipelineLength.ToString(), document.Text);
            StringAssert.Contains(calculationPipelineStandards[0].Duration.ToString(), document.Text);
            StringAssert.Contains(stepwisePipelineStandard.PipelineLength.ToString(), document.Text);
            StringAssert.Contains(stepwisePipelineStandard.Duration.ToString(), document.Text);
            StringAssert.Contains(stepwiseExtrapolationDurationByTCP.StepwiseDuration.ToString(), document.Text);
            StringAssert.Contains("ступенчатой экстраполяции", document.Text);
            StringAssert.Contains(stepwiseExtrapolationDurationByTCP.PipelineDiameterPresentation, document.Text);
            StringAssert.Contains(stepwiseExtrapolationDurationByTCP.PipelineLength.ToString(), document.Text);
            StringAssert.Contains(stepwiseExtrapolationDurationByTCP.RoundedDuration.ToString(), document.Text);
            StringAssert.Contains(stepwiseExtrapolationDurationByTCP.PreparatoryPeriod.ToString(), document.Text);
        }
    }
}
