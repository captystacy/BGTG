using Calabonga.OperationResults;
using POS.Infrastructure.Appenders.Base;
using POS.Infrastructure.Calculators.Base;
using POS.Infrastructure.Factories.Base;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Services.DocumentServices;
using POS.ViewModels;

namespace POS.Infrastructure.Services
{
    public class EnergyAndWaterService : IEnergyAndWaterService
    {
        private readonly IMyWordDocumentFactory _documentFactory;
        private readonly IEnergyAndWaterCalculator _energyAndWaterCalculator;
        private readonly IEnergyAndWaterAppender _energyAndWaterAppender;
        private readonly IEstimateService _estimateService;
        private readonly ICalendarWorkCalculator _calendarWorkCalculator;

        public EnergyAndWaterService(IMyWordDocumentFactory documentFactory, 
            IEnergyAndWaterCalculator energyAndWaterCalculator,
            IEnergyAndWaterAppender energyAndWaterAppender,
            IEstimateService estimateService,
            ICalendarWorkCalculator calendarWorkCalculator)
        {
            _documentFactory = documentFactory;
            _energyAndWaterCalculator = energyAndWaterCalculator;
            _energyAndWaterAppender = energyAndWaterAppender;
            _estimateService = estimateService;
            _calendarWorkCalculator = calendarWorkCalculator;
        }

        public async Task<OperationResult<MemoryStream>> GetEnergyAndWaterStream(EnergyAndWaterViewModel viewModel)
        {
            var operation = OperationResult.CreateResult<MemoryStream>();

            var getTotalEstimateWorkOperation = await _estimateService.GetTotalEstimateWork(viewModel.EstimateFiles, viewModel.TotalWorkChapter);

            if (!getTotalEstimateWorkOperation.Ok)
            {
                operation.AddError(getTotalEstimateWorkOperation.GetMetadataMessages());
                return operation;
            }

            var totalEstimateWork = getTotalEstimateWorkOperation.Result;

            var getConstructionStartDateOperation = await _estimateService.GetConstructionStartDate(viewModel.EstimateFiles[0]);

            if (!getConstructionStartDateOperation.Ok)
            {
                operation.AddError(getConstructionStartDateOperation.GetMetadataMessages());
                return operation;
            }

            var constructionStartDate = getConstructionStartDateOperation.Result;

            var calculateCalendarWorkOperation = await _calendarWorkCalculator.Calculate(totalEstimateWork, constructionStartDate);

            if (!calculateCalendarWorkOperation.Ok)
            {
                operation.AddError(calculateCalendarWorkOperation.GetMetadataMessages());
                return operation;
            }

            var totalCalendarWork = calculateCalendarWorkOperation.Result;

            var calculateEnergyAndWaterOperation = await _energyAndWaterCalculator.Calculate(totalCalendarWork.TotalCostIncludingCAIW, constructionStartDate.Year);

            if (!calculateEnergyAndWaterOperation.Ok)
            {
                operation.AddError(calculateEnergyAndWaterOperation.GetMetadataMessages());
                return operation;
            }

            var document = await _documentFactory.CreateAsync();

            var section = document.AddSection();

            await _energyAndWaterAppender.AppendAsync(section, calculateEnergyAndWaterOperation.Result);

            var memoryStream = new MemoryStream();
            document.SaveAs(memoryStream, MyFileFormat.DocX);
            memoryStream.Seek(0, SeekOrigin.Begin);

            operation.Result = memoryStream;

            return operation;
        }
    }
}
