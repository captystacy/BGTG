using System.Collections.Generic;
using BGTG.POS.Tools.DurationTools.DurationByTCPTool.TCP;

namespace BGTG.POS.Tools.DurationTools.DurationByTCPTool
{
    public class InterpolationDurationByTCP : DurationByTCP
    {
        public decimal DurationChange { get; }
        public decimal VolumeChange { get; }

        public InterpolationDurationByTCP(string pipelineMaterial, int pipelineDiameter, string pipelineDiameterPresentation, decimal pipelineLength, IEnumerable<PipelineStandard> calculationPipelineStandards, DurationCalculationType durationCalculationType, decimal duration, decimal roundedDuration, decimal preparatoryPeriod, decimal durationChange, decimal volumeChange, Appendix appendix) 
            : base(pipelineMaterial, pipelineDiameter, pipelineDiameterPresentation, pipelineLength, calculationPipelineStandards, durationCalculationType, duration, roundedDuration, preparatoryPeriod, appendix)
        {
            DurationChange = durationChange;
            VolumeChange = volumeChange;
        }
    }
}
