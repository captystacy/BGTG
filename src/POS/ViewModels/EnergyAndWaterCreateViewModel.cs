using System.ComponentModel.DataAnnotations;

namespace POS.ViewModels;

public class EnergyAndWaterCreateViewModel : IValidatableObject
{
    public IFormFileCollection EstimateFiles { get; set; } = null!;
    public string ObjectCipher { get; set; } = null!;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EstimateFiles.Count == 0)
        {
            yield return new ValidationResult(AppData.EstimateFilesValidationMessage);
        }

        if (string.IsNullOrEmpty(ObjectCipher) || !(AppData.ObjectCipherExpression1.IsMatch(ObjectCipher) || AppData.ObjectCipherExpression2.IsMatch(ObjectCipher)))
        {
            yield return new ValidationResult(AppData.ObjectCipherValidationMessage);
        }
    }
}