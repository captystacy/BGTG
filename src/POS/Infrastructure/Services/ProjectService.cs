using Calabonga.OperationResults;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Calculators.Base;
using POS.Infrastructure.Readers.Base;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;
using POS.ViewModels;

namespace POS.Infrastructure.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectWriter _projectWriter;
        private readonly IDurationByLCReader _durationByLCReader;
        private readonly ICalendarPlanReader _calendarPlanReader;
        private readonly IEmployeesNeedCalculator _employeesNeedCalculator;

        public ProjectService(IProjectWriter projectWriter, IDurationByLCReader durationByLCReader, ICalendarPlanReader calendarPlanReader, IEmployeesNeedCalculator employeesNeedCalculator)
        {
            _projectWriter = projectWriter;
            _durationByLCReader = durationByLCReader;
            _calendarPlanReader = calendarPlanReader;
            _employeesNeedCalculator = employeesNeedCalculator;
        }

        public async Task<OperationResult<MemoryStream>> GetProjectStream(ProjectViewModel viewModel)
        {
            var operation = OperationResult.CreateResult<MemoryStream>();

            var durationByLCStream =
                viewModel.CalculationFiles.First(x => x.FileName.Contains(Constants.DurationByLCFileName)).OpenReadStream();

            var getDurationByLCOperation = await _durationByLCReader.GetDurationByLC(durationByLCStream);

            if (!getDurationByLCOperation.Ok)
            {
                operation.AddError(getDurationByLCOperation.GetMetadataMessages());
                return operation;
            }

            var durationByLC = getDurationByLCOperation.Result;

            var calculateOperation = await _employeesNeedCalculator.Calculate(durationByLC.NumberOfEmployees, durationByLC.Shift);

            if (!calculateOperation.Ok)
            {
                operation.AddError(calculateOperation.GetMetadataMessages());
                return operation;
            }

            var employeesNeed = calculateOperation.Result;

            var calendarPlanStream =
                viewModel.CalculationFiles.First(x => x.FileName.Contains(Constants.CalendarPlanFileName)).OpenReadStream();

            var getConstructionStartDateOperation = await _calendarPlanReader.GetConstructionStartDate(calendarPlanStream);

            if (!getConstructionStartDateOperation.Ok)
            {
                operation.AddError(getConstructionStartDateOperation.GetMetadataMessages());
                return operation;
            }

            operation.Result = await _projectWriter.GetProjectStream(viewModel, getConstructionStartDateOperation.Result, employeesNeed, durationByLC);

            return operation;
        }
    }
}
