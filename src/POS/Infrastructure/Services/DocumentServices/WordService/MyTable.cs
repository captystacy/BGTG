using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Format;
using Spire.Doc;
using Spire.Doc.Documents;

namespace POS.Infrastructure.Services.DocumentServices.WordService
{
    public class MyTable : IMyTable
    {
        private readonly Table _table;
        private readonly MyCellFormat? _cellFormat;
        private readonly MyParagraphFormat? _paragraphFormat;
        private readonly MyCharacterFormat? _characterFormat;

        public IReadOnlyList<IMyRow> Rows { get; private set; }

        public MyTable(Table table, IReadOnlyList<IMyRow> rows)
        {
            _table = table;
            Rows = rows;
        }

        public MyTable(Table table, IReadOnlyList<IMyRow> rows, MyCellFormat? cellFormat, MyParagraphFormat? paragraphFormat, MyCharacterFormat? characterFormat)
        {
            _table = table;
            _cellFormat = cellFormat;
            _paragraphFormat = paragraphFormat;
            _characterFormat = characterFormat;
            Rows = rows;
        }

        public IMyRow AddRow()
        {
            return AddRow(_cellFormat);
        }

        public IMyRow AddRow(MyCellFormat? format)
        {
            var row = _table.AddRow();

            var cells = new List<IMyCell>();
            foreach (TableCell cell in row.Cells)
            {
                var myCell = new MyCell(cell, new List<IMyParagraph>(), _paragraphFormat, _characterFormat);

                if (format is not null)
                {
                    myCell.ApplyFormat(format);
                }

                cells.Add(myCell);
            }

            var myRow = new MyRow(row, cells);
            return AddRowToMyList(myRow);
        }

        private IMyRow AddRowToMyList(IMyRow myRow)
        {
            var rows = Rows.ToList();
            rows.Add(myRow);

            Rows = rows;

            return myRow;
        }

        public void ApplyFormat(MyTableFormat format)
        {
            if (format.HorizontalAlignment is not null)
            {
                _table.TableFormat.HorizontalAlignment = format.HorizontalAlignment switch
                {
                    MyTableAlignment.Left => RowAlignment.Left,
                    MyTableAlignment.Center => RowAlignment.Center,
                    MyTableAlignment.Right => RowAlignment.Right,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        public void ApplyVerticalMerge(int columnIndex, int startRowIndex, int endRowIndex)
        {
            _table.ApplyVerticalMerge(columnIndex, startRowIndex, endRowIndex);
        }

        public void ApplyHorizontalMerge(int rowIndex, int startCellIndex, int endCellIndex)
        {
            _table.ApplyHorizontalMerge(rowIndex, startCellIndex, endCellIndex);
        }

        public void AutoFit(MyAutoFitBehaviorType myAutoFitBehaviorType)
        {
            var autoFitBehaviorType = myAutoFitBehaviorType switch
            {
                MyAutoFitBehaviorType.FixedColumnWidths => AutoFitBehaviorType.FixedColumnWidths,
                MyAutoFitBehaviorType.AutoFitToContents => AutoFitBehaviorType.AutoFitToContents,
                MyAutoFitBehaviorType.AutoFitToWindow => AutoFitBehaviorType.AutoFitToWindow,
                _ => throw new ArgumentOutOfRangeException(nameof(myAutoFitBehaviorType), myAutoFitBehaviorType, null)
            };

            _table.AutoFit(autoFitBehaviorType);
        }
    }
}