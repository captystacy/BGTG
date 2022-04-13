using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using POS.Infrastructure.Readers;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Tests.Helpers;
using POS.Tests.Helpers.Factories;
using POS.Tests.Helpers.Parsers;
using POS.Tests.Helpers.Services.DocumentServices.WordService;
using Xunit;

namespace POS.Tests.Infrastructure.Readers
{
    public class CalendarPlanReaderTests
    {
        [Fact]
        public async Task ItShould_get_construction_start_date()
        {
            // arrange

            var constructionStartDateCellStr = "Август 2022";
            var constructionStartDate = new DateTime(2022, 8, 1);

            var rows = new List<Mock<IMyRow>>
            {
                MyRowHelper.GetMock(),
                MyRowHelper.GetMock(3, constructionStartDateCellStr),
            };

            var table = MyTableHelper.GetMock(rows);
            var section = MySectionHelper.GetMock(new List<Mock<IMyTable>> { table });
            var document = MyWordDocumentHelper.GetMock(section);
            var stream = StreamHelper.GetMock();
            var documentFactory = MyWordDocumentFactoryHelper.GetMock(stream, document);
            var constructionParser = ConstructionParserHelper.GetMock(constructionStartDateCellStr, constructionStartDate);
            var sut = new CalendarPlanReader(documentFactory.Object, constructionParser.Object);

            // act

            var getConstructionStartDateOperation = await sut.GetConstructionStartDate(stream.Object);

            // assert

            Assert.True(getConstructionStartDateOperation.Ok);

            Assert.Equal(constructionStartDate, getConstructionStartDateOperation.Result);
        }
    }
}
