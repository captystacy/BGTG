namespace POS.DurationLogic.DurationByTCP.Interfaces
{
    public interface IDurationByTCPWriter
    {
        void Write(DurationByTCP durationByTCP, string templatePath, string savePath);
    }
}