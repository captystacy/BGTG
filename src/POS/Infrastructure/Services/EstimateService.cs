using Calabonga.OperationResults;
using POS.Infrastructure.Connectors;
using POS.Infrastructure.Readers.Base;
using POS.Infrastructure.Services.Base;
using POS.Models.EstimateModels;

namespace POS.Infrastructure.Services
{
    public class EstimateService : IEstimateService
    {
        private readonly IEstimateConnector _estimateConnector;
        private readonly IEstimateReader _estimateReader;

        public EstimateService(IEstimateConnector estimateConnector, IEstimateReader estimateReader)
        {
            _estimateConnector = estimateConnector;
            _estimateReader = estimateReader;
        }

        public Task<OperationResult<DateTime>> GetConstructionStartDate(IFormFile estimateFile) => 
            _estimateReader.GetConstructionStartDate(estimateFile.OpenReadStream());

        public async Task<OperationResult<EstimateWork>> GetTotalEstimateWork(IFormFileCollection estimateFiles, TotalWorkChapter totalWorkChapter)
        {
            var operation = OperationResult.CreateResult<EstimateWork>();

            if (estimateFiles.Count == 0)
            {
                operation.AddError("Estimate files are not found");
                return operation;
            }

            if (totalWorkChapter is TotalWorkChapter.None)
            {
                operation.AddError("Total work chapter was not set");
                return operation;
            }

            var estimateStreams = estimateFiles.Select(x => x.OpenReadStream());

            var tasks = new List<Task<OperationResult<EstimateWork>>>();

            foreach (var stream in estimateStreams)
            {
                var getTotalEstimateWorkTask = _estimateReader.GetTotalEstimateWork(stream, totalWorkChapter);
                tasks.Add(getTotalEstimateWorkTask);
            }

            var readOperations = await Task.WhenAll(tasks);

            if (readOperations.Any(x => !x.Ok))
            {
                var errors = readOperations.Select(x => x.GetMetadataMessages());
                operation.AddError(string.Join('\n', errors));
                return operation;
            }

            var estimateWorks = readOperations.Select(x => x.Result).ToList();

            var connectOperation = await _estimateConnector.ConnectEstimateWork(estimateWorks);

            if (!connectOperation.Ok)
            {
                operation.AddError(connectOperation.GetMetadataMessages());
                return operation;
            }

            operation.Result = connectOperation.Result;

            return operation;
        }

        public async Task<OperationResult<Estimate>> GetEstimate(IFormFileCollection estimateFiles, TotalWorkChapter totalWorkChapter)
        {
            var operation = OperationResult.CreateResult<Estimate>();

            if (estimateFiles.Count == 0)
            {
                operation.AddError("Estimate files are not found");
                return operation;
            }

            if (totalWorkChapter is TotalWorkChapter.None)
            {
                operation.AddError("Total work chapter was not set");
                return operation;
            }

            var estimateStreams = estimateFiles.Select(x => x.OpenReadStream());

            var tasks = new List<Task<OperationResult<Estimate>>>();

            foreach (var stream in estimateStreams)
            {
                var readTask = _estimateReader.GetEstimate(stream, totalWorkChapter);
                tasks.Add(readTask);
            }

            var readOperations = await Task.WhenAll(tasks);

            if (readOperations.Any(x => !x.Ok))
            {
                var errors = readOperations.Select(x => x.GetMetadataMessages());
                operation.AddError(string.Join('\n', errors));
                return operation;
            }

            var estimates = readOperations.Select(x => x.Result).ToList();

            var connectOperation = await _estimateConnector.Connect(estimates);

            if (!connectOperation.Ok)
            {
                operation.AddError(connectOperation.GetMetadataMessages());
                return operation;
            }

            operation.Result = connectOperation.Result;

            return operation;
        }

        public async Task<OperationResult<int>> GetLaborCosts(IFormFileCollection estimateFiles)
        {
            var operation = OperationResult.CreateResult<int>();

            if (estimateFiles.Count == 0)
            {
                operation.AddError("Estimate files are not found");
                return operation;
            }

            var estimateStreams = estimateFiles.Select(x => x.OpenReadStream());

            var tasks = new List<Task<OperationResult<int>>>();

            foreach (var stream in estimateStreams)
            {
                var readTask = _estimateReader.GetLaborCosts(stream);
                tasks.Add(readTask);
            }

            var readOperations = await Task.WhenAll(tasks);

            if (readOperations.Any(x => !x.Ok))
            {
                var errors = readOperations.Select(x => x.GetMetadataMessages());
                operation.AddError(string.Join('\n', errors));
                return operation;
            }

            var laborCosts = readOperations.Select(x => x.Result).Sum();

            operation.Result = laborCosts;

            return operation;
        }
    }
}