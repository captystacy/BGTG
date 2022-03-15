using POS.DomainModels.CalendarPlanDomainModels;
using POS.DomainModels.EstimateDomainModels;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Creators.Base;

namespace POS.Infrastructure.Creators;

public class CalendarWorksCreator : ICalendarWorksCreator
{
    private readonly ICalendarWorkCreator _calendarWorkCreator;

    public CalendarWorksCreator(ICalendarWorkCreator calendarWorkCreator)
    {
        _calendarWorkCreator = calendarWorkCreator;
    }

    public List<CalendarWork> CreatePreparatoryCalendarWorks(IEnumerable<EstimateWork> preparatoryEstimateWorks, DateTime constructionStartDate)
    {
        var preparatoryCalendarWorks = new List<CalendarWork>();
        var preparatoryPercentages = Constants.AppConstants.PreparatoryPercentages.ToList();
        var calendarWorks = preparatoryEstimateWorks
            .Select(x => _calendarWorkCreator.Create(x, constructionStartDate)).ToList();

        var calendarWorksChapter1 =
            calendarWorks.FindAll(x => x.EstimateChapter == Constants.AppConstants.PreparatoryWorkChapter);
        if (calendarWorksChapter1.Count != 0)
        {
            var preparatoryCalendarWork = _calendarWorkCreator.CreateAnyPreparatoryWork(
                Constants.AppConstants.PreparatoryWorkName, calendarWorksChapter1,
                Constants.AppConstants.PreparatoryWorkChapter, constructionStartDate, preparatoryPercentages);

            if (preparatoryCalendarWork.TotalCostIncludingCAIW > 0)
            {
                preparatoryCalendarWorks.Add(preparatoryCalendarWork);
            }
        }

        var calendarWorksChapter8 = calendarWorks.FindAll(x =>
            x.EstimateChapter == Constants.AppConstants.PreparatoryTemporaryBuildingsWorkChapter);

        var temporaryBuildingsWork = _calendarWorkCreator.CreateAnyPreparatoryWork(
            Constants.AppConstants.PreparatoryTemporaryBuildingsWorkName, calendarWorksChapter8,
            Constants.AppConstants.PreparatoryTemporaryBuildingsWorkChapter, constructionStartDate,
            preparatoryPercentages);
        preparatoryCalendarWorks.Add(temporaryBuildingsWork);

        var totalCalendarWork = _calendarWorkCreator.CreateAnyPreparatoryWork(Constants.AppConstants.TotalWorkName,
            preparatoryCalendarWorks, Constants.AppConstants.PreparatoryTotalWorkChapter, constructionStartDate,
            preparatoryPercentages);
        preparatoryCalendarWorks.Add(totalCalendarWork);

        return preparatoryCalendarWorks;
    }


    public List<CalendarWork> CreateMainCalendarWorks(IEnumerable<EstimateWork> estimateWorks, CalendarWork preparatoryTotalWork, DateTime constructionStartDate, int constructionDurationCeiling, List<decimal> otherExpensesPercentages, TotalWorkChapter totalWorkChapter)
    {
        var mainCalendarWorks = estimateWorks.Select(x => _calendarWorkCreator.Create(x, constructionStartDate))
            .ToList();

        var initialTotalMainWork = mainCalendarWorks.Find(x => x.EstimateChapter == (int)totalWorkChapter)!;
        mainCalendarWorks.Remove(initialTotalMainWork);

        var mainOverallPreparatoryWork = new CalendarWork(Constants.AppConstants.MainOverallPreparatoryWorkName,
            preparatoryTotalWork.TotalCost, preparatoryTotalWork.TotalCostIncludingCAIW,
            preparatoryTotalWork.ConstructionMonths, Constants.AppConstants.MainOverallPreparatoryTotalWorkChapter);
        mainCalendarWorks.Insert(0, mainOverallPreparatoryWork);

        var otherExpensesCalendarWork = _calendarWorkCreator.CreateOtherExpensesWork(mainCalendarWorks,
            initialTotalMainWork, constructionStartDate, otherExpensesPercentages);
        mainCalendarWorks.Add(otherExpensesCalendarWork);

        var mainTotalWork = _calendarWorkCreator.CreateMainTotalWork(mainCalendarWorks, initialTotalMainWork,
            constructionStartDate, constructionDurationCeiling);
        mainCalendarWorks.Add(mainTotalWork);

        return mainCalendarWorks;
    }
}