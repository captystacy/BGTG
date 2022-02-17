using System;

namespace BGTG.Entities.POS.DurationByTCPToolEntities
{
    public class StepwisePipelineStandardEntity : PipelineStandardEntity
    {
        public Guid StepwiseExtrapolationDurationByTCPId { get; set; }
        public StepwiseExtrapolationDurationByTCPEntity StepwiseExtrapolationDurationByTCP { get; set; } = null!;
    }
}
