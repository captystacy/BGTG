using System.ComponentModel.DataAnnotations;
using POS.Infrastructure.AppConstants;
using POS.Models;

namespace POS.ViewModels
{
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
            if (!CalculationFiles.Any(x => x.FileName.Contains(Constants.DurationByLCFileName)))
            {
                yield return new ValidationResult("Duration by labor costs file is not found");
            }

            if (!CalculationFiles.Any(x => x.FileName.Contains(Constants.CalendarPlanFileName)))
            {
                yield return new ValidationResult("Calendar plan file is not found");
            }

            if (!CalculationFiles.Any(x => x.FileName.Contains(Constants.EnergyAndWaterFileName)))
            {
                yield return new ValidationResult("Energy and water file is not found");
            }

            if (string.IsNullOrEmpty(ObjectCipher))
            {
                yield return new ValidationResult("Object cipher could not be empty");
            }

            if (CalculationFiles.Count == 0)
            {
                yield return new ValidationResult("Calculation files are not found");
            }
        }
    }
}