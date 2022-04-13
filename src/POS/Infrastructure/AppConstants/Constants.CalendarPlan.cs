using POS.ViewModels;

namespace POS.Infrastructure.AppConstants
{
    public partial class Constants
    {
        public const string PreparatoryWorkName = "Подготовка территории строительства";
        public const string PreparatoryTemporaryBuildingsWorkName = "Временные здания и сооружения";
        public const string TotalWorkName = "Итого:";
        public const string MainOverallPreparatoryWorkName = "Работы, выполняемые в подготовительный период";
        public const string MainOtherExpensesWorkName = "Прочие работы и затраты";

        public const int PreparatoryWorkChapter = 1;
        public const int PreparatoryTotalWorkChapter = 1;
        public const int PreparatoryTemporaryBuildingsWorkChapter = 8;
        public const int MainOverallPreparatoryTotalWorkChapter = 2;
        public const int MainOtherExpensesWorkChapter = 9;

        public static readonly CalendarWorkViewModel PreparatoryCalendarWork = new()
        {
            WorkName = PreparatoryWorkName,
            Chapter = PreparatoryWorkChapter,
            Percentages = new List<decimal>()
        };

        public static readonly CalendarWorkViewModel TemporaryBuildingsCalendarWork = new()
        {
            WorkName = PreparatoryTemporaryBuildingsWorkName,
            Chapter = PreparatoryTemporaryBuildingsWorkChapter,
            Percentages = new List<decimal>()
        };

        public static readonly CalendarWorkViewModel OtherExpensesCalendarWork = new()
        {
            WorkName = MainOtherExpensesWorkName,
            Chapter = MainOtherExpensesWorkChapter,
            Percentages = new List<decimal>()
        };
    }
}