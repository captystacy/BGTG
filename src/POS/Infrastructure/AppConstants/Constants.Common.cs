using System.Text.RegularExpressions;

namespace POS.Infrastructure.AppConstants
{
    public static partial class Constants
    {
        public const string Unknown = "Unknown";

        public const string DocxMimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

        public const string DecimalThreePlacesFormat = "F3";

        public const string PercentFormat = "P2";

        public const string DateTimeMonthAndYearShortFormat = "MM.yy";

        public const string DateTimeMonthStrAndYearFormat = "MMMM yyyy";

        public const string DurationByLCFileName = "Продолжительность по трудозатратам";

        public const string CalendarPlanFileName = "Календарный план";

        public const string EnergyAndWaterFileName = "Энергия и вода";

        public static readonly Regex DecimalRegex = new(@"[\d,]+");
    }
}