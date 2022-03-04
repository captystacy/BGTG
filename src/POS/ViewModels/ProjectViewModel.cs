using System.ComponentModel.DataAnnotations;
using POS.Infrastructure.Tools;
using POS.Infrastructure.Tools.ProjectTool;

namespace POS.ViewModels;

public class ProjectViewModel : IValidatableObject
{
    public string ObjectCipher { get; set; } = null!;
    public ProjectTemplate ProjectTemplate { get; set; }
    public ChiefProjectEngineer ChiefProjectEngineer { get; set; }
    public bool HouseholdTownIncluded { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(ObjectCipher) || !(AppData.ObjectCipherExpression1.IsMatch(ObjectCipher) || AppData.ObjectCipherExpression2.IsMatch(ObjectCipher)))
        {
            yield return new ValidationResult(AppData.ObjectCipherValidationMessage);
        }
    }
}