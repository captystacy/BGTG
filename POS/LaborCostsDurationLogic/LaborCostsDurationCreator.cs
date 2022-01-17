using System;
using POS.LaborCostsDurationLogic.Interfaces;

namespace POS.LaborCostsDurationLogic
{
    public class LaborCostsDurationCreator : ILaborCostsDurationCreator
    {
        private decimal _halfMonthAcceptanceTime = 0.5M;
        private int _oneMonthAcceptanceTime = 1;

        public LaborCostsDuration Create(decimal laborCosts, decimal workingDayDuration, decimal shift, decimal numberOfWorkingDaysInMonth, int numberOfEmployees, bool acceptanceTimeIncluded, decimal technologicalLaborCosts)
        {
            var duration = GetDuration(laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees);

            if (duration < 1)
            {
                if (duration < 0.25M)
                {
                    return GetLessThanQuarterMonthLaborCostsDuration(duration, laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, acceptanceTimeIncluded, technologicalLaborCosts);
                }

                return GetMoreThanQuarterButLessThanOneMonthLaborCostsDuration(duration, laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, acceptanceTimeIncluded, technologicalLaborCosts);
            }

            return GetMoreThanOneMonthLaborCostsDuration(duration, laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, acceptanceTimeIncluded, technologicalLaborCosts);
        }

        public decimal GetDuration(decimal laborCosts, decimal workingDayDuration, decimal shift, decimal numberOfWorkingDaysInMonth, int numberOfEmployees)
        {
            return laborCosts / workingDayDuration / shift / numberOfWorkingDaysInMonth / numberOfEmployees;
        }

        private LaborCostsDuration GetLessThanQuarterMonthLaborCostsDuration(decimal duration, decimal laborCosts,
            decimal workingDayDuration, decimal shift, decimal numberOfWorkingDaysInMonth, int numberOfEmployees,
            bool acceptanceTimeIncluded, decimal technologicalLaborCosts)
        {
            var roundedDuration = Ceiling(duration, 1);
            var totalDuration = acceptanceTimeIncluded
                ? roundedDuration + _halfMonthAcceptanceTime
                : roundedDuration;
            var preparatoryPeriod = totalDuration / 10;
            return new LaborCostsDuration(decimal.Round(duration, 2), laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth,
                numberOfEmployees, totalDuration, preparatoryPeriod, roundedDuration, acceptanceTimeIncluded ? _halfMonthAcceptanceTime : 0, acceptanceTimeIncluded, false, technologicalLaborCosts);

        }

        private LaborCostsDuration GetMoreThanQuarterButLessThanOneMonthLaborCostsDuration(decimal duration,
            decimal laborCosts, decimal workingDayDuration, decimal shift, decimal numberOfWorkingDaysInMonth,
            int numberOfEmployees, bool acceptanceTimeIncluded, decimal technologicalLaborCosts)
        {
            var roundedDuration = GetRoundedDuration(duration);
            var totalDuration = acceptanceTimeIncluded
                ? roundedDuration + _halfMonthAcceptanceTime
                : roundedDuration;
            var preparatoryPeriod = totalDuration / 10;
            return new LaborCostsDuration(decimal.Round(duration, 2), laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth,
                numberOfEmployees, Normalize(totalDuration), Normalize(preparatoryPeriod), Normalize(roundedDuration), 
                acceptanceTimeIncluded ? _halfMonthAcceptanceTime : 0, acceptanceTimeIncluded, true, technologicalLaborCosts);
        }

        private LaborCostsDuration GetMoreThanOneMonthLaborCostsDuration(decimal duration, decimal laborCosts,
            decimal workingDayDuration, decimal shift, decimal numberOfWorkingDaysInMonth, int numberOfEmployees,
            bool acceptanceTimeIncluded, decimal technologicalLaborCosts)
        {
            var roundedDuration = GetRoundedDuration(duration);
            var totalDuration = acceptanceTimeIncluded
                ? roundedDuration + _oneMonthAcceptanceTime
                : roundedDuration;
            var preparatoryPeriod = Floor(totalDuration / 10, 1);
            return new LaborCostsDuration(decimal.Round(duration, 2), laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth,
                numberOfEmployees, Normalize(totalDuration), preparatoryPeriod, Normalize(roundedDuration), 
                acceptanceTimeIncluded ? _oneMonthAcceptanceTime : 0, acceptanceTimeIncluded, true, technologicalLaborCosts);
        }

        private decimal GetRoundedDuration(decimal duration)
        {
            var majorPart = Math.Truncate(duration);
            var minorPart = duration % 1;

            var diff = 0.5M - minorPart;
            var diffAbs = Math.Abs(diff);
            var quarter = 0.25M;

            return 0 > diff
                ? diffAbs < quarter
                    ? decimal.Round(majorPart + minorPart - diffAbs, 2)
                    : decimal.Ceiling(duration)
                : diffAbs < quarter
                    ? decimal.Round(majorPart + minorPart + diff, 2)
                    : majorPart;
        }

        private decimal Normalize(decimal value)
        {
            return value / 1.000000000000000000000000000000000m;
        }

        private decimal Ceiling(decimal input, int places)
        {
            var multiplier = (decimal)Math.Pow(10, places);
            return decimal.Ceiling(input * multiplier) / multiplier;
        }

        private decimal Floor(decimal input, int places)
        {
            var multiplier = (decimal)Math.Pow(10, places);
            return decimal.Floor(input * multiplier) / multiplier;
        }
    }
}
