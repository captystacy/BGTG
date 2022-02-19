using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BGTG.Entities.Core;
using BGTG.POS;
using BGTG.POS.ProjectTool;

namespace BGTG.Web.ViewModels.POSViewModels
{
    public class TableOfContentsViewModel : IValidatableObject
    {
        public string ObjectCipher { get; set; } = null!;
        public ProjectTemplate ProjectTemplate { get; set; }
        public ChiefProjectEngineer ChiefProjectEngineer { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(ObjectCipher) || !(AppData.ObjectCipherExpression1.IsMatch(ObjectCipher) || AppData.ObjectCipherExpression2.IsMatch(ObjectCipher)))
            {
                yield return new ValidationResult(AppData.ObjectCipherValidationMessage);
            }
        }
    }
}
