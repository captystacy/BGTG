using System.Collections.Generic;

namespace BGTG.Entities.POSEntities.DurationByTCPToolEntities
{
    public class InterpolationDurationByTCPEntity : DurationByTCPEntity
    {
        public decimal DurationChange { get; set; }
        public decimal VolumeChange { get; set; }
        public ICollection<InterpolationPipelineStandardEntity> CalculationPipelineStandards { get; set; } = null!;
    }
}
