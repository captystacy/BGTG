using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using BGTG.Core;
using BGTG.POS;

namespace BGTG.Web.ViewModels.POSViewModels
{
    public class TitlePageViewModel : IValidatableObject
    {
        public string ObjectCipher { get; set; }
        public string ObjectName { get; set; }
        public ChiefProjectEngineer ChiefProjectEngineer { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(ObjectCipher) || !(Regex.IsMatch(ObjectCipher, AppData.ObjectCipherRegexPattern1) || Regex.IsMatch(ObjectCipher, AppData.ObjectCipherRegexPattern2)))
            {
                yield return new ValidationResult(AppData.ObjectCipherValidationMessage);
            }

            if (string.IsNullOrEmpty(ObjectName))
            {
                yield return new ValidationResult(AppData.ObjectNameValidationMessage);
            }
        }
    }
}
