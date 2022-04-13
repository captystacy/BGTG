using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Format;
using Spire.Doc;

namespace POS.Infrastructure.Services.DocumentServices.WordService
{
    public class MyRow : IMyRow
    {
        private readonly TableRow _row;

        public IReadOnlyList<IMyCell> Cells { get; private set; }

        public MyRow(TableRow row, IReadOnlyList<IMyCell> cells)
        {
            _row = row;
            Cells = cells;
        }

        public IMyCell AddCell()
        {
            return AddCell(null!);
        }

        public IMyCell AddCell(MyCellFormat? format)
        {
            var cell = _row.AddCell();

            var myCell = new MyCell(cell, new List<IMyParagraph>());
            if (format is not null)
            {
                myCell.ApplyFormat(format);
            }

            var cells = Cells.ToList();
            cells.Add(myCell);

            Cells = cells;
            return myCell;
        }

        public int GetRowIndex()
        {
            return _row.GetRowIndex();
        }
    }
}