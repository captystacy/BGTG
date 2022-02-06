using System;
using BGTG.Entities.POSEntities.CalendarPlanToolEntities;
using BGTG.Entities.POSEntities.DurationByLCToolEntities;
using BGTG.Entities.POSEntities.DurationByTCPToolEntities;
using BGTG.Entities.POSEntities.EnergyAndWaterToolEntities;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.Entities.POSEntities
{
    public class POSEntity : Identity
    {
        public Guid ConstructionObjectId { get; set; }
        public ConstructionObjectEntity ConstructionObject { get; set; }
        public CalendarPlanEntity CalendarPlan { get; set; }
        public EnergyAndWaterEntity EnergyAndWater { get; set; }
        public DurationByLCEntity DurationByLC { get; set; }
        public InterpolationDurationByTCPEntity InterpolationDurationByTCP { get; set; }
        public ExtrapolationDurationByTCPEntity ExtrapolationDurationByTCP { get; set; }
        public StepwiseExtrapolationDurationByTCPEntity StepwiseExtrapolationDurationByTCP { get; set; }
    }
}
