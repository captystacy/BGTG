using System.ComponentModel.DataAnnotations;
using POS.Infrastructure.Tools.EstimateTool.Models;

namespace POS.ViewModels;

public class CalendarPlanCreateViewModel : IValidatableObject
{
    public IFormFileCollection EstimateFiles { get; set; } = null!;
    public TotalWorkChapter TotalWorkChapter { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EstimateFiles.Count == 0)
        {
            yield return new ValidationResult(AppData.EstimateFilesValidationMessage);
        }

        var totalWorkChapterInt = (int)TotalWorkChapter;
        if (totalWorkChapterInt != 9 && totalWorkChapterInt != 11 && totalWorkChapterInt != 12)
        {
            yield return new ValidationResult(AppData.TotalWorkChapterValidationMessage);
        }
    }
}