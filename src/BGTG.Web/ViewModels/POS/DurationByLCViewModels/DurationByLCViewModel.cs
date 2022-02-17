using System;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.Web.ViewModels.POS.DurationByLCViewModels
{
    public class DurationByLCViewModel : ViewModelBase
    {
        public decimal Duration { get; set; }
        public decimal TotalLaborCosts { get; set; }
        public int NumberOfEmployees { get; set; }
        public decimal TotalDuration { get; set; }
        public decimal PreparatoryPeriod { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
    }
}