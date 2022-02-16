using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BGTG.Entities.Core;
using BGTG.POS.EstimateTool;
using Microsoft.AspNetCore.Http;

namespace BGTG.Web.ViewModels.POS.CalendarPlanViewModels
{
    public class CalendarPlanPreCreateViewModel : IValidatableObject
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
}
