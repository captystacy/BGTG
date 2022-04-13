using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using POS.Infrastructure.Connectors;
using POS.Models.EstimateModels;
using POS.Tests.Helpers.Connectors;
using Xunit;

namespace POS.Tests.Infrastructure.Connectors
{
    public class EstimateConnectorTests
    {
        [Fact]
        public async Task ItShould_return_estimate_if_was_single()
        {
            // arrange

            var expectedEstimate = new Estimate();

            var sut = new EstimateConnector();

            // act

            var connectOperation = await sut.Connect(new List<Estimate> { expectedEstimate });

            // assert

            Assert.Same(expectedEstimate, connectOperation.Result);
        }

        [Fact]
        public async Task ItShould_sum_estimate_works_with_same_name()
        {
            // arrange

            var sameWorkName = "Электрохимическая защита";

            var firstEstimateWork = new EstimateWork()
            {
                Percentages = new List<decimal>(),
                Chapter = 2,
                EquipmentCost = 1.2M,
                OtherProductsCost = 1.3M,
                TotalCost = 1.4M,
                WorkName = sameWorkName
            };

            var secondEstimateWork = new EstimateWork()
            {
                Percentages = new List<decimal>(),
                Chapter = 2,
                EquipmentCost = 1.5M,
                OtherProductsCost = 1.6M,
                TotalCost = 1.7M,
                WorkName = sameWorkName
            };

            var expectedEstimateWork = new EstimateWork
            {
                Percentages = new List<decimal>(),
                Chapter = 2,
                EquipmentCost = 2.7M,
                OtherProductsCost = 2.9M,
                TotalCost = 3.1M,
                WorkName = sameWorkName
            };

            var fixture = new Fixture();

            var firstEstimate = fixture
                .Build<Estimate>()
                .With(x => x.PreparatoryEstimateWorks, new List<EstimateWork> { firstEstimateWork })
                .Create();

            var secondEstimate = fixture
                .Build<Estimate>()
                .With(x => x.PreparatoryEstimateWorks, new List<EstimateWork> { secondEstimateWork })
                .Create();

            var sut = new EstimateConnector();

            // act

            var connectOperation = await sut.Connect(new List<Estimate> { firstEstimate, secondEstimate });

            var actualEstimateWork = connectOperation.Result.PreparatoryEstimateWorks.First();

            // assert

            Assert.True(connectOperation.Ok);

            Assert.Equal(expectedEstimateWork, actualEstimateWork);
        }

        [Fact]
        public async Task ItShould_connect_estimate_work()
        {
            // arrange

            var sameWorkName = "Электрохимическая защита";

            var firstEstimateWork = new EstimateWork()
            {
                Percentages = new List<decimal>(),
                Chapter = 2,
                EquipmentCost = 1.2M,
                OtherProductsCost = 1.3M,
                TotalCost = 1.4M,
                WorkName = sameWorkName
            };

            var secondEstimateWork = new EstimateWork()
            {
                Percentages = new List<decimal>(),
                Chapter = 2,
                EquipmentCost = 1.5M,
                OtherProductsCost = 1.6M,
                TotalCost = 1.7M,
                WorkName = sameWorkName
            };

            var expected = new EstimateWork
            {
                Percentages = new List<decimal>(),
                Chapter = 2,
                EquipmentCost = 2.7M,
                OtherProductsCost = 2.9M,
                TotalCost = 3.1M,
                WorkName = sameWorkName
            };

            var sut = new EstimateConnector();

            // act

            var connectOperation = await sut.ConnectEstimateWork(new List<EstimateWork> { firstEstimateWork, secondEstimateWork });

            // assert

            Assert.True(connectOperation.Ok);

            Assert.Equal(expected, connectOperation.Result);
        }
    }
}