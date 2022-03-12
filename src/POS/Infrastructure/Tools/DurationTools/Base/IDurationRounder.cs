namespace POS.Infrastructure.Tools.DurationTools.Base;

public interface IDurationRounder
{
    decimal GetRoundedDuration(decimal duration);
    decimal GetRoundedPreparatoryPeriod(decimal totalDuration);
}