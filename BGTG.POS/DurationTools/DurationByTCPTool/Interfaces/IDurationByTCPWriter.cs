namespace BGTG.POS.DurationTools.DurationByTCPTool.Interfaces
{
    public interface IDurationByTCPWriter
    {
        void Write(DurationByTCP durationByTCP, string templatePath, string savePath);
    }
}