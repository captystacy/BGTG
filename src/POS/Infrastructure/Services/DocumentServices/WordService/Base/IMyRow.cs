using POS.Infrastructure.Services.DocumentServices.WordService.Format;

namespace POS.Infrastructure.Services.DocumentServices.WordService.Base
{
    public interface IMyRow
    {
        IReadOnlyList<IMyCell> Cells { get; }
        IMyCell AddCell();
        int GetRowIndex();
        IMyCell AddCell(MyCellFormat? format);
    }
}