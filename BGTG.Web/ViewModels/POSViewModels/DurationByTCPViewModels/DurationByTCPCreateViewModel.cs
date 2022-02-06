using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using BGTG.Core;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.Web.ViewModels.POSViewModels.DurationByTCPViewModels
{
    public class DurationByTCPCreateViewModel : IViewModel, IValidatableObject
    {
        public string ObjectCipher { get; set; }
        public char AppendixKey { get; set; }
        public string PipelineCategoryName { get; set; }
        public string PipelineMaterial { get; set; }
        public int PipelineDiameter { get; set; }
        public decimal PipelineLength { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(ObjectCipher) || !Regex.IsMatch(ObjectCipher, AppData.ObjectCipherRegexPattern))
            {
                yield return new ValidationResult(AppData.ObjectCipherValidationMessage);
            }

            if (PipelineDiameter <= 0)
            {
                yield return new ValidationResult(AppData.PipelineDiameterValidationMessage);

            }

            if (PipelineLength <= 0)
            {
                yield return new ValidationResult(AppData.PipelineLengthValidationMessage);
            }
        }
    }
}