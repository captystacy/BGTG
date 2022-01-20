using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BGTGWeb.ViewModels.Attributes;
using POS.EstimateLogic;

namespace BGTGWeb.ViewModels
{
    public class CalendarPlanViewModel
    {
        public string ObjectCipher { get; set; }

        [MinDateYear(1900)]
        public DateTime ConstructionStartDate { get; set; }

        [Range(1, 21)]
        public int ConstructionDurationCeiling { get; set; }
        public virtual List<UserWorkViewModel> UserWorks { get; set; }
        public TotalWorkChapter TotalWorkChapter { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as CalendarPlanViewModel;

            if (other == null)
            {
                return false;
            }

            return ObjectCipher == other.ObjectCipher
                && ConstructionStartDate == other.ConstructionStartDate
                && ConstructionDurationCeiling == other.ConstructionDurationCeiling
                && UserWorks.SequenceEqual(other.UserWorks);
        }

        public override int GetHashCode() => HashCode.Combine(ObjectCipher, ConstructionStartDate, ConstructionDurationCeiling, UserWorks);
    }
}
