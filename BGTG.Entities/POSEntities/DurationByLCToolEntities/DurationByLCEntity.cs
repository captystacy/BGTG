using System;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.Entities.POSEntities.DurationByLCToolEntities
{
    public class DurationByLCEntity : Auditable
    {
        public decimal Duration { get; set; }
        public decimal TotalLaborCosts { get; set; }
        public decimal EstimateLaborCosts { get; set; }
        public decimal TechnologicalLaborCosts { get; set; }
        public decimal WorkingDayDuration { get; set; }
        public decimal Shift { get; set; }
        public decimal NumberOfWorkingDays { get; set; }
        public int NumberOfEmployees { get; set; }
        public decimal RoundedDuration { get; set; }
        public decimal TotalDuration { get; set; }
        public decimal PreparatoryPeriod { get; set; }
        public decimal AcceptanceTime { get; set; }
        public bool AcceptanceTimeIncluded { get; set; }
        public bool RoundingIncluded { get; set; }
        public Guid POSId { get; set; }
        public POSEntity POS { get; set; }
    }
}
