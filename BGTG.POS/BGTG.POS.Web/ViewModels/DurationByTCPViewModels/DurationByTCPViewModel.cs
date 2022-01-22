using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BGTG.POS.Core;

namespace BGTG.POS.Web.ViewModels.DurationByTCPViewModels
{
    public class DurationByTCPViewModel : IValidatableObject
    {
        public char AppendixKey { get; set; }
        public string PipelineCategoryName { get; set; }
        public string PipelineMaterial { get; set; }
        public int PipelineDiameter { get; set; }
        public decimal PipelineLength { get; set; }
        public string UserFullName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PipelineDiameter < 0
                || PipelineLength < 0)
            {
                yield return new ValidationResult(AppData.Messages.NegativeValuesAreUnacceptable);
            }
        }
    }
}