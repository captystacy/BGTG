namespace POS.Infrastructure.Rounders
{
    public interface IDurationRounder
    {
        Task<decimal> GetRoundedDuration(decimal duration);
        Task<decimal> GetRoundedPreparatoryPeriod(decimal totalDuration);
    }
}