namespace POS.Infrastructure.Rounders;

public interface IDurationRounder
{
    decimal GetRoundedDuration(decimal duration);
    decimal GetRoundedPreparatoryPeriod(decimal totalDuration);
}