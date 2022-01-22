using System.Collections.Generic;
using BGTG.POS.Tools.DurationTools.DurationByTCPTool.TCP;

namespace BGTG.POS.Tools.DurationTools.DurationByTCPTool.Interfaces
{
    public interface IDurationByTCPEngineer
    {
        DurationCalculationType DurationCalculationType { get; }
        IEnumerable<PipelineStandard> CalculationPipelineStandards { get; set; }
        void DefineCalculationType(IEnumerable<PipelineStandard> pipelineStandards, decimal pipelineLength);
    }
}