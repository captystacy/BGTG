namespace BGTG.POS.Tools.DurationTools.DurationByTCPTool.TCP.Interfaces
{
    public interface ITCP212Helper
    {
        Appendix GetAppendix(char appendixKey);
        PipelineCharacteristic GetPipelineCharacteristic(Appendix appendix, string pipelineMaterial, int pipelineDiameter, string pipelineCategoryName);
    }
}