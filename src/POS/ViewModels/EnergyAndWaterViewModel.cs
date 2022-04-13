using System.ComponentModel.DataAnnotations;
using POS.Models.EstimateModels;

namespace POS.ViewModels
{
    public class EnergyAndWaterViewModel : IValidatableObject
    {
        public IFormFileCollection EstimateFiles { get; set; } = null!;
        public TotalWorkChapter TotalWorkChapter { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EstimateFiles.Count == 0)
            {
                yield return new ValidationResult("Estimate files was not found");
            }

            if (TotalWorkChapter is TotalWorkChapter.None)
            {
                yield return new ValidationResult("Total wok chapter was not set");
            }
        }
    }
}