using Calabonga.OperationResults;
using POS.Infrastructure.Appenders.Base;
using POS.Infrastructure.Calculators.Base;
using POS.Infrastructure.Factories.Base;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Services.DocumentServices;
using POS.ViewModels;

namespace POS.Infrastructure.Services
{
    public class DurationByLCService : IDurationByLCService
    {
        private readonly IMyWordDocumentFactory _documentFactory;
        private readonly IDurationByLCCalculator _durationByLCCalculator;
        private readonly IDurationByLCAppender _durationByLCAppender;

        public DurationByLCService(IMyWordDocumentFactory documentFactory, IDurationByLCCalculator durationByLCCalculator, IDurationByLCAppender durationByLCAppender)
        {
            _documentFactory = documentFactory;
            _durationByLCCalculator = durationByLCCalculator;
            _durationByLCAppender = durationByLCAppender;
        }

        public async Task<OperationResult<MemoryStream>> GetDurationByLCStream(DurationByLCViewModel viewModel)
        {
            var operation = OperationResult.CreateResult<MemoryStream>();

            var calculateOperation = await _durationByLCCalculator.Calculate(viewModel);

            if (!calculateOperation.Ok)
            {
                operation.AddError(calculateOperation.GetMetadataMessages());
                return operation;
            }

            var document = await _documentFactory.CreateAsync();

            var section = document.AddSection();

            await _durationByLCAppender.AppendAsync(section, calculateOperation.Result);

            var memoryStream = new MemoryStream();
            document.SaveAs(memoryStream, MyFileFormat.DocX);
            memoryStream.Seek(0, SeekOrigin.Begin);

            operation.Result = memoryStream;

            return operation;
        }
    }
}
