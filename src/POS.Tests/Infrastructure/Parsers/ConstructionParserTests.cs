using System;
using System.Globalization;
using System.Threading.Tasks;
using POS.Infrastructure.Parsers;
using Xunit;

namespace POS.Tests.Infrastructure.Parsers
{
    public class ConstructionParserTests
    {
        public ConstructionParserTests()
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
        }

        [Fact]
        public async Task ItShould_parse_construction_duration()
        {
            // arrange

            var constructionDurationCellStr = "0,7 мес";

            var sut = new ConstructionParser();

            // act

            var getConstructionDurationOperation = await sut.GetConstructionDuration(constructionDurationCellStr);

            // assert

            Assert.True(getConstructionDurationOperation.Ok);
            Assert.Equal(0.7M, getConstructionDurationOperation.Result);
        }

        [Fact]
        public async Task ItShould_parse_construction_start_date()
        {
            // arrange

            var constructionStartDateCellStr = "август 2022";

            var sut = new ConstructionParser();

            // act

            var getConstructionStartDateOperation = await sut.GetConstructionStartDate(constructionStartDateCellStr);

            // assert

            Assert.True(getConstructionStartDateOperation.Ok);
            Assert.Equal(new DateTime(2022, 8, 1), getConstructionStartDateOperation.Result);
        }
    }
}
