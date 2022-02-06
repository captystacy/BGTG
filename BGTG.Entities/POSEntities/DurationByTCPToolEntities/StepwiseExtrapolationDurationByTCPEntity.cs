using System;
using System.Collections.Generic;

namespace BGTG.Entities.POSEntities.DurationByTCPToolEntities
{
    public class StepwiseExtrapolationDurationByTCPEntity : DurationByTCPEntity
    {
        public decimal VolumeChangePercent { get; set; }
        public decimal StandardChangePercent { get; set; }
        public decimal StepwiseDuration { get; set; }
        public StepwisePipelineStandardEntity StepwisePipelineStandard { get; set; }
        public ICollection<StepwiseExtrapolationPipelineStandardEntity> CalculationPipelineStandards { get; set; }
    }
}
