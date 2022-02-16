namespace BGTG.POS.DurationTools.DurationByTCPTool.Base
{
    public interface IDurationByTCPWriter
    {
        void Write(DurationByTCP durationByTCP, string templatePath, string savePath);
    }
}