using System.Text.RegularExpressions;

namespace BGTG.Entities.Core;

public partial class AppData
{

    public const string Unknown = "Unknown";

    public const string DocxMimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

    public const string DecimalFormat = "G29";

    public const string DecimalThreePlacesFormat = "F3";

    public const string PercentFormat = "P2";

    public const string DateTimeMonthAndYearFormat = "MMMM yyyy";

    public const string DateTimeMonthAndYearShortFormat = "MM.yy";

    public static readonly Regex ObjectCipherExpression1 = new(@"^\d\.\d-\d+\.\d+$");
    public static readonly Regex ObjectCipherExpression2 = new(@"^\d\.\d-\d+\.\d+-\d+$");

    public const string ObjectCipherValidationMessage = "Шифр объекта указан в неправильном формате. Правильный формат: Ц.Ц-Ч.Ч или Ц.Ц-Ч.Ч-Ч, где Ц - цифра, Ч - число.";
}