using POS.Infrastructure.Services.DocumentServices.WordService.Format;

namespace POS.Infrastructure.Services.DocumentServices.WordService.Base
{
    public interface IMyTable
    {
        IReadOnlyList<IMyRow> Rows { get; }
        IMyRow AddRow();
        void ApplyVerticalMerge(int columnIndex, int startRowIndex, int endRowIndex);
        void ApplyHorizontalMerge(int rowIndex, int startCellIndex, int endCellIndex);
        IMyRow AddRow(MyCellFormat format);
        void AutoFit(MyAutoFitBehaviorType myAutoFitBehaviorType);
        void ApplyFormat(MyTableFormat format);
    }
}