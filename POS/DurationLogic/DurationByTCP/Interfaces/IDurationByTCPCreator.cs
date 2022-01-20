namespace POS.DurationLogic.DurationByTCP.Interfaces
{
    public interface IDurationByTCPCreator
    {
        DurationByTCP Create(string pipelineMaterial, int pipelineDiameter, decimal pipelineLength, char appendixKey, string pipelineCategoryName);
    }
}
