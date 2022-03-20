namespace POS.Infrastructure.Constants;

public static partial class AppConstants
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
    public const int MainOtherExpensesWorkChapter = 10;

    public static readonly IEnumerable<decimal> PreparatoryPercentages = new decimal[] { 1 };
}