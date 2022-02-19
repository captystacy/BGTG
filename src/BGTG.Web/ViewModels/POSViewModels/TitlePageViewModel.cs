using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BGTG.Entities.Core;
using BGTG.POS;

namespace BGTG.Web.ViewModels.POSViewModels
{
    public class TitlePageViewModel : IValidatableObject
    {
        public string ObjectCipher { get; set; } = null!;
        public string ObjectName { get; set; } = null!;
        public ChiefProjectEngineer ChiefProjectEngineer { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(ObjectCipher) || !(AppData.ObjectCipherExpression1.IsMatch(ObjectCipher) || AppData.ObjectCipherExpression2.IsMatch(ObjectCipher)))
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
