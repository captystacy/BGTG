using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BGTG.Core;
using BGTG.POS.EstimateTool;
using Microsoft.AspNetCore.Http;

namespace BGTG.Web.ViewModels.POSViewModels.CalendarPlanViewModels
{
    public class CalendarPlanPreCreateViewModel : IValidatableObject
    {
        public IFormFileCollection EstimateFiles { get; set; }
        public TotalWorkChapter TotalWorkChapter { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EstimateFiles == null || EstimateFiles.Count == 0)
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
