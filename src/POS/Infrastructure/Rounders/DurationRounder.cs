namespace POS.Infrastructure.Rounders
{
    public class DurationRounder : IDurationRounder
    {
        public Task<decimal> GetRoundedDuration(decimal duration)
        {
            decimal result;

            if (duration < 0.25M)
            {
                result = decimal.Ceiling(duration * 10) / 10;
                return Task.FromResult(result);
            }

            var majorPart = Math.Truncate(duration);
            var minorPart = duration % 1;

            var diff = 0.5M - minorPart;
            var diffAbs = Math.Abs(diff);
            var quarter = 0.25M;

            result = 0 > diff
                ? diffAbs < quarter
                    ? decimal.Round(majorPart + minorPart - diffAbs, 2)
                    : decimal.Ceiling(duration)
                : diffAbs <= quarter
                    ? decimal.Round(majorPart + minorPart + diff, 2)
                    : majorPart;

            return Task.FromResult(result);
        }

        public Task<decimal> GetRoundedPreparatoryPeriod(decimal totalDuration)
        {
            var tenth = totalDuration / 10;

            if (totalDuration < 1)
            {
                return Task.FromResult(tenth);
            }

            var result = decimal.Floor(tenth * 10) / 10;

            return Task.FromResult(result);
        }
    }
}