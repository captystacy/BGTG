using POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.Models;
using POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.TCP.Models;

namespace POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.Base;

public interface IDurationByTCPEngineer
{
    DurationCalculationType DurationCalculationType { get; }
    IEnumerable<PipelineStandard> CalculationPipelineStandards { get; set; }
    void DefineCalculationType(IEnumerable<PipelineStandard> pipelineStandards, decimal pipelineLength);
}