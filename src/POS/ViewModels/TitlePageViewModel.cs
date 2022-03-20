using System.ComponentModel.DataAnnotations;
using POS.DomainModels;
using POS.Infrastructure.Constants;

namespace POS.ViewModels;

public class TitlePageViewModel : IValidatableObject
{
    public string ObjectCipher { get; set; } = null!;
    public string ObjectName { get; set; } = null!;
    public ChiefProjectEngineer ChiefProjectEngineer { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(ObjectCipher))
        {
            yield return new ValidationResult(AppConstants.ObjectCipherValidationMessage);
        }

        if (string.IsNullOrEmpty(ObjectName))
        {
            yield return new ValidationResult("Имя объекта не может быть пустым.");
        }
    }
}