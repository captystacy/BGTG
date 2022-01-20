namespace POS.DurationLogic.DurationByLaborCosts.Interfaces
{
    public interface IDurationByLaborCostsWriter
    {
        void Write(DurationByLaborCosts durationByLaborCosts, string templatePath, string savePath);
    }
}
