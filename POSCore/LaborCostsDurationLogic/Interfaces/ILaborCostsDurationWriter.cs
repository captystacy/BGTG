namespace POS.LaborCostsDurationLogic.Interfaces
{
    public interface ILaborCostsDurationWriter
    {
        void Write(LaborCostsDuration laborCostsDuration, string templatePath, string savePath);
    }
}
