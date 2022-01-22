using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BGTG.POS.Tools.EstimateTool;

namespace BGTG.POS.Web.ViewModels.CalendarPlanViewModels
{
    public class CalendarPlanViewModel :IValidatableObject, IEquatable<CalendarPlanViewModel>
    {
        public string ObjectCipher { get; set; }
        public DateTime ConstructionStartDate { get; set; }
        public int ConstructionDurationCeiling { get; set; }
        public virtual List<UserWorkViewModel> UserWorks { get; set; }
        public TotalWorkChapter TotalWorkChapter { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ConstructionStartDate.Year <= 1900)
            {
                yield return new ValidationResult("Год должен быть больше 1900.");
            }

            if (ConstructionDurationCeiling < 1 || ConstructionDurationCeiling > 21)
            {
                yield return new ValidationResult("Месяц должен быть в диапазоне от 1 до 21.");
            }
        }

        public bool Equals(CalendarPlanViewModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ObjectCipher == other.ObjectCipher && ConstructionStartDate.Equals(other.ConstructionStartDate) &&
                   ConstructionDurationCeiling == other.ConstructionDurationCeiling &&
                   UserWorks.SequenceEqual(other.UserWorks) && TotalWorkChapter == other.TotalWorkChapter;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CalendarPlanViewModel)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ObjectCipher, ConstructionStartDate, ConstructionDurationCeiling, UserWorks, (int)TotalWorkChapter);
        }

        public static bool operator ==(CalendarPlanViewModel left, CalendarPlanViewModel right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CalendarPlanViewModel left, CalendarPlanViewModel right)
        {
            return !Equals(left, right);
        }
    }
}
