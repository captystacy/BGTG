namespace BGTG.POS.DurationTools.Base
{
    public interface IDurationRounder
    {
        decimal GetRoundedDuration(decimal duration);
        decimal GetRoundedPreparatoryPeriod(decimal totalDuration);
    }
}