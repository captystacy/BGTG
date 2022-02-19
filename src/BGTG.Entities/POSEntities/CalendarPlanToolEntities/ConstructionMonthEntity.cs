using System;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.Entities.POSEntities.CalendarPlanToolEntities
{
    public abstract class ConstructionMonthEntity : Identity
    {
        public DateTime Date { get; set; }
        public decimal InvestmentVolume { get; set; }
        public decimal VolumeCAIW { get; set; }
        public decimal PercentPart { get; set; }
        public int CreationIndex { get; set; }
    }
}
