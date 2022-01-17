using System;

namespace POSCore.LaborCostsDurationLogic
{
    public class LaborCostsDuration : IEquatable<LaborCostsDuration>
    {
        public decimal Duration { get; set; }
        public decimal LaborCosts { get; }
        public decimal WorkingDayDuration { get; }
        public decimal Shift { get; }
        public decimal NumberOfWorkingDays { get; }
        public int NumberOfEmployees { get; }
        public decimal RoundedDuration { get; }
        public decimal TotalDuration { get; }
        public decimal PreparatoryPeriod { get; }
        public decimal AcceptanceTime { get; }
        public bool AcceptanceTimeIncluded { get; }
        public bool RoundingIncluded { get; }

        public LaborCostsDuration(decimal duration, decimal laborCosts, decimal workingDayDuration, decimal shift,
            decimal numberOfWorkingDaysInMonth, int numberOfEmployees, decimal totalDuration, decimal preparatoryPeriod,
            decimal roundedDuration, decimal acceptanceTime, bool acceptanceTimeIncluded, bool roundingIncluded)
        {
            Duration = duration;
            LaborCosts = laborCosts;
            WorkingDayDuration = workingDayDuration;
            Shift = shift;
            NumberOfWorkingDays = numberOfWorkingDaysInMonth;
            NumberOfEmployees = numberOfEmployees;
            RoundedDuration = roundedDuration;
            TotalDuration = totalDuration;
            PreparatoryPeriod = preparatoryPeriod;
            AcceptanceTime = acceptanceTime;
            AcceptanceTimeIncluded = acceptanceTimeIncluded;
            RoundingIncluded = roundingIncluded;
        }

        public bool Equals(LaborCostsDuration other)
        {
            if (other == null)
            {
                return false;
            }

            return Duration == other.Duration
                && LaborCosts == other.LaborCosts
                && WorkingDayDuration == other.WorkingDayDuration
                && Shift == other.Shift
                && NumberOfWorkingDays == other.NumberOfWorkingDays
                && NumberOfEmployees == other.NumberOfEmployees
                && RoundedDuration == other.RoundedDuration
                && TotalDuration == other.TotalDuration
                && PreparatoryPeriod == other.PreparatoryPeriod
                && AcceptanceTime == other.AcceptanceTime
                && AcceptanceTimeIncluded == other.AcceptanceTimeIncluded
                && RoundingIncluded == other.RoundingIncluded;
        }

        public override bool Equals(object obj) => Equals(obj as LaborCostsDuration);

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(Duration);
            hashcode.Add(LaborCosts);
            hashcode.Add(WorkingDayDuration);
            hashcode.Add(Shift);
            hashcode.Add(NumberOfWorkingDays);
            hashcode.Add(NumberOfEmployees);
            hashcode.Add(RoundedDuration);
            hashcode.Add(TotalDuration);
            hashcode.Add(PreparatoryPeriod);
            hashcode.Add(AcceptanceTime);
            hashcode.Add(AcceptanceTimeIncluded);
            hashcode.Add(RoundingIncluded);
            return hashcode.ToHashCode();
        }

        public static bool operator ==(LaborCostsDuration laborCostsDuration1, LaborCostsDuration laborCostsDuration2)
        {
            if (laborCostsDuration1 is null)
            {
                return laborCostsDuration2 is null;
            }

            return laborCostsDuration1.Equals(laborCostsDuration2);
        }

        public static bool operator !=(LaborCostsDuration laborCostsDuration1, LaborCostsDuration laborCostsDuration2) => !(laborCostsDuration1 == laborCostsDuration2);

        public override string ToString()
        {
            return string.Join(", ", Duration, LaborCosts, WorkingDayDuration, Shift, NumberOfEmployees,
                RoundedDuration, TotalDuration, PreparatoryPeriod, AcceptanceTime, AcceptanceTimeIncluded,
                RoundingIncluded);
        }
    }
}
