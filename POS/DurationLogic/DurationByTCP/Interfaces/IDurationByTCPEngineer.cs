using System.Collections.Generic;
using POS.DurationLogic.DurationByTCP.TCP;

namespace POS.DurationLogic.DurationByTCP.Interfaces
{
    public interface IDurationByTCPEngineer
    {
        DurationCalculationType DurationCalculationType { get; }
        IEnumerable<PipelineStandard> CalculationPipelineStandards { get; set; }
        void DefineCalculationType(IEnumerable<PipelineStandard> pipelineStandards, decimal pipelineLength);
    }
}