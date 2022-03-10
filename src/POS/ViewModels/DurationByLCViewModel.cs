using System.ComponentModel.DataAnnotations;

namespace POS.ViewModels;

public class DurationByLCViewModel : IValidatableObject
{
    public IFormFileCollection EstimateFiles { get; set; } = null!;
    public decimal NumberOfWorkingDays { get; set; }
    public decimal WorkingDayDuration { get; set; }
    public decimal Shift { get; set; }
    public int NumberOfEmployees { get; set; }
    public decimal TechnologicalLaborCosts { get; set; }
    public bool AcceptanceTimeIncluded { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EstimateFiles.Count == 0)
        {
            yield return new ValidationResult(AppData.EstimateFilesValidationMessage);
        }

        if (NumberOfWorkingDays <= 0)
        {
            yield return new ValidationResult(AppData.NumberOfWorkingDaysValidationMessage);
        }

        if (WorkingDayDuration <= 0)
        {
            yield return new ValidationResult(AppData.WorkingDayDurationValidationMessage);
        }

        if (Shift <= 0)
        {
            yield return new ValidationResult(AppData.ShiftValidationMessage);
        }

        if (NumberOfEmployees <= 0)
        {
            yield return new ValidationResult(AppData.NumberOfEmployeesValidationMessage);
        }

        if (TechnologicalLaborCosts < 0)
        {
            yield return new ValidationResult(AppData.TechnologicalLaborCostsValidationMessage);
        }
    }
}