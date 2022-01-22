namespace BGTG.POS.Tools.DurationTools.DurationByLCTool.Interfaces
{
    public interface IDurationByLCWriter
    {
        void Write(DurationByLC durationByLC, string templatePath, string savePath);
    }
}
