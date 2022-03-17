using System.ComponentModel.DataAnnotations;
using POS.DomainModels;
using POS.Infrastructure.Constants;

namespace POS.ViewModels;

public class ProjectViewModel : IValidatableObject
{
    public IFormFileCollection CalculationFiles { get; set; } = null!;
    public string ObjectCipher { get; set; } = null!;
    public ProjectTemplate ProjectTemplate { get; set; }
    public ProjectEngineer ProjectEngineer { get; set; }
    public ChiefProjectEngineer ChiefProjectEngineer { get; set; }
    public bool HouseholdTownIncluded { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(ObjectCipher))
        {
            yield return new ValidationResult(AppConstants.ObjectCipherValidationMessage);
        }

        if (CalculationFiles.Count == 0)
        {
            yield return new ValidationResult(AppConstants.EstimateFilesValidationMessage);
        }
    }
}