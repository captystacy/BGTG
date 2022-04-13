using System.Threading.Tasks;
using AutoFixture;
using Moq;
using POS.Infrastructure.Replacers;
using POS.Models;
using POS.Tests.Helpers.Services.DocumentServices.WordService;
using Xunit;

namespace POS.Tests.Infrastructure.Replacers
{
    public class EmployeesNeedReplacerTests
    {
        [Fact]
        public async Task ItShould_replace_employees_need_patterns()
        {
            // arrange

            var document = MyWordDocumentHelper.GetMock();
            var employeesNeed = new Fixture().Create<EmployeesNeed>();

            var sut = new EmployeesNeedReplacer();

            // act

            await sut.Replace(document.Object, employeesNeed);

            // assert

            document.Verify(x => x.Replace("%TNOE%", employeesNeed.TotalNumberOfEmployees.ToString()), Times.Once);
            document.Verify(x => x.Replace("%NOWE%", employeesNeed.NumberOfWorkingEmployees.ToString()), Times.Once);
            document.Verify(x => x.Replace("%NOM%", employeesNeed.NumberOfManagers.ToString()), Times.Once);
            document.Verify(x => x.Replace("%FR%", employeesNeed.ForemanRoom.ToString()), Times.Once);
            document.Verify(x => x.Replace("%DR%", employeesNeed.DressingRoom.ToString()), Times.Once);
            document.Verify(x => x.Replace("%WR%", employeesNeed.WashingRoom.ToString()), Times.Once);
            document.Verify(x => x.Replace("%WC%", employeesNeed.WashingCrane.ToString()), Times.Once);
            document.Verify(x => x.Replace("%SR%", employeesNeed.ShowerRoom.ToString()), Times.Once);
            document.Verify(x => x.Replace("%SM%", employeesNeed.ShowerMesh.ToString()), Times.Once);
            document.Verify(x => x.Replace("%T%", employeesNeed.Toilet.ToString()), Times.Once);
            document.Verify(x => x.Replace("%FP%", employeesNeed.FoodPoint.ToString()), Times.Once);
        }
    }
}
