using System.Linq;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Engineers;
using POS.Models.DurationByTCPModels;
using POS.Models.DurationByTCPModels.TCPModels;
using Xunit;

namespace POS.Tests.Infrastructure.Engineers
{
    public class DurationByTCPEngineerTests
    {
        [Fact]
        public void
            ItShould_define_calculation_type_when_pipeline_length_below_standards_with_first_standard_and_extrapolation_ascending()
        {
            // arrange

            var pipelineLength = 0.05M;

            var pipelineStandards = new[]
            {
                new PipelineStandard(0.1M, 1M, 0.3M),
                new PipelineStandard(0.5M, 2M, 0.3M),
                new PipelineStandard(1M, 2.5M, 0.3M),
                new PipelineStandard(1.5M, 4M, 0.5M),
            };

            var expectedCalculationPipelineStandards = new[]
            {
                new PipelineStandard(0.1M, 1M, 0.3M),
            };

            var expectedDurationCalculationType = DurationCalculationType.ExtrapolationAscending;

            var sut = new DurationByTCPEngineer();

            // act

            sut.DefineCalculationType(pipelineStandards, pipelineLength);

            // assert

            Assert.Equal(expectedDurationCalculationType, sut.DurationCalculationType);
            Assert.True(sut.CalculationPipelineStandards.SequenceEqual(expectedCalculationPipelineStandards));
        }

        [Fact]
        public void
            ItShould_define_calculation_type_when_pipeline_length_below_standards_with_first_standard_and_extrapolation_stepwise_ascending()
        {
            // arrange

            var pipelineLength = 0.049M;

            var pipelineStandards = new[]
            {
                new PipelineStandard(0.1M, 1M, 0.3M),
                new PipelineStandard(0.5M, 2M, 0.3M),
                new PipelineStandard(1M, 2.5M, 0.3M),
                new PipelineStandard(1.5M, 4M, 0.5M),
            };

            var expectedCalculationPipelineStandards = new[]
            {
                new PipelineStandard(0.1M, 1M, 0.3M),
            };

            var expectedDurationCalculationType = DurationCalculationType.StepwiseExtrapolationAscending;

            var sut = new DurationByTCPEngineer();

            // act

            sut.DefineCalculationType(pipelineStandards, pipelineLength);

            // assert

            Assert.Equal(expectedDurationCalculationType, sut.DurationCalculationType);
            Assert.True(sut.CalculationPipelineStandards.SequenceEqual(expectedCalculationPipelineStandards));
        }

        [Fact]
        public void
            ItShould_define_calculation_type_when_pipeline_length_above_standards_with_last_standard_and_extrapolation_ascending()
        {
            // arrange

            var pipelineLength = 3M;

            var pipelineStandards = new[]
            {
                new PipelineStandard(0.1M, 1M, 0.3M),
                new PipelineStandard(0.5M, 2M, 0.3M),
                new PipelineStandard(1M, 2.5M, 0.3M),
                new PipelineStandard(1.5M, 4M, 0.5M),
            };

            var expectedCalculationPipelineStandards = new[]
            {
                new PipelineStandard(1.5M, 4M, 0.5M),
            };

            var expectedDurationCalculationType = DurationCalculationType.ExtrapolationDescending;

            var sut = new DurationByTCPEngineer();

            // act

            sut.DefineCalculationType(pipelineStandards, pipelineLength);

            // assert

            Assert.Equal(expectedDurationCalculationType, sut.DurationCalculationType);
            Assert.True(sut.CalculationPipelineStandards.SequenceEqual(expectedCalculationPipelineStandards));
        }

        [Fact]
        public void
            ItShould_define_calculation_type_when_pipeline_length_above_standards_with_last_standard_and_extrapolation_stepwise_ascending()
        {
            // arrange

            var pipelineLength = 3.01M;

            var pipelineStandards = new[]
            {
                new PipelineStandard(0.1M, 1M, 0.3M),
                new PipelineStandard(0.5M, 2M, 0.3M),
                new PipelineStandard(1M, 2.5M, 0.3M),
                new PipelineStandard(1.5M, 4M, 0.5M),
            };

            var expectedCalculationPipelineStandards = new[]
            {
                new PipelineStandard(1.5M, 4M, 0.5M),
            };

            var expectedDurationCalculationType = DurationCalculationType.StepwiseExtrapolationDescending;

            var sut = new DurationByTCPEngineer();

            // act

            sut.DefineCalculationType(pipelineStandards, pipelineLength);

            // assert

            Assert.Equal(expectedDurationCalculationType, sut.DurationCalculationType);
            Assert.True(sut.CalculationPipelineStandards.SequenceEqual(expectedCalculationPipelineStandards));
        }

        [Fact]
        public void
            ItShould_define_calculation_type_when_pipeline_length_between_standards_with_previous_and_next_standards_and_inteplotaion()
        {
            // arrange

            var pipelineLength = 0.25M;

            var pipelineStandards = new[]
            {
                new PipelineStandard(0.1M, 1M, 0.3M),
                new PipelineStandard(0.5M, 2M, 0.3M),
                new PipelineStandard(1M, 2.5M, 0.3M),
                new PipelineStandard(1.5M, 4M, 0.5M),
            };

            var expectedCalculationPipelineStandards = new[]
            {
                new PipelineStandard(0.1M, 1M, 0.3M),
                new PipelineStandard(0.5M, 2M, 0.3M),
            };

            var expectedDurationCalculationType = DurationCalculationType.Interpolation;

            var sut = new DurationByTCPEngineer();

            // act

            sut.DefineCalculationType(pipelineStandards, pipelineLength);

            // assert

            Assert.Equal(expectedDurationCalculationType, sut.DurationCalculationType);
            Assert.True(sut.CalculationPipelineStandards.SequenceEqual(expectedCalculationPipelineStandards));
        }

        [Fact]
        public void GetAppendix_AKey_AppendixA()
        {
            // arrange

            var appendixKey = 'A';
            var expectedAppendix = Constants.AppendixA;

            var sut = new DurationByTCPEngineer();

            // act

            var actualAppendix = sut.GetAppendix(appendixKey);

            // assert

            Assert.Equal(expectedAppendix, actualAppendix);
        }

        [Fact]
        public void GetAppendix_BKey_AppendixB()
        {
            // arrange

            var appendixKey = 'B';
            var expectedAppendix = Constants.AppendixB;

            var sut = new DurationByTCPEngineer();

            // act

            var actualAppendix = sut.GetAppendix(appendixKey);

            // assert

            Assert.Equal(expectedAppendix, actualAppendix);
        }

        [Fact]
        public void GetPipelineCharacteristic_UrbanSlopesSteelPipelineWith300Diameter_CorrectPipelineCharacteristic()
        {
            // arrange

            var pipelineMaterial = "стальных труб";
            var pipelineDiameter = 300;
            var appendix = Constants.AppendixA;
            var pipelineCategoryName =
                "Уличные трубопроводы водо-, газоснабжения и канализации, сооружаемые в траншеях с откосами";

            var expectedPipelineCharacteristic = new PipelineCharacteristic(new DiameterRange(0, 500, "до 500"), new[]
            {
                new PipelineStandard(0.1M, 1M, 0.3M),
                new PipelineStandard(0.5M, 2M, 0.3M),
                new PipelineStandard(1M, 2.5M, 0.3M),
                new PipelineStandard(1.5M, 4M, 0.5M),
            });

            var sut = new DurationByTCPEngineer();

            // act

            var actualPipelineCharacteristic = sut.GetPipelineCharacteristic(appendix, pipelineMaterial,
                pipelineDiameter, pipelineCategoryName);

            // assert

            Assert.Equal(expectedPipelineCharacteristic, actualPipelineCharacteristic);
        }

        [Fact]
        public void GetPipelineCharacteristic_UrbanFastenersSteelPipelineWith300Diameter_CorrectPipelineCharacteristic()
        {
            // arrange

            var pipelineMaterial = "стальных труб";
            var pipelineDiameter = 300;
            var appendix = Constants.AppendixA;
            var pipelineCategoryName =
                "Уличные трубопроводы водо-, газоснабжения и канализации, сооружаемые в траншеях с креплением стенок";

            var expectedPipelineCharacteristic = new PipelineCharacteristic(new DiameterRange(0, 500, "до 500"), new[]
            {
                new PipelineStandard(0.1M, 1.5M, 0.3M),
                new PipelineStandard(0.5M, 2.5M, 0.3M),
                new PipelineStandard(1M, 3.5M, 0.3M),
                new PipelineStandard(1.5M, 5.5M, 0.5M),
            });

            var sut = new DurationByTCPEngineer();

            // act

            var actualPipelineCharacteristic = sut.GetPipelineCharacteristic(appendix, pipelineMaterial,
                pipelineDiameter, pipelineCategoryName);

            // assert

            Assert.Equal(expectedPipelineCharacteristic, actualPipelineCharacteristic);
        }

        [Fact]
        public void GetPipelineCharacteristic_CountrySteelPipelineWith300Diameter_CorrectPipelineCharacteristic()
        {
            // arrange

            var pipelineMaterial = "стальных труб";
            var pipelineDiameter = 300;
            var appendix = Constants.AppendixB;
            var pipelineCategoryName = "Наружные трубопроводы";

            var expectedPipelineCharacteristic = new PipelineCharacteristic(new DiameterRange(0, 400, "до 400"), new[]
            {
                new PipelineStandard(1M, 2M, 0M),
                new PipelineStandard(2M, 3M, 0M),
                new PipelineStandard(5M, 4M, 0M),
                new PipelineStandard(10M, 6M, 0M),
            });

            var sut = new DurationByTCPEngineer();

            // act

            var actualPipelineCharacteristic = sut.GetPipelineCharacteristic(appendix, pipelineMaterial,
                pipelineDiameter, pipelineCategoryName);

            // assert

            Assert.Equal(expectedPipelineCharacteristic, actualPipelineCharacteristic);
        }
    }
}