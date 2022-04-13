using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Calabonga.OperationResults;
using Moq;
using POS.Infrastructure.Calculators;
using POS.Models.CalendarPlanModels;
using POS.Models.EstimateModels;
using POS.Tests.Helpers.Calculators;
using Xunit;

namespace POS.Tests.Infrastructure.Calculators
{
    public class CalendarWorkCalculatorTests
    {
        [Fact]
        public async Task ItShould_calculate_correct_calendar_work_from_estimate_work()
        {
            // arrange

            var constructionStartDate = new DateTime(1999, 9, 21);
            var estimateWork = new EstimateWork
            {
                TotalCost = 2.628M,
                EquipmentCost = 0.2M,
                OtherProductsCost = 0.3M,
                Chapter = 2,
                Percentages = new List<decimal>()
                {
                    0.2M, 0, 0.3M, 0.5M
                },
                WorkName = "Электрохимическая защита"
            };

            var constructionMonths = new List<ConstructionMonth>
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

            var constructionMonthsCalculator = ConstructionMonthsCalculatorHelper.GetMock();

            constructionMonthsCalculator
                .Setup(x => x.Calculate(estimateWork.TotalCost, 2.128M, constructionStartDate, estimateWork.Percentages))
                .ReturnsAsync(new OperationResult<IEnumerable<ConstructionMonth>> { Result = constructionMonths });

            var expectedCalendarWork = new CalendarWork
            {
                TotalCost = 2.628M,
                TotalCostIncludingCAIW = 2.128M,
                WorkName = "Электрохимическая защита",
                EstimateChapter = 2,
                ConstructionMonths = constructionMonths
            };


            var sut = new CalendarWorkCalculator(constructionMonthsCalculator.Object);

            // act

            var operation = await sut.Calculate(estimateWork, constructionStartDate);

            // assert

            Assert.True(operation.Ok);

            Assert.Equal(expectedCalendarWork, operation.Result);
        }
    }
}