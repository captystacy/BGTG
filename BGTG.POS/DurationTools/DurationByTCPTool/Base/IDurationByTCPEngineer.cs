using System.Collections.Generic;
using BGTG.POS.DurationTools.DurationByTCPTool.TCP;

namespace BGTG.POS.DurationTools.DurationByTCPTool.Base
{
    public interface IDurationByTCPEngineer
    {
        DurationCalculationType DurationCalculationType { get; }
        IEnumerable<PipelineStandard> CalculationPipelineStandards { get; set; }
        void DefineCalculationType(IEnumerable<PipelineStandard> pipelineStandards, decimal pipelineLength);
    }
}