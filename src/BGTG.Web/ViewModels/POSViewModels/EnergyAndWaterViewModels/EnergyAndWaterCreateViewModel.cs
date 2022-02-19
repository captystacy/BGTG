using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BGTG.Entities.Core;
using Calabonga.EntityFrameworkCore.Entities.Base;
using Microsoft.AspNetCore.Http;

namespace BGTG.Web.ViewModels.POSViewModels.EnergyAndWaterViewModels
{
    public class EnergyAndWaterCreateViewModel : IViewModel, IValidatableObject
    {
        public IFormFileCollection EstimateFiles { get; set; } = null!;
        public string ObjectCipher { get; set; } = null!;

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
        }
    }
}