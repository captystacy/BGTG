using Calabonga.OperationResults;
using POS.Infrastructure.Factories.Base;
using POS.Infrastructure.Parsers.Base;
using POS.Infrastructure.Readers.Base;

namespace POS.Infrastructure.Readers
{
    public class CalendarPlanReader : ICalendarPlanReader
    {
        private readonly IMyWordDocumentFactory _documentFactory;
        private readonly IConstructionParser _constructionParser;

        public CalendarPlanReader(IMyWordDocumentFactory documentFactory, IConstructionParser constructionParser)
        {
            _documentFactory = documentFactory;
            _constructionParser = constructionParser;
        }

        public async Task<OperationResult<DateTime>> GetConstructionStartDate(Stream stream)
        {
            var operation = OperationResult.CreateResult<DateTime>();

            if (stream.Length <= 0)
            {
                operation.AddError("Stream has length 0");
                return operation;
            }

            using var document = await _documentFactory.CreateAsync(stream);

            var constructionStartDateCellStr = document.Sections[0].Tables[0].Rows[1].Cells[3].Paragraphs[0].Text;

            var getConstructionStartDateOperation = await _constructionParser.GetConstructionStartDate(constructionStartDateCellStr);

            if (!getConstructionStartDateOperation.Ok)
            {
                operation.AddError(getConstructionStartDateOperation.GetMetadataMessages());
                return operation;
            }

            operation.Result = getConstructionStartDateOperation.Result;

            return operation;
        }
    }
}
