namespace POS.Models
{
    public class EmployeesNeed
    {
        public int TotalNumberOfEmployees { get; set; }
        public int NumberOfWorkingEmployees { get; set; }
        public int NumberOfManagers { get; set; }
        public decimal ForemanRoom { get; set; }
        public decimal DressingRoom { get; set; }
        public decimal WashingRoom { get; set; }
        public int WashingCrane { get; set; }
        public decimal ShowerRoom { get; set; }
        public int ShowerMesh { get; set; }
        public int Toilet { get; set; }
        public decimal FoodPoint { get; set; }

        protected bool Equals(EmployeesNeed other)
        {
            return TotalNumberOfEmployees == other.TotalNumberOfEmployees &&
                   NumberOfWorkingEmployees == other.NumberOfWorkingEmployees &&
                   NumberOfManagers == other.NumberOfManagers && ForemanRoom == other.ForemanRoom &&
                   DressingRoom == other.DressingRoom && WashingRoom == other.WashingRoom &&
                   WashingCrane == other.WashingCrane && ShowerRoom == other.ShowerRoom && ShowerMesh == other.ShowerMesh &&
                   Toilet == other.Toilet && FoodPoint == other.FoodPoint;

        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((EmployeesNeed)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(TotalNumberOfEmployees);
            hashCode.Add(NumberOfWorkingEmployees);
            hashCode.Add(NumberOfManagers);
            hashCode.Add(ForemanRoom);
            hashCode.Add(DressingRoom);
            hashCode.Add(WashingRoom);
            hashCode.Add(WashingCrane);
            hashCode.Add(ShowerRoom);
            hashCode.Add(ShowerMesh);
            hashCode.Add(Toilet);
            hashCode.Add(FoodPoint);
            return hashCode.ToHashCode();
        }

        public static bool operator ==(EmployeesNeed? left, EmployeesNeed? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EmployeesNeed? left, EmployeesNeed? right)
        {
            return !Equals(left, right);
        }
    }
}