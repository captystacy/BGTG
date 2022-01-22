using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BGTG.POS.Core;

namespace BGTG.POS.Web.ViewModels.DurationByLCViewModels
{
    public class DurationByLCViewModel : IValidatableObject
    {
        public decimal NumberOfWorkingDays { get; set; }
        public decimal WorkingDayDuration { get; set; }
        public decimal Shift { get; set; }
        public int NumberOfEmployees { get; set; }
        public decimal TechnologicalLaborCosts { get; set; }
        public bool AcceptanceTimeIncluded { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NumberOfWorkingDays < 0
                || WorkingDayDuration < 0
                || Shift < 0
                || NumberOfEmployees < 0
                || TechnologicalLaborCosts < 0)
            {
                yield return new ValidationResult(AppData.Messages.NegativeValuesAreUnacceptable);
            }
        }
    }
}
