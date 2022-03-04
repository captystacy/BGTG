using POS.Infrastructure.Tools.DurationTools.Base;

namespace POS.Infrastructure.Tools.DurationTools;

public class DurationRounder : IDurationRounder
{
    public decimal GetRoundedDuration(decimal duration)
    {
        if (duration < 0.25M)
        {
            return decimal.Ceiling(duration * 10) / 10;
        }

        var majorPart = Math.Truncate(duration);
        var minorPart = duration % 1;

        var diff = 0.5M - minorPart;
        var diffAbs = Math.Abs(diff);
        var quarter = 0.25M;

        return 0 > diff
            ? diffAbs < quarter
                ? decimal.Round(majorPart + minorPart - diffAbs, 2)
                : decimal.Ceiling(duration)
            : diffAbs <= quarter
                ? decimal.Round(majorPart + minorPart + diff, 2)
                : majorPart;
    }

    public decimal GetRoundedPreparatoryPeriod(decimal totalDuration)
    {
        var tenth = totalDuration / 10;

        if (totalDuration < 1)
        {
            return tenth;
        }

        return decimal.Floor(tenth * 10) / 10;
    }
}