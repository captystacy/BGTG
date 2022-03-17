namespace POS.Infrastructure.Constants;

public static class AppConstants
{
    public const string Unknown = "Unknown";

    public const string DocxMimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

    public const string DecimalFormat = "G29";

    public const string DecimalThreePlacesFormat = "F3";

    public const string PercentFormat = "P2";

    public const string DateTimeMonthAndYearShortFormat = "MM.yy";

    public const string ObjectCipherValidationMessage = "Шифр объекта указан в неправильном формате. Правильный формат: Ц.Ц-Ч.Ч или Ц.Ц-Ч.Ч-Ч, где Ц - цифра, Ч - число.";

    public const string PipelineDiameterValidationMessage = "Диаметр трубопровода не может быть отрицательным или равным нулю.";

    public const string PipelineLengthValidationMessage = "Длина трубопровода не может быть отрицательна.";

    public const string EstimateFilesValidationMessage = "Сметы не найдены.";

    public const string NumberOfWorkingDaysValidationMessage = "Количество рабочих дней в месяце не может быть отрицательным или равным нулю.";

    public const string WorkingDayDurationValidationMessage = "Продолжительность рабочего дня не может быть отрицательной или равной нулю.";

    public const string ShiftValidationMessage = "Сменность не может быть отрицательной или равной нулю.";

    public const string NumberOfEmployeesValidationMessage = "Количество работающих в бригаде не может быть отрицательным или равным нулю.";

    public const string TechnologicalLaborCostsValidationMessage = "Трудозатраты по технологической карте не могут быть отрицательными или равными нулю.";

    public const string ConstructionStartDateValidationMessage = "Год даты начала строительства не может быть ниже 1900.";

    public const string TotalWorkChapterValidationMessage = "Итого поддерживается только по 1-9, 1-11, или по последней строчке.";

    public const string WorkNameValidationMessage = "Наименование работы не найдено.";

    public const string ChapterValidationMessage = "Глава сметной работы не может быть меньше 0.";

    public const string ObjectNameValidationMessage = "Имя объекта не может быть пустым.";

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