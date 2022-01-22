namespace BGTG.POS.Tools.DurationTools.DurationByTCPTool.Interfaces
{
    public interface IDurationByTCPCreator
    {
        DurationByTCP Create(string pipelineMaterial, int pipelineDiameter, decimal pipelineLength, char appendixKey, string pipelineCategoryName);
    }
}
