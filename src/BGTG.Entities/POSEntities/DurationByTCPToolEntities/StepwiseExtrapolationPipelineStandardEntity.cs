using System;

namespace BGTG.Entities.POSEntities.DurationByTCPToolEntities
{
    public class StepwiseExtrapolationPipelineStandardEntity : PipelineStandardEntity
    {
        public Guid StepwiseExtrapolationDurationByTCPId { get; set; }
        public StepwiseExtrapolationDurationByTCPEntity StepwiseExtrapolationDurationByTCP { get; set; } = null!;

    }
}
