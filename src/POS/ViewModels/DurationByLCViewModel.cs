using System.ComponentModel.DataAnnotations;

namespace POS.ViewModels
{
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
                yield return new ValidationResult("Estimate files are not found");
            }

            if (NumberOfWorkingDays <= 0)
            {
                yield return new ValidationResult("Number of working days can't be less or equal zero");
            }

            if (WorkingDayDuration <= 0)
            {
                yield return new ValidationResult("Working day duration can't be less or equal zero");
            }

            if (Shift <= 0)
            {
                yield return new ValidationResult("Shift can't be less or equal zero");
            }

            if (NumberOfEmployees <= 0)
            {
                yield return new ValidationResult("Number of employees can't be less or equal zero");
            }

            if (TechnologicalLaborCosts < 0)
            {
                yield return new ValidationResult("Technological labor costs can't be less than zero");
            }
        }
    }
}