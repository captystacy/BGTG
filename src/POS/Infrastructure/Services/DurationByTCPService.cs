using Calabonga.OperationResults;
using POS.Infrastructure.Appenders.Base;
using POS.Infrastructure.Calculators.Base;
using POS.Infrastructure.Factories.Base;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Services.DocumentServices;
using POS.ViewModels;

namespace POS.Infrastructure.Services
{
    public class DurationByTCPService : IDurationByTCPService
    {
        private readonly IMyWordDocumentFactory _documentFactory;
        private readonly IDurationByTCPCalculator _durationByTCPCalculator;
        private readonly IDurationByTCPAppender _durationByTCPAppender;

        public DurationByTCPService(IMyWordDocumentFactory documentFactory, IDurationByTCPCalculator durationByTCPCalculator, IDurationByTCPAppender durationByTCPAppender)
        {
            _documentFactory = documentFactory;
            _durationByTCPCalculator = durationByTCPCalculator;
            _durationByTCPAppender = durationByTCPAppender;
        }

        public async Task<OperationResult<MemoryStream>> GetDurationByTCPStream(DurationByTCPViewModel viewModel)
        {
            var operation = OperationResult.CreateResult<MemoryStream>();

            var calculateOperation = await _durationByTCPCalculator.Calculate(viewModel);

            if (!calculateOperation.Ok)
            {
                operation.AddError(calculateOperation.GetMetadataMessages());
                return operation;
            }

            var document = await _documentFactory.CreateAsync();

            var section = document.AddSection();

            await _durationByTCPAppender.AppendAsync(section, calculateOperation.Result);

            var memoryStream = new MemoryStream();
            document.SaveAs(memoryStream, MyFileFormat.DocX);
            memoryStream.Seek(0, SeekOrigin.Begin);

            operation.Result = memoryStream;

            return operation;
        }
    }
}
