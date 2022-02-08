namespace BGTG.Core
{
    public partial class AppData
    {
        public const string CalendarPlanDownloadFileName = "Календарный план.docx";
        public const string DurationByLCDownloadFileName = "Продолжительность по трудозатратам.docx";
        public const string DurationByTCPDownloadFileName = "Продолжительность по ТКП.docx";
        public const string EnergyAndWaterDownloadFileName = "Энергия и вода.docx";

        public const string PipelineDiameterValidationMessage = "Диаметр трубопровода не может быть отрицательным или равным нулю.";

        public const string PipelineLengthValidationMessage = "Длина трубопровода не может быть отрицательна.";

        public const string EstimateFilesValidationMessage = "Сметы не найдены.";

        public const string NumberOfWorkingDaysValidationMessage = "Количество рабочих дней в месяце не может быть отрицательным или равным нулю.";

        public const string WorkingDayDurationValidationMessage = "Продолжительность рабочего дня не может быть отрицательной или равной нулю.";

        public const string ShiftValidationMessage = "Сменность не может быть отрицательной или равной нулю.";

        public const string NumberOfEmployeesValidationMessage = "Количество работающих в бригаде не может быть отрицательным или равным нулю.";

        public const string TechnologicalLaborCostsValidationMessage = "Трудозатраты по технологической карте не могут быть отрицательными или равными нулю.";

        public const string ConstructionStartDateValidationMessage = "Год даты начала строительства не может быть ниже 1900.";

        public const string ConstructionDurationCeilingValidationMessage = "Месяц должен быть в диапазоне от 1 до 21.";

        public const string TotalWorkChapterValidationMessage = "Итого поддерживается только по 1-9, 1-11, или по последней строчке.";

        public const string WorkNameValidationMessage = "Наименование работы не найдено.";

        public const string ChapterValidationMessage = "Глава сметной работы не может быть меньше 0.";

        public const string DurationByTCPBadParametersValidationMessage = "Не найдены подходящие нормы по запрашиваемым параметрам.";

        public const string DurationByTCPUnknown = "Неизвестный тип продолжительности по ТКП.";

        public const string BadCalendarPlanId = "Календарный план не найден по данному ID.";

        public const string BadDurationByLCId = "Продолжительность по трудозатратам не найдена по данному ID.";

        public const string BadDurationByTCPId = "Продолжительность по ТКП не найдена по данному ID.";

        public const string BadEnergyAndWaterId = "Энергия и вода не найдена по данному ID.";
    }
}
