﻿using BGTG.POS.DurationTools.Base;
using BGTG.POS.DurationTools.DurationByLCTool.Base;

namespace BGTG.POS.DurationTools.DurationByLCTool
{
    public class DurationByLCCreator : IDurationByLCCreator
    {
        private readonly IDurationRounder _durationRounder;

        private const decimal HalfMonthAcceptanceTime = 0.5M;
        private const int OneMonthAcceptanceTime = 1;

        public DurationByLCCreator(IDurationRounder durationRounder)
        {
            _durationRounder = durationRounder;
        }

        public DurationByLC Create(decimal estimateLaborCosts, decimal technologicalLaborCosts, decimal workingDayDuration, decimal shift, decimal numberOfWorkingDaysInMonth, int numberOfEmployees, bool acceptanceTimeIncluded)
        {
            var totalLaborCosts = estimateLaborCosts + technologicalLaborCosts;
            var duration = totalLaborCosts / workingDayDuration / shift / numberOfWorkingDaysInMonth / numberOfEmployees;

            decimal roundedDuration;
            decimal totalDuration;
            decimal acceptanceTime;
            bool roundingIncluded;

            if (duration < 1)
            {
                acceptanceTime = acceptanceTimeIncluded ? HalfMonthAcceptanceTime : 0;
                if (duration < 0.25M)
                {
                    roundingIncluded = false;
                    roundedDuration = _durationRounder.GetRoundedDuration(duration);
                    totalDuration = acceptanceTimeIncluded
                        ? roundedDuration + HalfMonthAcceptanceTime
                        : roundedDuration;
                }
                else
                {
                    roundingIncluded = true;
                    roundedDuration = _durationRounder.GetRoundedDuration(duration);
                    totalDuration = acceptanceTimeIncluded
                        ? roundedDuration + HalfMonthAcceptanceTime
                        : roundedDuration;
                }
            }
            else
            {
                acceptanceTime = acceptanceTimeIncluded ? OneMonthAcceptanceTime : 0;
                roundingIncluded = true;
                roundedDuration = _durationRounder.GetRoundedDuration(duration);
                totalDuration = acceptanceTimeIncluded
                    ? roundedDuration + OneMonthAcceptanceTime
                    : roundedDuration;
            }
            var preparatoryPeriod = _durationRounder.GetRoundedPreparatoryPeriod(totalDuration);

            return new DurationByLC(decimal.Round(duration, 2), totalLaborCosts, estimateLaborCosts,
                technologicalLaborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth,
                numberOfEmployees, totalDuration, preparatoryPeriod, roundedDuration, acceptanceTime,
                acceptanceTimeIncluded, roundingIncluded);
        }
    }
}