namespace POS.DomainModels;

public class EmployeesNeed
{
    public int TotalNumberOfEmployees { get; }
    public int NumberOfWorkingEmployees { get; }
    public int NumberOfManagers { get; }
    public decimal ForemanRoom { get; }
    public decimal DressingRoom { get; }
    public decimal WashingRoom { get; }
    public int WashingCrane { get; }
    public decimal ShowerRoom { get; }
    public int ShowerMesh { get; }
    public int Toilet { get; }
    public decimal FoodPoint { get; }

    public EmployeesNeed(int totalNumberOfEmployees, int numberOfWorkingEmployees, int numberOfManagers, decimal foremanRoom, decimal dressingRoom, decimal washingRoom, int washingCrane, decimal showerRoom, int showerMesh, int toilet, decimal foodPoint)
    {
        TotalNumberOfEmployees = totalNumberOfEmployees;
        NumberOfWorkingEmployees = numberOfWorkingEmployees;
        NumberOfManagers = numberOfManagers;
        ForemanRoom = foremanRoom;
        DressingRoom = dressingRoom;
        WashingRoom = washingRoom;
        WashingCrane = washingCrane;
        ShowerRoom = showerRoom;
        ShowerMesh = showerMesh;
        Toilet = toilet;
        FoodPoint = foodPoint;
    }

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