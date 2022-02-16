namespace BGTG.POS.DurationTools.DurationByTCPTool.Base
{
    public interface IDurationByTCPCreator
    {
        DurationByTCP? Create(string pipelineMaterial, int pipelineDiameter, decimal pipelineLength, char appendixKey, string pipelineCategoryName);
    }
}
