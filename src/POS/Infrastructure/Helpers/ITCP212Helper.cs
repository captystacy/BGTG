using POS.DomainModels.DurationByTCPDomainModels.TCPDomainModels;

namespace POS.Infrastructure.Helpers;

public interface ITCP212Helper
{
    Appendix GetAppendix(char appendixKey);
    PipelineCharacteristic? GetPipelineCharacteristic(Appendix appendix, string pipelineMaterial, int pipelineDiameter, string pipelineCategoryName);
}