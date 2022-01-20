using System.ComponentModel.DataAnnotations;

namespace BGTGWeb.ViewModels
{
    public class DurationByLaborCostsViewModel
    {
        [Range(0, double.MaxValue)]
        public decimal NumberOfWorkingDays { get; set; }

        [Range(0, double.MaxValue)]
        public decimal WorkingDayDuration { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Shift { get; set; }

        [Range(0, int.MaxValue)]
        public int NumberOfEmployees { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TechnologicalLaborCosts { get; set; }
        public bool AcceptanceTimeIncluded { get; set; }
    }
}
