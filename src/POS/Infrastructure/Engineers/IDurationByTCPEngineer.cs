using POS.Models.DurationByTCPModels;
using POS.Models.DurationByTCPModels.TCPModels;

namespace POS.Infrastructure.Engineers
{
    public interface IDurationByTCPEngineer
    {
        DurationCalculationType DurationCalculationType { get; }
        IEnumerable<PipelineStandard> CalculationPipelineStandards { get; set; }
        void DefineCalculationType(IEnumerable<PipelineStandard> pipelineStandards, decimal pipelineLength);
        Appendix GetAppendix(char appendixKey);
        PipelineCharacteristic? GetPipelineCharacteristic(Appendix appendix, string pipelineMaterial, int pipelineDiameter, string pipelineCategoryName);
    }
}