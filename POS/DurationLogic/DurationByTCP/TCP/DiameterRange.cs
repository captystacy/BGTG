using System;

namespace POS.DurationLogic.DurationByTCP.TCP
{
    public class DiameterRange : IEquatable<DiameterRange>
    {
        public int Start { get; }
        public int End { get; }
        public string Presentation { get; }

        public DiameterRange(int start, int end, string presentation)
        {
            Start = start;
            End = end;
            Presentation = presentation;
        }

        public bool IsInRange(int value)
        {
            return Start <= value && value < End;
        }

        public bool Equals(DiameterRange other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Start == other.Start && End == other.End && Presentation == other.Presentation;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DiameterRange) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start, End, Presentation);
        }

        public static bool operator ==(DiameterRange left, DiameterRange right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DiameterRange left, DiameterRange right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"{nameof(Start)}: {Start}, {nameof(End)}: {End}, {nameof(Presentation)}: {Presentation}";
        }
    }
}
