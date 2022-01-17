using System;

namespace POS.CalendarPlanLogic
{
    public class ConstructionMonth : IEquatable<ConstructionMonth>
    {
        public DateTime Date { get; }
        public decimal InvestmentVolume { get; }
        public decimal VolumeCAIW { get; }
        public decimal PercentPart { get; set; }
        public int CreationIndex { get; }

        public ConstructionMonth(DateTime date, decimal investmentVolume, decimal volumeCAIW, decimal percentePart, int creationIndex)
        {
            Date = date;
            InvestmentVolume = investmentVolume;
            VolumeCAIW = volumeCAIW;
            PercentPart = percentePart;
            CreationIndex = creationIndex;
        }

        public bool Equals(ConstructionMonth other)
        {
            if (other == null)
            {
                return false;
            }

            return Date == other.Date
                && InvestmentVolume == other.InvestmentVolume
                && VolumeCAIW == other.VolumeCAIW
                && PercentPart == other.PercentPart
                && CreationIndex == other.CreationIndex;
        }

        public override bool Equals(object obj) => Equals(obj as ConstructionMonth);

        public override int GetHashCode() => HashCode.Combine(Date, InvestmentVolume, VolumeCAIW, PercentPart, CreationIndex);

        public static bool operator ==(ConstructionMonth constructionMonth1, ConstructionMonth constructionMonth2)
        {
            if (constructionMonth1 is null)
            {
                return constructionMonth2 is null;
            }

            return constructionMonth1.Equals(constructionMonth2);
        }

        public static bool operator !=(ConstructionMonth constructionMonth1, ConstructionMonth constructionMonth2) => !(constructionMonth1 == constructionMonth2);

        public override string ToString()
        {
            return string.Join(", ", Date, InvestmentVolume, VolumeCAIW, PercentPart, CreationIndex);
        }
    }
}
