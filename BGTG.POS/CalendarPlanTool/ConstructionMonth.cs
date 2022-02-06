using System;

namespace BGTG.POS.CalendarPlanTool
{
    public class ConstructionMonth : IEquatable<ConstructionMonth>
    {
        public DateTime Date { get; }
        public decimal InvestmentVolume { get; }
        public decimal VolumeCAIW { get; }
        public decimal PercentPart { get; set; }
        public int CreationIndex { get; }

        public ConstructionMonth(DateTime date, decimal investmentVolume, decimal volumeCAIW, decimal percentPart, int creationIndex)
        {
            Date = date;
            InvestmentVolume = investmentVolume;
            VolumeCAIW = volumeCAIW;
            PercentPart = percentPart;
            CreationIndex = creationIndex;
        }

        public bool Equals(ConstructionMonth other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Date.Equals(other.Date) && InvestmentVolume == other.InvestmentVolume &&
                   VolumeCAIW == other.VolumeCAIW && PercentPart == other.PercentPart &&
                   CreationIndex == other.CreationIndex;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConstructionMonth)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Date, InvestmentVolume, VolumeCAIW, PercentPart, CreationIndex);
        }

        public static bool operator ==(ConstructionMonth left, ConstructionMonth right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ConstructionMonth left, ConstructionMonth right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"{nameof(Date)}: {Date}, {nameof(InvestmentVolume)}: {InvestmentVolume}, {nameof(VolumeCAIW)}: {VolumeCAIW}, {nameof(PercentPart)}: {PercentPart}, {nameof(CreationIndex)}: {CreationIndex}";
        }
    }
}
