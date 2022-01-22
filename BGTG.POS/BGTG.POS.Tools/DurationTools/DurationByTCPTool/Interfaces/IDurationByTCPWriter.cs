namespace BGTG.POS.Tools.DurationTools.DurationByTCPTool.Interfaces
{
    public interface IDurationByTCPWriter
    {
        void Write(DurationByTCP durationByTCP, string templatePath, string savePath);
    }
}