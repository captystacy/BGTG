using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POS.Infrastructure.Calculators;
using POS.Models.CalendarPlanModels;
using Xunit;

namespace POS.Tests.Infrastructure.Calculators
{
    public class ConstructionMonthsCalculatorTests
    {
        [Fact]
        public async Task ItShould_calculate_correct_construction_period()
        {
            // arrange

            var totalCost = 2.628M;
            var totalCostIncludingCAIW = 2.128M;
            var percentages = new List<decimal>()
            {
                0.2M, 0, 0.3M, 0.5M
            };
            var constructionStartDate = new DateTime(1999, 9, 21);

            var expected = new List<ConstructionMonth>
            {
                new()
                {
                    CreationIndex = 0,
                    PercentPart = 0.2M,
                    Date = new DateTime(1999, 9, 21),
                    InvestmentVolume = 0.5256M,
                    VolumeCAIW = 0.4256M,
                },
                new()
                {
                    CreationIndex = 2,
                    PercentPart = 0.3M,
                    Date = new DateTime(1999, 11, 21),
                    InvestmentVolume = 0.7884M,
                    VolumeCAIW = 0.6384M,
                },
                new()
                {
                    CreationIndex = 3,
                    PercentPart = 0.5M,
                    Date = new DateTime(1999, 12, 21),
                    InvestmentVolume = 1.314M,
                    VolumeCAIW = 1.064M,
                },
            };

            var sut = new ConstructionMonthsCalculator();

            // act

            var calculateOperation = await sut.Calculate(totalCost, totalCostIncludingCAIW, constructionStartDate, percentages);

            var actual = calculateOperation.Result.ToList();

            // assert

            Assert.True(calculateOperation.Ok);

            Assert.Equal(expected.Count, actual.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i], actual[i]);
            }
        }
    }
}