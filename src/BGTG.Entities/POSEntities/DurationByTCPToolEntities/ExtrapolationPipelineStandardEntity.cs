using System;

namespace BGTG.Entities.POSEntities.DurationByTCPToolEntities
{
    public class ExtrapolationPipelineStandardEntity : PipelineStandardEntity
    {
        public Guid ExtrapolationDurationByTCPId { get; set; }
        public ExtrapolationDurationByTCPEntity ExtrapolationDurationByTCP { get; set; } = null!;
    }
}
