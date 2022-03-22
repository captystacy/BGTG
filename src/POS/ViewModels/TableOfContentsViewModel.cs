using System.ComponentModel.DataAnnotations;
using POS.DomainModels;
using POS.Infrastructure.Constants;

namespace POS.ViewModels;

public class TableOfContentsViewModel : IValidatableObject
{
    public string ObjectCipher { get; set; } = null!;
    public ProjectTemplate ProjectTemplate { get; set; }
    public Engineer NormalInspectionEngineer { get; set; }
    public Engineer ChiefProjectEngineer { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(ObjectCipher))
        {
            yield return new ValidationResult(AppConstants.ObjectCipherValidationMessage);
        }
    }
}