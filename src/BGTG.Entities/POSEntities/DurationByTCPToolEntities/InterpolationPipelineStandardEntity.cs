using System;

namespace BGTG.Entities.POSEntities.DurationByTCPToolEntities
{
    public class InterpolationPipelineStandardEntity : PipelineStandardEntity
    {
        public Guid InterpolationDurationByTCPId { get; set; }
        public InterpolationDurationByTCPEntity InterpolationDurationByTCP { get; set; } = null!;
    }
}
