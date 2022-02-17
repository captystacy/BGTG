using System.Collections.Generic;
using BGTG.POS.DurationTools.Base;
using BGTG.POS.DurationTools.DurationByTCPTool;
using BGTG.POS.DurationTools.DurationByTCPTool.Base;
using BGTG.POS.DurationTools.DurationByTCPTool.TCP;
using BGTG.POS.DurationTools.DurationByTCPTool.TCP.Interfaces;
using Moq;
using NUnit.Framework;

namespace BGTG.POS.Tests.DurationTools.DurationByTCPTool
{
    public class DurationByTCPCreatorTests
    {
        private DurationByTCPCreator _durationByTCPCreator = null!;
        private Mock<IDurationByTCPEngineer> _durationByTCPEngineerMock = null!;
        private Mock<ITCP212Helper> _tcp212HelperMock = null!;
        private Mock<IDurationRounder> _durationRounderMock = null!;

        [SetUp]
        public void SetUp()
        {
            _durationByTCPEngineerMock = new Mock<IDurationByTCPEngineer>();
            _tcp212HelperMock = new Mock<ITCP212Helper>();
            _durationRounderMock = new Mock<IDurationRounder>();
            _durationByTCPCreator = new DurationByTCPCreator(_durationByTCPEngineerMock.Object, _tcp212HelperMock.Object, _durationRounderMock.Object);
        }

        [Test]
        public void Create_InterpolationExample_CorrectDuration()
        {
            var pipelineMaterial = string.Empty;
            var pipelineDiameter = 0;
            var pipelineDiameterPresentation = "";
            var pipelineLength = 13M;
            var appendix = TCP212.AppendixA;
            var pipelineCategoryName = "";
            var durationCalculationType = DurationCalculationType.Interpolation;
            var duration = 10.8M;
            var roundedDuration = 11M;
            var preparatoryPeriod = 1.1M;
            var durationChange = 0.6M;
            var volumeChange = 3M;

            var calculationPipelineStandards = new[]
            {
                new PipelineStandard(10M, 9M, 0M),
                new PipelineStandard(15M, 12M, 0M),
            };

            var expectedInterpolationDuration = new InterpolationDurationByTCP(pipelineMaterial, pipelineDiameter,
                pipelineDiameterPresentation, pipelineLength, calculationPipelineStandards, durationCalculationType, duration,
                roundedDuration, preparatoryPeriod, durationChange, volumeChange, appendix.Key, appendix.Page);


            var pipelineCharacteristic = new PipelineCharacteristic(new DiameterRange(0, 0, pipelineDiameterPresentation),
                new List<PipelineStandard>());

            _tcp212HelperMock.Setup(x => x.GetAppendix(appendix.Key)).Returns(appendix);
            _tcp212HelperMock
                .Setup(x => x.GetPipelineCharacteristic(appendix, pipelineMaterial, pipelineDiameter,
                    pipelineCategoryName)).Returns(pipelineCharacteristic);

            _durationByTCPEngineerMock.SetupGet(x => x.CalculationPipelineStandards)
                .Returns(calculationPipelineStandards);
            _durationByTCPEngineerMock.SetupGet(x => x.DurationCalculationType).Returns(durationCalculationType);

            _durationRounderMock.Setup(x => x.GetRoundedDuration(duration)).Returns(roundedDuration);
            _durationRounderMock.Setup(x => x.GetRoundedPreparatoryPeriod(roundedDuration)).Returns(preparatoryPeriod);

            var actualInterpolationDuration = _durationByTCPCreator.Create(pipelineMaterial, pipelineDiameter,
                pipelineLength, appendix.Key, pipelineCategoryName);

            Assert.AreEqual(expectedInterpolationDuration, actualInterpolationDuration);

            _tcp212HelperMock.Verify(x => x.GetAppendix(appendix.Key), Times.Once);
            _tcp212HelperMock.Verify(
                x => x.GetPipelineCharacteristic(appendix, pipelineMaterial, pipelineDiameter, pipelineCategoryName),
                Times.Once);
            _durationByTCPEngineerMock.Verify(
                x => x.DefineCalculationType(pipelineCharacteristic.PipelineStandards, pipelineLength), Times.Once);

            _durationByTCPEngineerMock.VerifyGet(x => x.CalculationPipelineStandards, Times.Exactly(2));
            _durationByTCPEngineerMock.VerifyGet(x => x.DurationCalculationType, Times.Exactly(2));
            _durationRounderMock.Verify(x => x.GetRoundedDuration(duration), Times.Once);
            _durationRounderMock.Verify(x => x.GetRoundedPreparatoryPeriod(roundedDuration), Times.Once);
        }

        [Test]
        public void Create_ExtrapolationAscendingExample_CorrectDuration()
        {
            var pipelineMaterial = string.Empty;
            var pipelineDiameter = 0;
            var pipelineDiameterPresentation = "";
            var pipelineLength = 25000M;
            var appendix = TCP212.AppendixA;
            var pipelineCategoryName = "";
            var durationCalculationType = DurationCalculationType.ExtrapolationAscending;
            var duration = 11.4M;
            var roundedDuration = 11.5M;
            var preparatoryPeriod = 1.1M;
            var volumeChangePercent = 16.7M;
            var standardChangePercent = 5M;

            var calculationPipelineStandards = new[]
            {
                new PipelineStandard(30000M, 12M, 0M),
            };

            var expectedExtrapolationDuration = new ExtrapolationDurationByTCP(pipelineMaterial, pipelineDiameter,
                pipelineDiameterPresentation, pipelineLength,
                calculationPipelineStandards, durationCalculationType, duration, roundedDuration, preparatoryPeriod,
                volumeChangePercent, standardChangePercent, appendix.Key, appendix.Page);


            var pipelineCharacteristic = new PipelineCharacteristic(new DiameterRange(0, 0, pipelineDiameterPresentation),
                new List<PipelineStandard>());

            _tcp212HelperMock.Setup(x => x.GetAppendix(appendix.Key)).Returns(appendix);
            _tcp212HelperMock
                .Setup(x => x.GetPipelineCharacteristic(appendix, pipelineMaterial, pipelineDiameter,
                    pipelineCategoryName)).Returns(pipelineCharacteristic);

            _durationByTCPEngineerMock.SetupGet(x => x.CalculationPipelineStandards)
                .Returns(calculationPipelineStandards);

            _durationByTCPEngineerMock.SetupGet(x => x.DurationCalculationType).Returns(durationCalculationType);

            _durationRounderMock.Setup(x => x.GetRoundedDuration(duration)).Returns(roundedDuration);
            _durationRounderMock.Setup(x => x.GetRoundedPreparatoryPeriod(roundedDuration)).Returns(preparatoryPeriod);

            var actualExtrapolationDuration = _durationByTCPCreator.Create(pipelineMaterial, pipelineDiameter, pipelineLength, appendix.Key, pipelineCategoryName);

            Assert.AreEqual(expectedExtrapolationDuration, actualExtrapolationDuration);

            _tcp212HelperMock.Verify(x => x.GetAppendix(appendix.Key), Times.Once);
            _tcp212HelperMock.Verify(
                x => x.GetPipelineCharacteristic(appendix, pipelineMaterial, pipelineDiameter, pipelineCategoryName),
                Times.Once);
            _durationByTCPEngineerMock.Verify(
                x => x.DefineCalculationType(pipelineCharacteristic.PipelineStandards, pipelineLength), Times.Once);
            _durationByTCPEngineerMock.VerifyGet(x => x.CalculationPipelineStandards, Times.Exactly(2));
            _durationByTCPEngineerMock.VerifyGet(x => x.DurationCalculationType, Times.Exactly(2));
            _durationRounderMock.Verify(x => x.GetRoundedDuration(duration), Times.Once);
            _durationRounderMock.Verify(x => x.GetRoundedPreparatoryPeriod(roundedDuration), Times.Once);
        }

        [Test]
        public void Create_ExtrapolationDescendingExample_CorrectDuration()
        {
            var pipelineMaterial = string.Empty;
            var pipelineDiameter = 0;
            var pipelineDiameterPresentation = "";
            var pipelineLength = 62700M;
            var appendix = TCP212.AppendixA;
            var pipelineCategoryName = "";
            var durationCalculationType = DurationCalculationType.ExtrapolationDescending;
            var duration = 15.1M;
            var roundedDuration = 15M;
            var preparatoryPeriod = 1.5M;
            var volumeChangePercent = 25.4M;
            var standardChangePercent = 7.6M;

            var calculationPipelineStandards = new[]
            {
                new PipelineStandard(50000M, 14M, 0M),
            };

            var expectedExtrapolationDuration = new ExtrapolationDurationByTCP(pipelineMaterial, pipelineDiameter, pipelineDiameterPresentation, pipelineLength,
                calculationPipelineStandards, durationCalculationType, duration, roundedDuration, preparatoryPeriod, volumeChangePercent, standardChangePercent, appendix.Key, appendix.Page);

            var pipelineCharacteristic = new PipelineCharacteristic(new DiameterRange(0, 0, pipelineDiameterPresentation),
                new List<PipelineStandard>());

            _tcp212HelperMock.Setup(x => x.GetAppendix(appendix.Key)).Returns(appendix);
            _tcp212HelperMock
                .Setup(x => x.GetPipelineCharacteristic(appendix, pipelineMaterial, pipelineDiameter,
                    pipelineCategoryName)).Returns(pipelineCharacteristic);

            _durationByTCPEngineerMock.SetupGet(x => x.CalculationPipelineStandards)
                .Returns(calculationPipelineStandards);
            _durationByTCPEngineerMock.SetupGet(x => x.DurationCalculationType).Returns(durationCalculationType);

            _durationRounderMock.Setup(x => x.GetRoundedDuration(duration)).Returns(roundedDuration);
            _durationRounderMock.Setup(x => x.GetRoundedPreparatoryPeriod(roundedDuration)).Returns(preparatoryPeriod);

            var actualExtrapolationDuration = _durationByTCPCreator.Create(pipelineMaterial, pipelineDiameter, pipelineLength, appendix.Key, pipelineCategoryName);

            Assert.AreEqual(expectedExtrapolationDuration, actualExtrapolationDuration);

            _tcp212HelperMock.Verify(x => x.GetAppendix(appendix.Key), Times.Once);
            _tcp212HelperMock.Verify(
                x => x.GetPipelineCharacteristic(appendix, pipelineMaterial, pipelineDiameter, pipelineCategoryName),
                Times.Once);
            _durationByTCPEngineerMock.Verify(
                x => x.DefineCalculationType(pipelineCharacteristic.PipelineStandards, pipelineLength), Times.Once);

            _durationByTCPEngineerMock.VerifyGet(x => x.CalculationPipelineStandards, Times.Exactly(2));
            _durationByTCPEngineerMock.VerifyGet(x => x.DurationCalculationType, Times.Exactly(2));
            _durationRounderMock.Verify(x => x.GetRoundedDuration(duration), Times.Once);
            _durationRounderMock.Verify(x => x.GetRoundedPreparatoryPeriod(roundedDuration), Times.Once);
        }

        [Test]
        public void Create_StepwiseExtrapolationAscendingExample_CorrectDuration()
        {
            var pipelineMaterial = string.Empty;
            var pipelineDiameter = 0;
            var pipelineDiameterPresentation = "";
            var pipelineLength = 7000M;
            var appendix = TCP212.AppendixA;
            var pipelineCategoryName = "";
            var durationCalculationType = DurationCalculationType.StepwiseExtrapolationAscending;
            var duration = 13.9M;
            var roundedDuration = 14M;
            var preparatoryPeriod = 1.4M;
            var volumeChangePercent = 30M;
            var standardChangePercent = 9M;
            var stepwiseDuration = 15.3M;
            var stepwisePipelineStandard = new PipelineStandard(10000M, 15.3M, 0);

            var calculationPipelineStandards = new[]
            {
                new PipelineStandard(20000M, 18M, 0M),
            };

            var expectedStepwiseExtrapolationDuration = new StepwiseExtrapolationDurationByTCP(pipelineMaterial, pipelineDiameter, pipelineDiameterPresentation, pipelineLength,
                calculationPipelineStandards, durationCalculationType, duration, roundedDuration, preparatoryPeriod, volumeChangePercent, standardChangePercent, stepwiseDuration, stepwisePipelineStandard, appendix.Key, appendix.Page);

            var pipelineCharacteristic = new PipelineCharacteristic(new DiameterRange(0, 0, pipelineDiameterPresentation),
                new List<PipelineStandard>());

            _tcp212HelperMock.Setup(x => x.GetAppendix(appendix.Key)).Returns(appendix);
            _tcp212HelperMock
                .Setup(x => x.GetPipelineCharacteristic(appendix, pipelineMaterial, pipelineDiameter,
                    pipelineCategoryName)).Returns(pipelineCharacteristic);

            _durationByTCPEngineerMock.SetupGet(x => x.CalculationPipelineStandards)
                .Returns(calculationPipelineStandards);
            _durationByTCPEngineerMock.SetupGet(x => x.DurationCalculationType).Returns(durationCalculationType);

            _durationRounderMock.Setup(x => x.GetRoundedDuration(duration)).Returns(roundedDuration);
            _durationRounderMock.Setup(x => x.GetRoundedPreparatoryPeriod(roundedDuration)).Returns(preparatoryPeriod);

            var actualStepwiseExtrapolationDuration = _durationByTCPCreator.Create(pipelineMaterial, pipelineDiameter,
                pipelineLength, appendix.Key, pipelineCategoryName);

            Assert.AreEqual(expectedStepwiseExtrapolationDuration, actualStepwiseExtrapolationDuration);

            _tcp212HelperMock.Verify(x => x.GetAppendix(appendix.Key), Times.Once);
            _tcp212HelperMock.Verify(
                x => x.GetPipelineCharacteristic(appendix, pipelineMaterial, pipelineDiameter, pipelineCategoryName),
                Times.Once);
            _durationByTCPEngineerMock.Verify(
                x => x.DefineCalculationType(pipelineCharacteristic.PipelineStandards, pipelineLength), Times.Once);

            _durationByTCPEngineerMock.VerifyGet(x => x.CalculationPipelineStandards, Times.Exactly(2));
            _durationByTCPEngineerMock.VerifyGet(x => x.DurationCalculationType, Times.Exactly(2));
            _durationRounderMock.Verify(x => x.GetRoundedDuration(duration), Times.Once);
            _durationRounderMock.Verify(x => x.GetRoundedPreparatoryPeriod(roundedDuration), Times.Once);
        }

        [Test]
        public void Create_StepwiseExtrapolationDescendingExample_CorrectDuration()
        {
            var pipelineMaterial = string.Empty;
            var pipelineDiameter = 0;
            var pipelineDiameterPresentation = "";
            var pipelineLength = 22000M;
            var appendix = TCP212.AppendixA;
            var pipelineCategoryName = "";
            var durationCalculationType = DurationCalculationType.StepwiseExtrapolationDescending;
            var duration = 29.5M;
            var roundedDuration = 29.5M;
            var preparatoryPeriod = 3M;
            var volumeChangePercent = 10M;
            var standardChangePercent = 3M;
            var stepwiseDuration = 28.6M;
            var stepwisePipelineStandard = new PipelineStandard(20000M, 28.6M, 0);

            var calculationPipelineStandards = new[]
            {
                new PipelineStandard(10000M, 22M, 0M),
            };

            var expectedStepwiseExtrapolationDuration = new StepwiseExtrapolationDurationByTCP(pipelineMaterial, pipelineDiameter, pipelineDiameterPresentation, pipelineLength,
                calculationPipelineStandards, durationCalculationType, duration, roundedDuration, preparatoryPeriod, volumeChangePercent, standardChangePercent, stepwiseDuration, stepwisePipelineStandard, appendix.Key, appendix.Page);

            var pipelineCharacteristic = new PipelineCharacteristic(new DiameterRange(0, 0, pipelineDiameterPresentation),
                new List<PipelineStandard>());

            _tcp212HelperMock.Setup(x => x.GetAppendix(appendix.Key)).Returns(appendix);
            _tcp212HelperMock
                .Setup(x => x.GetPipelineCharacteristic(appendix, pipelineMaterial, pipelineDiameter,
                    pipelineCategoryName)).Returns(pipelineCharacteristic);

            _durationByTCPEngineerMock.SetupGet(x => x.CalculationPipelineStandards)
                .Returns(calculationPipelineStandards);
            _durationByTCPEngineerMock.SetupGet(x => x.DurationCalculationType).Returns(durationCalculationType);

            _durationRounderMock.Setup(x => x.GetRoundedDuration(duration)).Returns(roundedDuration);
            _durationRounderMock.Setup(x => x.GetRoundedPreparatoryPeriod(roundedDuration)).Returns(preparatoryPeriod);

            var actualStepwiseExtrapolationDuration = _durationByTCPCreator.Create(pipelineMaterial, pipelineDiameter,
                pipelineLength, appendix.Key, pipelineCategoryName);

            Assert.AreEqual(expectedStepwiseExtrapolationDuration, actualStepwiseExtrapolationDuration);

            _tcp212HelperMock.Verify(x => x.GetAppendix(appendix.Key), Times.Once);
            _tcp212HelperMock.Verify(
                x => x.GetPipelineCharacteristic(appendix, pipelineMaterial, pipelineDiameter, pipelineCategoryName),
                Times.Once);
            _durationByTCPEngineerMock.Verify(x =>
                x.DefineCalculationType(pipelineCharacteristic.PipelineStandards, pipelineLength), Times.Once);

            _durationByTCPEngineerMock.VerifyGet(x => x.CalculationPipelineStandards, Times.Exactly(2));
            _durationByTCPEngineerMock.VerifyGet(x => x.DurationCalculationType, Times.Exactly(2));
            _durationRounderMock.Verify(x => x.GetRoundedDuration(duration), Times.Once);
            _durationRounderMock.Verify(x => x.GetRoundedPreparatoryPeriod(roundedDuration), Times.Once);
        }
    }
}

