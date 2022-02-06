using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using BGTG.Core;
using Calabonga.EntityFrameworkCore.Entities.Base;
using Microsoft.AspNetCore.Http;

namespace BGTG.Web.ViewModels.POSViewModels.DurationByLCViewModels
{
    public class DurationByLCCreateViewModel : IViewModel, IValidatableObject
    {
        public IFormFileCollection EstimateFiles { get; set; }
        public string ObjectCipher { get; set; }
        public decimal NumberOfWorkingDays { get; set; }
        public decimal WorkingDayDuration { get; set; }
        public decimal Shift { get; set; }
        public int NumberOfEmployees { get; set; }
        public decimal TechnologicalLaborCosts { get; set; }
        public bool AcceptanceTimeIncluded { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EstimateFiles == null || EstimateFiles.Count == 0)
            {
                yield return new ValidationResult(AppData.EstimateFilesValidationMessage);
            }

            if (string.IsNullOrEmpty(ObjectCipher) || !Regex.IsMatch(ObjectCipher, AppData.ObjectCipherRegexPattern))
            {
                yield return new ValidationResult(AppData.ObjectCipherValidationMessage);
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
}
