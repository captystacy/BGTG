using POS.DomainModels.DurationByTCPDomainModels;
using POS.DomainModels.DurationByTCPDomainModels.TCPDomainModels;

namespace POS.Infrastructure.Engineers;

public interface IDurationByTCPEngineer
{
    DurationCalculationType DurationCalculationType { get; }
    IEnumerable<PipelineStandard> CalculationPipelineStandards { get; set; }
    void DefineCalculationType(IEnumerable<PipelineStandard> pipelineStandards, decimal pipelineLength);
}