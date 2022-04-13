using System.ComponentModel.DataAnnotations;
using POS.Models;

namespace POS.ViewModels
{
    public class TitlePageViewModel : IValidatableObject
    {
        public string ObjectCipher { get; set; } = null!;
        public string ObjectName { get; set; } = null!;
        public Engineer ChiefProjectEngineer { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(ObjectCipher))
            {
                yield return new ValidationResult("Object cipher could not be empty");
            }

            if (string.IsNullOrEmpty(ObjectName))
            {
                yield return new ValidationResult("Object name could not be empty");
            }
        }
    }
}