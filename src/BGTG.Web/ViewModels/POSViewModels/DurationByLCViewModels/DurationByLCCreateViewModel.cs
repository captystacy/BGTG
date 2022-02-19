﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BGTG.Entities.Core;
using Calabonga.EntityFrameworkCore.Entities.Base;
using Microsoft.AspNetCore.Http;

namespace BGTG.Web.ViewModels.POSViewModels.DurationByLCViewModels
{
    public class DurationByLCCreateViewModel : IViewModel, IValidatableObject
    {
        public IFormFileCollection EstimateFiles { get; set; } = null!;
        public string ObjectCipher { get; set; } = null!;
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

            if (string.IsNullOrEmpty(ObjectCipher) || !(AppData.ObjectCipherExpression1.IsMatch(ObjectCipher) || AppData.ObjectCipherExpression2.IsMatch(ObjectCipher)))
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