using Calabonga.OperationResults;
using POS.Models.EstimateModels;

namespace POS.Infrastructure.Connectors
{
    public class EstimateConnector : IEstimateConnector
    {
        public async Task<OperationResult<Estimate>> Connect(IReadOnlyList<Estimate> estimates)
        {
            var operation = OperationResult.CreateResult<Estimate>();

            if (estimates.Count < 0)
            {
                operation.AddError("Estimates were empty");
                return operation;
            }

            if (estimates.Count == 1)
            {
                operation.Result = estimates[0];
                return operation;
            }

            var preparatoryEstimateWorks = estimates.SelectMany(x => x.PreparatoryEstimateWorks).ToList();

            var connectPreparatoryEstimateWorksOperations = await ConnectEstimateWorks(preparatoryEstimateWorks);

            if (connectPreparatoryEstimateWorksOperations.Any(x => !x.Ok))
            {
                var errors = string.Join('\n', connectPreparatoryEstimateWorksOperations.Select(x => x.GetMetadataMessages()));
                operation.AddError(errors);
                return operation;
            }

            var preparatoryEstimateWorksConnected = connectPreparatoryEstimateWorksOperations.Select(x => x.Result).ToList();

            var mainEstimateWorks = estimates.SelectMany(x => x.MainEstimateWorks).ToList();

            var connectMainEstimateWorksOperations = await ConnectEstimateWorks(mainEstimateWorks);

            if (connectMainEstimateWorksOperations.Any(x => !x.Ok))
            {
                var errors = string.Join('\n', connectMainEstimateWorksOperations.Select(x => x.GetMetadataMessages()));
                operation.AddError(errors);
                return operation;
            }

            var mainEstimateWorksConnected = connectMainEstimateWorksOperations.Select(x => x.Result).ToList();

            operation.Result = new Estimate
            {
                ConstructionStartDate = estimates[0].ConstructionStartDate,
                MainEstimateWorks = mainEstimateWorksConnected,
                ConstructionDuration = estimates[0].ConstructionDuration,
                ConstructionDurationCeiling = estimates[0].ConstructionDurationCeiling,
                PreparatoryEstimateWorks = preparatoryEstimateWorksConnected,
                TotalWorkChapter = estimates[0].TotalWorkChapter
            };

            return operation;
        }

        private Task<OperationResult<EstimateWork>[]> ConnectEstimateWorks(IEnumerable<EstimateWork> estimateWorks)
        {
            var sumTasks = estimateWorks
                .GroupBy(x => x.WorkName)
                .Select(x => ConnectEstimateWork(x.ToList()))
                .ToList();

            return Task.WhenAll(sumTasks);
        }

        public Task<OperationResult<EstimateWork>> ConnectEstimateWork(IReadOnlyList<EstimateWork> estimateWorks)
        {
            var operation = OperationResult.CreateResult<EstimateWork>();

            if (estimateWorks.Count == 0)
            {
                operation.AddError("Estimate works were empty");
                return Task.FromResult(operation);
            }

            if (estimateWorks.Count == 1)
            {
                operation.Result = estimateWorks[0];
                return Task.FromResult(operation);
            }

            var firstEstimateWork = estimateWorks[0];

            var estimateWork = new EstimateWork
            {
                TotalCost = estimateWorks.Sum(x => x.TotalCost),
                Chapter = firstEstimateWork.Chapter,
                EquipmentCost = estimateWorks.Sum(x => x.EquipmentCost),
                OtherProductsCost = estimateWorks.Sum(x => x.OtherProductsCost),
                WorkName = firstEstimateWork.WorkName,
                Percentages = firstEstimateWork.Percentages,
            };

            operation.Result = estimateWork;

            return Task.FromResult(operation);
        }
    }
}