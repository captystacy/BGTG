namespace POS.DurationLogic.Interfaces
{
    public interface IDurationRounder
    {
        decimal GetRoundedDuration(decimal duration);
        decimal GetRoundedPreparatoryPeriod(decimal totalDuration);
    }
}