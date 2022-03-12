using System.ComponentModel.DataAnnotations;

namespace POS.ViewModels;

public class DurationByTCPViewModel : IValidatableObject
{
    public char AppendixKey { get; set; }
    public string PipelineCategoryName { get; set; } = null!;
    public string PipelineMaterial { get; set; } = null!;
    public int PipelineDiameter { get; set; }
    public decimal PipelineLength { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
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