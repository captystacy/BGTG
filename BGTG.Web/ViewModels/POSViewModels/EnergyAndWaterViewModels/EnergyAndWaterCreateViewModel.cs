using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using BGTG.Core;
using Calabonga.EntityFrameworkCore.Entities.Base;
using Microsoft.AspNetCore.Http;

namespace BGTG.Web.ViewModels.POSViewModels.EnergyAndWaterViewModels
{
    public class EnergyAndWaterCreateViewModel : IViewModel, IValidatableObject
    {
        public IFormFileCollection EstimateFiles { get; set; }
        public string ObjectCipher { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EstimateFiles == null || EstimateFiles.Count == 0)
            {
                yield return new ValidationResult(AppData.EstimateFilesValidationMessage);
            }

            if (string.IsNullOrEmpty(ObjectCipher) || !(Regex.IsMatch(ObjectCipher, AppData.ObjectCipherRegexPattern1) || Regex.IsMatch(ObjectCipher, AppData.ObjectCipherRegexPattern2)))
            {
                yield return new ValidationResult(AppData.ObjectCipherValidationMessage);
            }
        }
    }
}