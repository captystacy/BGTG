using System.ComponentModel.DataAnnotations;
using POS.DomainModels;
using POS.Infrastructure.Constants;

namespace POS.ViewModels;

public class TableOfContentsViewModel : IValidatableObject
{
    public string ObjectCipher { get; set; } = null!;
    public ProjectTemplate ProjectTemplate { get; set; }
    public ProjectEngineer ProjectEngineer { get; set; }
    public ChiefProjectEngineer ChiefProjectEngineer { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(ObjectCipher) || !(AppConstants.ObjectCipherExpression1.IsMatch(ObjectCipher) || AppConstants.ObjectCipherExpression2.IsMatch(ObjectCipher)))
        {
            yield return new ValidationResult(AppConstants.ObjectCipherValidationMessage);
        }
    }
}