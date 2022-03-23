using System.ComponentModel.DataAnnotations;
using POS.DomainModels;
using POS.Infrastructure.Constants;

namespace POS.ViewModels;

public class ProjectViewModel : IValidatableObject
{
    public IFormFileCollection CalculationFiles { get; set; } = null!;
    public string ObjectCipher { get; set; } = null!;
    public ProjectTemplate ProjectTemplate { get; set; }
    public Engineer ChiefProjectEngineer { get; set; }
    public Engineer ChiefEngineer { get; set; }
    public Engineer NormalInspectionEngineer { get; set; }
    public Engineer ProjectEngineer { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!CalculationFiles.Any(x => x.FileName.Contains(AppConstants.DurationByLCFileName)))
        {
            yield return new ValidationResult("Не найден файл с продолжительностью по трудозатратам.");
        }

        if (!CalculationFiles.Any(x => x.FileName.Contains(AppConstants.CalendarPlanFileName)))
        {
            yield return new ValidationResult("Не найден файл с календарным планом.");
        }

        if (!CalculationFiles.Any(x => x.FileName.Contains(AppConstants.EnergyAndWaterFileName)))
        {
            yield return new ValidationResult("Не найден файл с энергией и водой.");
        }

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