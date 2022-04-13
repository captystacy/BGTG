using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Calculators;
using POS.Models.DurationByTCPModels;
using POS.Models.DurationByTCPModels.TCPModels;
using POS.Tests.Helpers.Engineers;
using POS.Tests.Helpers.Rounders;
using POS.ViewModels;
using Xunit;

namespace POS.Tests.Infrastructure.Calculators
{
    public class DurationByTCPCalculatorTests
    {
        [Fact]
        public async Task ItShould_calculate_correct_interpolation_duration_from_tcp_example()
        {
            // arrange

            var pipelineDiameterPresentation = string.Empty;
            var appendix = Constants.AppendixA;
            var durationCalculationType = DurationCalculationType.Interpolation;
            var duration = 10.8M;
            var roundedDuration = 11M;
            var preparatoryPeriod = 1.1M;
            var durationChange = 0.6M;
            var volumeChange = 3M;

            var viewModel = new DurationByTCPViewModel
            {
                PipelineLength = 13M,
                AppendixKey = appendix.Key,
                PipelineCategoryName = string.Empty,
                PipelineDiameter = 0,
                PipelineMaterial = string.Empty
            };

            var calculationPipelineStandards = new[]
            {
                new PipelineStandard(10M, 9M, 0M),
                new PipelineStandard(15M, 12M, 0M),
            };

            var expectedInterpolationDuration = new InterpolationDurationByTCP
            {
                Duration = duration,
                PreparatoryPeriod = preparatoryPeriod,
                PipelineLength = viewModel.PipelineLength,
                RoundedDuration = roundedDuration,
                PipelineDiameter = viewModel.PipelineDiameter,
                PipelineDiameterPresentation = pipelineDiameterPresentation,
                CalculationPipelineStandards = calculationPipelineStandards,
                DurationCalculationType = durationCalculationType,
                AppendixKey = appendix.Key,
                AppendixPage = appendix.Page,
                PipelineMaterial = viewModel.PipelineMaterial,
                DurationChange = durationChange,
                VolumeChange = volumeChange
            };

            var pipelineCharacteristic = new PipelineCharacteristic(new DiameterRange(0, 0, pipelineDiameterPresentation),
                new List<PipelineStandard>());

            var durationByTCPEngineerMock = DurationByTCPEngineerHelper.GetMock();

            durationByTCPEngineerMock.Setup(x => x.GetAppendix(appendix.Key)).Returns(appendix);
            durationByTCPEngineerMock
                .Setup(x => x.GetPipelineCharacteristic(appendix, viewModel.PipelineMaterial, viewModel.PipelineDiameter,
                    viewModel.PipelineCategoryName)).Returns(pipelineCharacteristic);

            durationByTCPEngineerMock.SetupGet(x => x.CalculationPipelineStandards)
                .Returns(calculationPipelineStandards);
            durationByTCPEngineerMock.SetupGet(x => x.DurationCalculationType).Returns(durationCalculationType);

            var durationRounderMock = DurationRounderHelper.GetMock();

            durationRounderMock.Setup(x => x.GetRoundedDuration(duration)).ReturnsAsync(roundedDuration);
            durationRounderMock.Setup(x => x.GetRoundedPreparatoryPeriod(roundedDuration)).ReturnsAsync(preparatoryPeriod);

            var sut = new DurationByTCPCalculator(durationByTCPEngineerMock.Object, durationRounderMock.Object);

            // act

            var calculateOperation = await sut.Calculate(viewModel);

            // assert

            Assert.True(calculateOperation.Ok);

            Assert.Equal(expectedInterpolationDuration, calculateOperation.Result);

            durationByTCPEngineerMock.Verify(x => x.GetAppendix(appendix.Key), Times.Once);
            durationByTCPEngineerMock.Verify(
                x => x.GetPipelineCharacteristic(appendix, viewModel.PipelineMaterial, viewModel.PipelineDiameter, viewModel.PipelineCategoryName),
                Times.Once);
            durationByTCPEngineerMock.Verify(
                x => x.DefineCalculationType(pipelineCharacteristic.PipelineStandards, viewModel.PipelineLength), Times.Once);

            durationByTCPEngineerMock.VerifyGet(x => x.CalculationPipelineStandards, Times.Exactly(2));
            durationByTCPEngineerMock.VerifyGet(x => x.DurationCalculationType, Times.Exactly(2));
            durationRounderMock.Verify(x => x.GetRoundedDuration(duration), Times.Once);
            durationRounderMock.Verify(x => x.GetRoundedPreparatoryPeriod(roundedDuration), Times.Once);
        }

        [Fact]
        public async Task ItShould_calculate_correct_extrapolation_ascending_duration_from_tcp_example()
        {
            // arrange

            var pipelineDiameterPresentation = string.Empty;
            var appendix = Constants.AppendixA;
            var durationCalculationType = DurationCalculationType.ExtrapolationAscending;
            var duration = 11.4M;
            var roundedDuration = 11.5M;
            var preparatoryPeriod = 1.1M;
            var volumeChangePercent = 16.7M;
            var standardChangePercent = 5M;

            var viewModel = new DurationByTCPViewModel
            {
                PipelineLength = 25000M,
                AppendixKey = appendix.Key,
                PipelineCategoryName = string.Empty,
                PipelineDiameter = 0,
                PipelineMaterial = string.Empty
            };

            var calculationPipelineStandards = new[]
            {
                new PipelineStandard(30000M, 12M, 0M),
            };

            var expectedExtrapolationDuration = new ExtrapolationDurationByTCP
            {
                Duration = duration,
                PreparatoryPeriod = preparatoryPeriod,
                PipelineLength = viewModel.PipelineLength,
                RoundedDuration = roundedDuration,
                PipelineDiameter = viewModel.PipelineDiameter,
                PipelineDiameterPresentation = pipelineDiameterPresentation,
                CalculationPipelineStandards = calculationPipelineStandards,
                DurationCalculationType = durationCalculationType,
                AppendixKey = appendix.Key,
                AppendixPage = appendix.Page,
                PipelineMaterial = viewModel.PipelineMaterial,
                StandardChangePercent = standardChangePercent,
                VolumeChangePercent = volumeChangePercent
            };

            var pipelineCharacteristic = new PipelineCharacteristic(new DiameterRange(0, 0, pipelineDiameterPresentation),
                new List<PipelineStandard>());

            var durationByTCPEngineerMock = DurationByTCPEngineerHelper.GetMock();

            durationByTCPEngineerMock.Setup(x => x.GetAppendix(appendix.Key)).Returns(appendix);
            durationByTCPEngineerMock
                .Setup(x => x.GetPipelineCharacteristic(appendix, viewModel.PipelineMaterial, viewModel.PipelineDiameter,
                    viewModel.PipelineCategoryName)).Returns(pipelineCharacteristic);

            durationByTCPEngineerMock.SetupGet(x => x.CalculationPipelineStandards)
                .Returns(calculationPipelineStandards);

            durationByTCPEngineerMock.SetupGet(x => x.DurationCalculationType).Returns(durationCalculationType);

            var durationRounderMock = DurationRounderHelper.GetMock();

            durationRounderMock.Setup(x => x.GetRoundedDuration(duration)).ReturnsAsync(roundedDuration);
            durationRounderMock.Setup(x => x.GetRoundedPreparatoryPeriod(roundedDuration)).ReturnsAsync(preparatoryPeriod);

            var sut = new DurationByTCPCalculator(durationByTCPEngineerMock.Object, durationRounderMock.Object);

            // act

            var calculateOperation = await sut.Calculate(viewModel);

            // assert

            Assert.True(calculateOperation.Ok);

            Assert.Equal(expectedExtrapolationDuration, calculateOperation.Result);

            durationByTCPEngineerMock.Verify(x => x.GetAppendix(appendix.Key), Times.Once);
            durationByTCPEngineerMock.Verify(
                x => x.GetPipelineCharacteristic(appendix, viewModel.PipelineMaterial, viewModel.PipelineDiameter, viewModel.PipelineCategoryName),
                Times.Once);

            durationByTCPEngineerMock.Verify(
                x => x.DefineCalculationType(pipelineCharacteristic.PipelineStandards, viewModel.PipelineLength), Times.Once);
            durationByTCPEngineerMock.VerifyGet(x => x.CalculationPipelineStandards, Times.Exactly(2));
            durationByTCPEngineerMock.VerifyGet(x => x.DurationCalculationType, Times.Exactly(2));
            durationRounderMock.Verify(x => x.GetRoundedDuration(duration), Times.Once);
            durationRounderMock.Verify(x => x.GetRoundedPreparatoryPeriod(roundedDuration), Times.Once);
        }

        [Fact]
        public async Task ItShould_calculate_correct_extrapolation_descending_duration_from_tcp_example()
        {
            // arrange

            var pipelineDiameterPresentation = string.Empty;
            var appendix = Constants.AppendixA;
            var durationCalculationType = DurationCalculationType.ExtrapolationDescending;
            var duration = 15.1M;
            var roundedDuration = 15M;
            var preparatoryPeriod = 1.5M;
            var volumeChangePercent = 25.4M;
            var standardChangePercent = 7.6M;

            var viewModel = new DurationByTCPViewModel
            {
                PipelineLength = 62700M,
                AppendixKey = appendix.Key,
                PipelineCategoryName = string.Empty,
                PipelineDiameter = 0,
                PipelineMaterial = string.Empty
            };

            var calculationPipelineStandards = new[]
            {
                new PipelineStandard(50000M, 14M, 0M),
            };

            var expectedExtrapolationDuration = new ExtrapolationDurationByTCP
            {
                Duration = duration,
                PreparatoryPeriod = preparatoryPeriod,
                PipelineLength = viewModel.PipelineLength,
                RoundedDuration = roundedDuration,
                PipelineDiameter = viewModel.PipelineDiameter,
                PipelineDiameterPresentation = pipelineDiameterPresentation,
                CalculationPipelineStandards = calculationPipelineStandards,
                DurationCalculationType = durationCalculationType,
                AppendixKey = appendix.Key,
                AppendixPage = appendix.Page,
                PipelineMaterial = viewModel.PipelineMaterial,
                StandardChangePercent = standardChangePercent,
                VolumeChangePercent = volumeChangePercent
            };

            var pipelineCharacteristic = new PipelineCharacteristic(new DiameterRange(0, 0, pipelineDiameterPresentation),
                new List<PipelineStandard>());

            var durationByTCPEngineerMock = DurationByTCPEngineerHelper.GetMock();

            durationByTCPEngineerMock.Setup(x => x.GetAppendix(appendix.Key)).Returns(appendix);
            durationByTCPEngineerMock
                .Setup(x => x.GetPipelineCharacteristic(appendix, viewModel.PipelineMaterial, viewModel.PipelineDiameter,
                    viewModel.PipelineCategoryName)).Returns(pipelineCharacteristic);

            durationByTCPEngineerMock.SetupGet(x => x.CalculationPipelineStandards)
                .Returns(calculationPipelineStandards);
            durationByTCPEngineerMock.SetupGet(x => x.DurationCalculationType).Returns(durationCalculationType);

            var durationRounderMock = DurationRounderHelper.GetMock();

            durationRounderMock.Setup(x => x.GetRoundedDuration(duration)).ReturnsAsync(roundedDuration);
            durationRounderMock.Setup(x => x.GetRoundedPreparatoryPeriod(roundedDuration)).ReturnsAsync(preparatoryPeriod);

            var sut = new DurationByTCPCalculator(durationByTCPEngineerMock.Object, durationRounderMock.Object);

            // act

            var calculateOperation = await sut.Calculate(viewModel);

            // assert

            Assert.True(calculateOperation.Ok);

            Assert.Equal(expectedExtrapolationDuration, calculateOperation.Result);

            durationByTCPEngineerMock.Verify(x => x.GetAppendix(appendix.Key), Times.Once);
            durationByTCPEngineerMock.Verify(
                x => x.GetPipelineCharacteristic(appendix, viewModel.PipelineMaterial, viewModel.PipelineDiameter, viewModel.PipelineCategoryName),
                Times.Once);

            durationByTCPEngineerMock.Verify(
                x => x.DefineCalculationType(pipelineCharacteristic.PipelineStandards, viewModel.PipelineLength), Times.Once);

            durationByTCPEngineerMock.VerifyGet(x => x.CalculationPipelineStandards, Times.Exactly(2));
            durationByTCPEngineerMock.VerifyGet(x => x.DurationCalculationType, Times.Exactly(2));
            durationRounderMock.Verify(x => x.GetRoundedDuration(duration), Times.Once);
            durationRounderMock.Verify(x => x.GetRoundedPreparatoryPeriod(roundedDuration), Times.Once);
        }

        [Fact]
        public async Task ItShould_calculate_correct_stepwise_extrapolation_ascending_duration_from_tcp_example()
        {
            // arrange

            var pipelineDiameterPresentation = string.Empty;
            var appendix = Constants.AppendixA;
            var durationCalculationType = DurationCalculationType.StepwiseExtrapolationAscending;
            var duration = 13.9M;
            var roundedDuration = 14M;
            var preparatoryPeriod = 1.4M;
            var volumeChangePercent = 30M;
            var standardChangePercent = 9M;
            var stepwiseDuration = 15.3M;
            var stepwisePipelineStandard = new PipelineStandard(10000M, 15.3M, 0);

            var viewModel = new DurationByTCPViewModel
            {
                PipelineLength = 7000M,
                AppendixKey = appendix.Key,
                PipelineCategoryName = string.Empty,
                PipelineDiameter = 0,
                PipelineMaterial = string.Empty
            };

            var calculationPipelineStandards = new[]
            {
                new PipelineStandard(20000M, 18M, 0M),
            };

            var expectedStepwiseExtrapolationDuration = new StepwiseExtrapolationDurationByTCP
            {
                Duration = duration,
                PreparatoryPeriod = preparatoryPeriod,
                PipelineLength = viewModel.PipelineLength,
                RoundedDuration = roundedDuration,
                PipelineDiameter = viewModel.PipelineDiameter,
                PipelineDiameterPresentation = pipelineDiameterPresentation,
                CalculationPipelineStandards = calculationPipelineStandards,
                DurationCalculationType = durationCalculationType,
                AppendixKey = appendix.Key,
                AppendixPage = appendix.Page,
                PipelineMaterial = viewModel.PipelineMaterial,
                StandardChangePercent = standardChangePercent,
                VolumeChangePercent = volumeChangePercent,
                StepwisePipelineStandard = stepwisePipelineStandard,
                StepwiseDuration = stepwiseDuration
            };

            var pipelineCharacteristic = new PipelineCharacteristic(new DiameterRange(0, 0, pipelineDiameterPresentation),
                new List<PipelineStandard>());

            var durationByTCPEngineerMock = DurationByTCPEngineerHelper.GetMock();

            durationByTCPEngineerMock.Setup(x => x.GetAppendix(appendix.Key)).Returns(appendix);
            durationByTCPEngineerMock
                .Setup(x => x.GetPipelineCharacteristic(appendix, viewModel.PipelineMaterial, viewModel.PipelineDiameter,
                    viewModel.PipelineCategoryName)).Returns(pipelineCharacteristic);

            durationByTCPEngineerMock.SetupGet(x => x.CalculationPipelineStandards)
                .Returns(calculationPipelineStandards);
            durationByTCPEngineerMock.SetupGet(x => x.DurationCalculationType).Returns(durationCalculationType);

            var durationRounderMock = DurationRounderHelper.GetMock();

            durationRounderMock.Setup(x => x.GetRoundedDuration(duration)).ReturnsAsync(roundedDuration);
            durationRounderMock.Setup(x => x.GetRoundedPreparatoryPeriod(roundedDuration)).ReturnsAsync(preparatoryPeriod);

            var sut = new DurationByTCPCalculator(durationByTCPEngineerMock.Object, durationRounderMock.Object);

            // act

            var calculateOperation = await sut.Calculate(viewModel);

            // assert

            Assert.True(calculateOperation.Ok);

            Assert.Equal(expectedStepwiseExtrapolationDuration, calculateOperation.Result);

            durationByTCPEngineerMock.Verify(x => x.GetAppendix(appendix.Key), Times.Once);
            durationByTCPEngineerMock.Verify(
                x => x.GetPipelineCharacteristic(appendix, viewModel.PipelineMaterial, viewModel.PipelineDiameter, viewModel.PipelineCategoryName),
                Times.Once);

            durationByTCPEngineerMock.Verify(
                x => x.DefineCalculationType(pipelineCharacteristic.PipelineStandards, viewModel.PipelineLength), Times.Once);

            durationByTCPEngineerMock.VerifyGet(x => x.CalculationPipelineStandards, Times.Exactly(2));
            durationByTCPEngineerMock.VerifyGet(x => x.DurationCalculationType, Times.Exactly(2));
            durationRounderMock.Verify(x => x.GetRoundedDuration(duration), Times.Once);
            durationRounderMock.Verify(x => x.GetRoundedPreparatoryPeriod(roundedDuration), Times.Once);
        }

        [Fact]
        public async Task ItShould_calculate_correct_stepwise_extrapolation_descending_duration_from_tcp_example()
        {
            // arrange

            var pipelineDiameterPresentation = string.Empty;
            var appendix = Constants.AppendixA;
            var durationCalculationType = DurationCalculationType.StepwiseExtrapolationDescending;
            var duration = 29.5M;
            var roundedDuration = 29.5M;
            var preparatoryPeriod = 3M;
            var volumeChangePercent = 10M;
            var standardChangePercent = 3M;
            var stepwiseDuration = 28.6M;
            var stepwisePipelineStandard = new PipelineStandard(20000M, 28.6M, 0);

            var viewModel = new DurationByTCPViewModel
            {
                PipelineLength = 22000M,
                AppendixKey = appendix.Key,
                PipelineCategoryName = string.Empty,
                PipelineDiameter = 0,
                PipelineMaterial = string.Empty
            };

            var calculationPipelineStandards = new[]
            {
                new PipelineStandard(10000M, 22M, 0M),
            };

            var expectedStepwiseExtrapolationDuration = new StepwiseExtrapolationDurationByTCP
            {
                Duration = duration,
                PreparatoryPeriod = preparatoryPeriod,
                PipelineLength = viewModel.PipelineLength,
                RoundedDuration = roundedDuration,
                PipelineDiameter = viewModel.PipelineDiameter,
                PipelineDiameterPresentation = pipelineDiameterPresentation,
                CalculationPipelineStandards = calculationPipelineStandards,
                DurationCalculationType = durationCalculationType,
                AppendixKey = appendix.Key,
                AppendixPage = appendix.Page,
                PipelineMaterial = viewModel.PipelineMaterial,
                StandardChangePercent = standardChangePercent,
                VolumeChangePercent = volumeChangePercent,
                StepwisePipelineStandard = stepwisePipelineStandard,
                StepwiseDuration = stepwiseDuration
            };

            var pipelineCharacteristic = new PipelineCharacteristic(new DiameterRange(0, 0, pipelineDiameterPresentation),
                new List<PipelineStandard>());

            var durationByTCPEngineerMock = DurationByTCPEngineerHelper.GetMock();

            durationByTCPEngineerMock.Setup(x => x.GetAppendix(appendix.Key)).Returns(appendix);
            durationByTCPEngineerMock
                .Setup(x => x.GetPipelineCharacteristic(appendix, viewModel.PipelineMaterial, viewModel.PipelineDiameter,
                    viewModel.PipelineCategoryName)).Returns(pipelineCharacteristic);

            durationByTCPEngineerMock.SetupGet(x => x.CalculationPipelineStandards)
                .Returns(calculationPipelineStandards);
            durationByTCPEngineerMock.SetupGet(x => x.DurationCalculationType).Returns(durationCalculationType);

            var durationRounderMock = DurationRounderHelper.GetMock();

            durationRounderMock.Setup(x => x.GetRoundedDuration(duration)).ReturnsAsync(roundedDuration);
            durationRounderMock.Setup(x => x.GetRoundedPreparatoryPeriod(roundedDuration)).ReturnsAsync(preparatoryPeriod);

            var sut = new DurationByTCPCalculator(durationByTCPEngineerMock.Object, durationRounderMock.Object);

            // act

            var calculateOperation = await sut.Calculate(viewModel);

            // assert

            Assert.True(calculateOperation.Ok);

            Assert.Equal(expectedStepwiseExtrapolationDuration, calculateOperation.Result);

            durationByTCPEngineerMock.Verify(x => x.GetAppendix(appendix.Key), Times.Once);
            durationByTCPEngineerMock.Verify(
                x => x.GetPipelineCharacteristic(appendix, viewModel.PipelineMaterial, viewModel.PipelineDiameter, viewModel.PipelineCategoryName),
                Times.Once);

            durationByTCPEngineerMock.Verify(x =>
                x.DefineCalculationType(pipelineCharacteristic.PipelineStandards, viewModel.PipelineLength), Times.Once);

            durationByTCPEngineerMock.VerifyGet(x => x.CalculationPipelineStandards, Times.Exactly(2));
            durationByTCPEngineerMock.VerifyGet(x => x.DurationCalculationType, Times.Exactly(2));
            durationRounderMock.Verify(x => x.GetRoundedDuration(duration), Times.Once);
            durationRounderMock.Verify(x => x.GetRoundedPreparatoryPeriod(roundedDuration), Times.Once);
        }
    }
}