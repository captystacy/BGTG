namespace POSCore.DurationLogic.LaborCosts.Interfaces
{
    public interface ILaborCostsDurationWriter
    {
        void Write(LaborCostsDuration laborCostsDuration, string templatePath, string savePath, string fileName);
    }
}
