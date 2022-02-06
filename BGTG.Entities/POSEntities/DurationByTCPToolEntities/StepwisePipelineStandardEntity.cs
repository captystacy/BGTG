using System;

namespace BGTG.Entities.POSEntities.DurationByTCPToolEntities
{
    public class StepwisePipelineStandardEntity : PipelineStandardEntity
    {
        public Guid StepwiseExtrapolationDurationByTCPId { get; set; }
        public StepwiseExtrapolationDurationByTCPEntity StepwiseExtrapolationDurationByTCP { get; set; }
    }
}
