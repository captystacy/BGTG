using POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.Models;

namespace POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.Base;

public interface IDurationByTCPCreator
{
    DurationByTCP? Create(string pipelineMaterial, int pipelineDiameter, decimal pipelineLength, char appendixKey, string pipelineCategoryName);
}