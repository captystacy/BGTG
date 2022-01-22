namespace BGTG.POS.Tools.DurationTools.Interfaces
{
    public interface IDurationRounder
    {
        decimal GetRoundedDuration(decimal duration);
        decimal GetRoundedPreparatoryPeriod(decimal totalDuration);
    }
}