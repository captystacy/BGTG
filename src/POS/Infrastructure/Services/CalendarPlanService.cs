using AutoMapper;
using Calabonga.OperationResults;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Appenders.Base;
using POS.Infrastructure.Calculators.Base;
using POS.Infrastructure.Factories.Base;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Services.DocumentServices;
using POS.Infrastructure.Services.DocumentServices.WordService.Format;
using POS.Models.CalendarPlanModels;
using POS.Models.EstimateModels;
using POS.ViewModels;

namespace POS.Infrastructure.Services
{
    public class CalendarPlanService : ICalendarPlanService
    {
        private readonly IMyWordDocumentFactory _documentFactory;
        private readonly IEstimateService _estimateService;
        private readonly ICalendarPlanCalculator _calendarPlanCalculator;
        private readonly ICalendarPlanAppender _calendarPlanAppender;
        private readonly IMapper _mapper;

        public CalendarPlanService(IMyWordDocumentFactory documentFactory, IEstimateService estimateService, ICalendarPlanCalculator calendarPlanCalculator,
            ICalendarPlanAppender calendarPlanAppender, IMapper mapper)
        {
            _documentFactory = documentFactory;
            _estimateService = estimateService;
            _calendarPlanCalculator = calendarPlanCalculator;
            _calendarPlanAppender = calendarPlanAppender;
            _mapper = mapper;
        }

        public async Task<OperationResult<CalendarPlanViewModel>> GetCalendarPlanViewModel(CalendarPlanCreateViewModel viewModel)
        {
            var operation = OperationResult.CreateResult<CalendarPlanViewModel>();

            var getEstimateOperation = await _estimateService.GetEstimate(viewModel.EstimateFiles, viewModel.TotalWorkChapter);

            if (!getEstimateOperation.Ok)
            {
                operation.AddError(getEstimateOperation.GetMetadataMessages());
                return operation;
            }

            var estimate = getEstimateOperation.Result;
            
            var calendarPlanViewModel = _mapper.Map<CalendarPlanViewModel>(estimate);

            var havePreparatoryCalendarWorks = calendarPlanViewModel.PreparatoryCalendarWorks
                .Any(x => x.Chapter == Constants.PreparatoryWorkChapter);

            calendarPlanViewModel.PreparatoryCalendarWorks.Clear();

            if (havePreparatoryCalendarWorks)
            {
                calendarPlanViewModel.PreparatoryCalendarWorks.Add(Constants.PreparatoryCalendarWork);
            }

            calendarPlanViewModel.PreparatoryCalendarWorks.Add(Constants.TemporaryBuildingsCalendarWork);

            calendarPlanViewModel.MainCalendarWorks.RemoveAll(x => x.Chapter == (int)viewModel.TotalWorkChapter);

            calendarPlanViewModel.MainCalendarWorks.Add(Constants.OtherExpensesCalendarWork);

            operation.Result = calendarPlanViewModel;

            return operation;
        }

        public async Task<OperationResult<IEnumerable<decimal>>> GetTotalPercentages(CalendarPlanViewModel viewModel)
        {
            var operation = OperationResult.CreateResult<IEnumerable<decimal>>();

            var getEstimateOperation = await _estimateService.GetEstimate(viewModel.EstimateFiles, viewModel.TotalWorkChapter);

            if (!getEstimateOperation.Ok)
            {
                operation.AddError(getEstimateOperation.GetMetadataMessages());
                return operation;
            }

            var preparatoryCalendarWork = viewModel.PreparatoryCalendarWorks.Find(x => x.WorkName == Constants.PreparatoryWorkName);
            var preparatoryPercentages = preparatoryCalendarWork is not null
                ? preparatoryCalendarWork.Percentages
                : new List<decimal>();

            var temporaryBuildingsPercentages = viewModel.PreparatoryCalendarWorks.Find(x => x.WorkName == Constants.PreparatoryTemporaryBuildingsWorkName)!.Percentages;

            var otherExpensesWork = viewModel.MainCalendarWorks.Find(x => x.WorkName == Constants.MainOtherExpensesWorkName)!;
            viewModel.MainCalendarWorks.Remove(otherExpensesWork);

            var estimate = PrepareEstimateForCalculations(getEstimateOperation.Result, viewModel, preparatoryPercentages, temporaryBuildingsPercentages);

            var calculatePreparatoryOperation = await _calendarPlanCalculator.CalculatePreparatory(estimate, preparatoryPercentages, temporaryBuildingsPercentages);

            if (!calculatePreparatoryOperation.Ok)
            {
                operation.AddError(calculatePreparatoryOperation.GetMetadataMessages());
                return operation;
            }

            var preparatoryCalendarPlan = calculatePreparatoryOperation.Result;

            var totalPreparatoryWork = preparatoryCalendarPlan.CalendarWorks.First(x => x.WorkName == Constants.TotalWorkName);

            var calculateMainOperation = await _calendarPlanCalculator.CalculateMain(estimate, totalPreparatoryWork, otherExpensesWork.Percentages);

            if (!calculateMainOperation.Ok)
            {
                operation.AddError(calculateMainOperation.GetMetadataMessages());
                return operation;
            }

            var mainCalendarPlan = calculateMainOperation.Result;

            operation.Result = mainCalendarPlan.CalendarWorks
                .First(x => x.EstimateChapter == (int)viewModel.TotalWorkChapter)
                .ConstructionMonths
                .Select(x => x.PercentPart);

            return operation;
        }

        public async Task<OperationResult<MemoryStream>> GetCalendarPlanStream(CalendarPlanViewModel viewModel)
        {
            var operation = OperationResult.CreateResult<MemoryStream>();

            var getEstimateOperation = await _estimateService.GetEstimate(viewModel.EstimateFiles, viewModel.TotalWorkChapter);

            if (!getEstimateOperation.Ok)
            {
                operation.AddError(getEstimateOperation.GetMetadataMessages());
                return operation;
            }

            var preparatoryCalendarWork = viewModel.PreparatoryCalendarWorks.Find(x => x.WorkName == Constants.PreparatoryWorkName);
            var preparatoryPercentages = preparatoryCalendarWork is not null 
                    ? preparatoryCalendarWork.Percentages
                    : new List<decimal>();

            var temporaryBuildingsPercentages = viewModel.PreparatoryCalendarWorks.Find(x => x.WorkName == Constants.PreparatoryTemporaryBuildingsWorkName)!.Percentages;

            var otherExpensesWork = viewModel.MainCalendarWorks.Find(x => x.WorkName == Constants.MainOtherExpensesWorkName)!;
            viewModel.MainCalendarWorks.Remove(otherExpensesWork);

            var estimate = PrepareEstimateForCalculations(getEstimateOperation.Result, viewModel, preparatoryPercentages, temporaryBuildingsPercentages);

            var calculatePreparatoryOperation = await _calendarPlanCalculator.CalculatePreparatory(estimate, preparatoryPercentages, temporaryBuildingsPercentages);

            if (!calculatePreparatoryOperation.Ok)
            {
                operation.AddError(calculatePreparatoryOperation.GetMetadataMessages());
                return operation;
            }

            var preparatoryCalendarPlan = calculatePreparatoryOperation.Result;

            var totalPreparatoryWork = preparatoryCalendarPlan.CalendarWorks.First(x => x.WorkName == Constants.TotalWorkName);

            var calculateMainOperation = await _calendarPlanCalculator.CalculateMain(estimate, totalPreparatoryWork, otherExpensesWork.Percentages);

            if (!calculateMainOperation.Ok)
            {
                operation.AddError(calculateMainOperation.GetMetadataMessages());
                return operation;
            }

            var mainCalendarPlan = calculateMainOperation.Result;

            using var document = await _documentFactory.CreateAsync();

            var section = document.AddSection();

            await _calendarPlanAppender.AppendAsync(section, preparatoryCalendarPlan, CalendarPlanType.Preparatory);

            var paragraph = section.AddParagraph();
            paragraph.AppendBreak(MyBreakType.PageBreak);

            await _calendarPlanAppender.AppendAsync(section, mainCalendarPlan, CalendarPlanType.Main);

            var memoryStream = new MemoryStream();
            document.SaveAs(memoryStream, MyFileFormat.DocX);
            memoryStream.Seek(0, SeekOrigin.Begin);

            operation.Result = memoryStream;

            return operation;
        }

        private Estimate PrepareEstimateForCalculations(Estimate estimate, CalendarPlanViewModel viewModel,
            List<decimal> preparatoryPercentages, List<decimal> temporaryBuildingsPercentages)
        {
            if (estimate.ConstructionStartDate == default)
            {
                estimate.ConstructionStartDate = viewModel.ConstructionStartDate;
            }

            if (estimate.ConstructionDuration == 0)
            {
                estimate.ConstructionDuration = viewModel.ConstructionDuration;
            }

            if (estimate.ConstructionDurationCeiling == 0)
            {
                estimate.ConstructionDurationCeiling = (int)decimal.Ceiling(viewModel.ConstructionDuration);
            }

            var preparatoryEstimateWorks = estimate.PreparatoryEstimateWorks
                .Where(x => x.Chapter == Constants.PreparatoryWorkChapter);

            foreach (var preparatoryEstimateWork in preparatoryEstimateWorks)
            {
                preparatoryEstimateWork.Percentages = preparatoryPercentages;
            }

            var temporaryBuildingsEstimateWorks = estimate.PreparatoryEstimateWorks
                .Where(x => x.Chapter == Constants.PreparatoryTemporaryBuildingsWorkChapter);

            foreach (var temporaryBuildingsEstimateWork in temporaryBuildingsEstimateWorks)
            {
                temporaryBuildingsEstimateWork.Percentages = temporaryBuildingsPercentages;
            }

            foreach (var calendarWorkViewModel in viewModel.MainCalendarWorks)
            {
                estimate.MainEstimateWorks
                    .First(estimateWork => estimateWork.WorkName == calendarWorkViewModel.WorkName)
                    .Percentages
                    .AddRange(calendarWorkViewModel.Percentages);
            }

            return estimate;
        }
    }
}