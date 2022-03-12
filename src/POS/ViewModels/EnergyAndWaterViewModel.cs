using System.ComponentModel.DataAnnotations;

namespace POS.ViewModels;

public class EnergyAndWaterViewModel : IValidatableObject
{
    public IFormFileCollection EstimateFiles { get; set; } = null!;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EstimateFiles.Count == 0)
        {
            yield return new ValidationResult(AppData.EstimateFilesValidationMessage);
        }
    }
}