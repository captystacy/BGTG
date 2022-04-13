using POS.Infrastructure.Services.DocumentServices.ExcelService.Base;

namespace POS.Infrastructure.Services.DocumentServices.ExcelService
{
    public class MyWorkBook : IMyWorkBook
    {
        public IReadOnlyList<IMyWorkSheet> WorkSheets { get; }

        public MyWorkBook(IReadOnlyList<IMyWorkSheet> workSheets)
        {
            WorkSheets = workSheets;
        }
    }
}