using System.Collections.Generic;

namespace BGTG.Entities.POS.DurationByTCPToolEntities
{
    public class InterpolationDurationByTCPEntity : DurationByTCPEntity
    {
        public decimal DurationChange { get; set; }
        public decimal VolumeChange { get; set; }
        public ICollection<InterpolationPipelineStandardEntity> CalculationPipelineStandards { get; set; } = null!;
    }
}
