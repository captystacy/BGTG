using System;
using Calabonga.OperationResults;
using Moq;
using POS.Infrastructure.Parsers.Base;

namespace POS.Tests.Helpers.Parsers
{
    public static class ConstructionParserHelper
    {
        public static Mock<IConstructionParser> GetMock()
        {
            return new Mock<IConstructionParser>();
        }
        
        public static Mock<IConstructionParser> GetMock(string constructionStartDateCellStr, DateTime constructionStartDate)
        {
            var constructionParser = GetMock();

            constructionParser
                .Setup(x => x.GetConstructionStartDate(constructionStartDateCellStr))
                .ReturnsAsync(new OperationResult<DateTime>
                {
                    Result = constructionStartDate
                });

            return constructionParser;
        }
    }
}
