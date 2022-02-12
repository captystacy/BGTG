using System.Collections.Generic;

namespace BGTG.Core
{
    /// <summary>
    /// Static data container
    /// </summary>
    public static partial class AppData
    {
        /// <summary>
        /// Current service name
        /// </summary>
        public const string ServiceName = "Module";

        /// <summary>
        /// "SystemAdministrator"
        /// </summary>
        public const string SystemAdministratorRoleName = "Administrator";

        /// <summary>
        /// "BusinessOwner"
        /// </summary>
        public const string ManagerRoleName = "Manager";

        /// <summary>
        /// Roles
        /// </summary>
        public static IEnumerable<string> Roles
        {
            get
            {
                yield return SystemAdministratorRoleName;
                yield return ManagerRoleName;
            }
        }

        /// <summary>
        /// IdentityServer4 path
        /// </summary>
        public const string AuthUrl = "/auth";

        public const string Unknown = "Unknown";

        public const string DocxMimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

        public const string DecimalFormat = "G29";

        public const string DecimalThreePlacesFormat = "F3";

        public const string PercentFormat = "P2";

        public const string DateTimeMonthAndYearFormat = "MMMM yyyy";

        public const string DateTimeMonthAndYearShortFormat = "MM.yy";

        public const string ObjectCipherRegexPattern1 = @"^\d\.\d-\d+\.\d+$";
        public const string ObjectCipherRegexPattern2 = @"^\d\.\d-\d+\.\d+-\d+$";

        public const string ObjectCipherValidationMessage = "Шифр объекта указан в неправильном формате. Правильный формат: Ц.Ц-Ч.Ч или Ц.Ц-Ч.Ч-Ч, где Ц - цифра, Ч - число.";
    }
}
