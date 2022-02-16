using System.Collections.Generic;

namespace BGTG.Entities.POS.DurationByTCPToolEntities
{
    public class StepwiseExtrapolationDurationByTCPEntity : DurationByTCPEntity
    {
        public decimal VolumeChangePercent { get; set; }
        public decimal StandardChangePercent { get; set; }
        public decimal StepwiseDuration { get; set; }
        public StepwisePipelineStandardEntity StepwisePipelineStandard { get; set; } = null!;
        public ICollection<StepwiseExtrapolationPipelineStandardEntity> CalculationPipelineStandards { get; set; } = null!;
    }
}
