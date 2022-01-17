namespace BGTGWeb.Models
{
    public class LaborCostsDurationVM
    {
        public decimal NumberOfWorkingDays { get; set; }
        public decimal WorkingDayDuration { get; set; }
        public decimal Shift { get; set; }
        public int NumberOfEmployees { get; set; }
        public decimal TechnologicalLaborCosts { get; set; }
        public bool AcceptanceTimeIncluded { get; set; }
    }
}
