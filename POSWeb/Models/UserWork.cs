using System.Collections.Generic;

namespace POSWeb.Models
{
    public class UserWork
    {
        public virtual string WorkName { get; set; }
        public int Chapter { get; set; }
        public virtual List<decimal> Percentages { get; set; }
    }
}
