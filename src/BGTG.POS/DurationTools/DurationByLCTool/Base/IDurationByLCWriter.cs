namespace BGTG.POS.DurationTools.DurationByLCTool.Base
{
    public interface IDurationByLCWriter
    {
        void Write(DurationByLC durationByLC, string templatePath, string savePath);
    }
}
