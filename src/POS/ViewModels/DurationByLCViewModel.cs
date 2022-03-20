using System.ComponentModel.DataAnnotations;
using POS.Infrastructure.Constants;

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
            yield return new ValidationResult(AppConstants.EstimateFilesValidationMessage);
        }

        if (NumberOfWorkingDays <= 0)
        {
            yield return new ValidationResult("Количество рабочих дней в месяце не может быть отрицательным или равным нулю.");
        }

        if (WorkingDayDuration <= 0)
        {
            yield return new ValidationResult("Продолжительность рабочего дня не может быть отрицательной или равной нулю.");
        }

        if (Shift <= 0)
        {
            yield return new ValidationResult("Сменность не может быть отрицательной или равной нулю.");
        }

        if (NumberOfEmployees <= 0)
        {
            yield return new ValidationResult("Количество работающих в бригаде не может быть отрицательным или равным нулю.");
        }

        if (TechnologicalLaborCosts < 0)
        {
            yield return new ValidationResult("Трудозатраты по технологической карте не могут быть отрицательными или равными нулю.");
        }
    }
}