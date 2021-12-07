using System;
using System.Collections.Generic;

namespace POSWeb.Models
{
    public class CalendarPlanVM
    {
        public string ObjectCipher { get; set; }
        public DateTime ConstructionStartDate { get; set; }
        public decimal ConstructionDuration { get; set; }
        public virtual List<UserWork> UserWorks { get; set; }
    }
}
