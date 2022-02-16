using System;
using BGTG.Entities.BGTG;
using BGTG.Entities.POS.CalendarPlanToolEntities;
using BGTG.Entities.POS.DurationByLCToolEntities;
using BGTG.Entities.POS.DurationByTCPToolEntities;
using BGTG.Entities.POS.EnergyAndWaterToolEntities;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.Entities.POS
{
    public class POSEntity : Identity
    {
        public Guid ConstructionObjectId { get; set; }
        public ConstructionObjectEntity ConstructionObject { get; set; } = null!;
        public DurationByLCEntity? DurationByLC { get; set; }
        public InterpolationDurationByTCPEntity? InterpolationDurationByTCP { get; set; }
        public ExtrapolationDurationByTCPEntity? ExtrapolationDurationByTCP { get; set; }
        public StepwiseExtrapolationDurationByTCPEntity? StepwiseExtrapolationDurationByTCP { get; set; }
        public CalendarPlanEntity? CalendarPlan { get; set; }
        public EnergyAndWaterEntity? EnergyAndWater { get; set; }
    }
}
