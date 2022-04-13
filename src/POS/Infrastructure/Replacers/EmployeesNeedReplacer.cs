using POS.Infrastructure.Replacers.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Models;

namespace POS.Infrastructure.Replacers
{
    public class EmployeesNeedReplacer : IEmployeesNeedReplacer
    {

        private const string TotalNumberOfEmployeesPattern = "%TNOE%";
        private const string NumberOfWorkingEmployeesPattern = "%NOWE%";
        private const string NumberOfManagersPattern = "%NOM%";
        private const string ForemanRoomPattern = "%FR%";
        private const string DressingRoomPattern = "%DR%";
        private const string WashingRoomPattern = "%WR%";
        private const string WashingCranePattern = "%WC%";
        private const string ShowerRoomPattern = "%SR%";
        private const string ShowerMeshPattern = "%SM%";
        private const string ToiletPattern = "%T%";
        private const string FoodPointPattern = "%FP%";

        public Task Replace(IMyWordDocument document, EmployeesNeed employeesNeed)
        {
            document.Replace(TotalNumberOfEmployeesPattern, employeesNeed.TotalNumberOfEmployees.ToString());
            document.Replace(NumberOfWorkingEmployeesPattern, employeesNeed.NumberOfWorkingEmployees.ToString());
            document.Replace(NumberOfManagersPattern, employeesNeed.NumberOfManagers.ToString());
            document.Replace(ForemanRoomPattern, employeesNeed.ForemanRoom.ToString());
            document.Replace(DressingRoomPattern, employeesNeed.DressingRoom.ToString());
            document.Replace(WashingRoomPattern, employeesNeed.WashingRoom.ToString());
            document.Replace(WashingCranePattern, employeesNeed.WashingCrane.ToString());
            document.Replace(ShowerRoomPattern, employeesNeed.ShowerRoom.ToString());
            document.Replace(ShowerMeshPattern, employeesNeed.ShowerMesh.ToString());
            document.Replace(ToiletPattern, employeesNeed.Toilet.ToString());
            document.Replace(FoodPointPattern, employeesNeed.FoodPoint.ToString());

            return Task.CompletedTask;
        }
    }
}
