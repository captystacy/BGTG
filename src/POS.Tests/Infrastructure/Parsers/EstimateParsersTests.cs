using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using POS.Infrastructure.Parsers;
using POS.Models.EstimateModels;
using POS.Tests.Helpers.Services.DocumentServices.ExcelService;
using Xunit;

namespace POS.Tests.Infrastructure.Parsers
{
    public class EstimateParsersTests
    {
        public EstimateParsersTests()
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
        }

        [Fact]
        public async Task ItShould_parse_labor_costs()
        {
            // arrange

            var cells = MyExcelRangeHelper.GetMock();
            cells.Setup(x => x[44, 2]).Returns(MyExcelRangeHelper.GetMock("итого по главе 1-8").Object);
            cells.Setup(x => x[44, 9]).Returns(MyExcelRangeHelper.GetMock("99").Object);
            var nrr103row = 45;
            
            var sut = new EstimateParser();

            // act

            var getLaborCostsOperation = await sut.GetLaborCosts(cells.Object, nrr103row);

            // assert

            Assert.True(getLaborCostsOperation.Ok);
            Assert.Equal(99, getLaborCostsOperation.Result);
        }

        [Fact]
        public async Task ItShould_parse_estimate_work()
        {
            // arrange

            var cells = MyExcelRangeHelper.GetMock();
            cells.Setup(x => x[45, 2]).Returns(MyExcelRangeHelper.GetMock("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА").Object);
            cells.Setup(x => x[45, 7]).Returns(MyExcelRangeHelper.GetMock("-").Object);
            cells.Setup(x => x[45, 8]).Returns(MyExcelRangeHelper.GetMock("0,017").Object);
            cells.Setup(x => x[45, 9]).Returns(MyExcelRangeHelper.GetMock("1,017").Object);

            cells.Setup(x => x[44, 2]).Returns(MyExcelRangeHelper.GetMock("ГЛАВА 2 ОСНОВНЫЕ ЗДАНИЯ,СООРУЖЕНИЯ").Object);

            var workRow = 45;

            var expectedEstimateWork = new EstimateWork
            {
                TotalCost = 1.017M,
                Chapter = 2,
                EquipmentCost = 0,
                OtherProductsCost = 0.017M,
                Percentages = new List<decimal>(),
                WorkName = "Электрохимическая защита"
            };

            var sut = new EstimateParser();

            // act

            var getEstimateWorkOperation = await sut.GetEstimateWork(cells.Object, workRow);

            // assert

            Assert.True(getEstimateWorkOperation.Ok);
            Assert.Equal(expectedEstimateWork, getEstimateWorkOperation.Result);
        }

        [Fact]
        public async Task ItShould_parse_total_estimate_work()
        {
            // arrange

            var cells = MyExcelRangeHelper.GetMock();
            cells.Setup(x => x[45, 2]).Returns(MyExcelRangeHelper.GetMock("ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ").Object);
            cells.Setup(x => x[45, 7]).Returns(MyExcelRangeHelper.GetMock("-").Object);
            cells.Setup(x => x[45, 8]).Returns(MyExcelRangeHelper.GetMock("0,017").Object);
            cells.Setup(x => x[45, 9]).Returns(MyExcelRangeHelper.GetMock("1,017").Object);

            var totalWorkPatternRow = 44;

            var expectedEstimateWork = new EstimateWork
            {
                TotalCost = 1.017M,
                Chapter = 12,
                EquipmentCost = 0,
                OtherProductsCost = 0.017M,
                Percentages = new List<decimal>(),
                WorkName = "Всего по сводному сметному расчету"
            };

            var sut = new EstimateParser();

            // act

            var getTotalEstimateWorkOperation = await sut.GetTotalEstimateWork(cells.Object, totalWorkPatternRow, TotalWorkChapter.TotalWork1To12Chapter);

            // assert

            Assert.True(getTotalEstimateWorkOperation.Ok);
            Assert.Equal(expectedEstimateWork, getTotalEstimateWorkOperation.Result);
        }
    }
}