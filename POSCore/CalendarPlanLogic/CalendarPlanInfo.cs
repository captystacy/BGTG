using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic
{
    public static class CalendarPlanInfo
    {
        public const string PreparatoryWorkName = "Подготовка территории строительства";
        public const string PreparatoryTemporaryBuildingsWorkName = "Временные здания и сооружения";

        public const string TotalWorkName = "Итого:";

        public const string MainOverallPreparatoryWorkName = "Работы, выполняемые в подготовительный период";
        public const string MainOtherExpensesWorkName = "Прочие работы и затраты";

        public const int PreparatoryWorkChapter = 1;
        public const int PreparatoryTotalWorkChapter = 1;
        public const int PreparatoryTemporaryBuildingsWorkChapter = 8;
        public const int MainOverallPreparatoryTotalWorkChapter = 1;
        public const int MainOtherExpensesWorkChapter = 10;
        public const int MainTotalWork1To9Chapter = 9;
        public const int MainTotalWork1To11Chapter = 11;
        public const int MainTotalWork1To12Chapter = 12;

        public static readonly IEnumerable<decimal> PreparatoryPercentages = new decimal[] { 1 };
    }
}
