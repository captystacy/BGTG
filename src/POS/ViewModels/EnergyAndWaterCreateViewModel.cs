using System.ComponentModel.DataAnnotations;
using POS.Infrastructure.Constants;

namespace POS.ViewModels;

public class EnergyAndWaterCreateViewModel : IValidatableObject
{
    public IFormFileCollection EstimateFiles { get; set; } = null!;
    public string ObjectCipher { get; set; } = null!;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EstimateFiles.Count == 0)
        {
            yield return new ValidationResult(AppConstants.EstimateFilesValidationMessage);
        }

        if (string.IsNullOrEmpty(ObjectCipher) || !(AppConstants.ObjectCipherExpression1.IsMatch(ObjectCipher) || AppConstants.ObjectCipherExpression2.IsMatch(ObjectCipher)))
        {
            yield return new ValidationResult(AppConstants.ObjectCipherValidationMessage);
        }
    }
}