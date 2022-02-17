using System;

namespace BGTG.Entities.POS.DurationByTCPToolEntities
{
    public class StepwiseExtrapolationPipelineStandardEntity : PipelineStandardEntity
    {
        public Guid StepwiseExtrapolationDurationByTCPId { get; set; }
        public StepwiseExtrapolationDurationByTCPEntity StepwiseExtrapolationDurationByTCP { get; set; } = null!;

    }
}
