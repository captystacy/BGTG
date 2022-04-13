using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Format;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Interface;

namespace POS.Infrastructure.Services.DocumentServices.WordService
{
    public class MySection : MyBody, IMySection
    {
        private readonly ISection _section;

        protected override Body Body => _section.Body;

        public IReadOnlyList<IMyTable> Tables { get; private set; }

        public MySection(ISection section, IReadOnlyList<IMyTable> tables, IReadOnlyList<IMyParagraph> paragraphs) : base(paragraphs)
        {
            _section = section;
            Tables = tables;
        }

        public IMyTable AddTable()
        {
            return AddTable(null!);
        }

        public IMyTable AddTable(MyTableStyle? myTableStyle)
        {
            return AddTable(null!, null!, myTableStyle);
        }

        public IMyTable AddTable(int? columnsNumber, int? rowsNumber, MyTableStyle? myTableStyle)
        {
            return AddTable(columnsNumber, rowsNumber, null!, null!, null!, null!, myTableStyle);
        }

        public IMyTable AddTable(int? columnsNumber, int? rowsNumber, MyTableFormat? tableFormat, MyCellFormat? cellFormat, MyParagraphFormat? paragraphFormat, MyCharacterFormat? characterFormat, MyTableStyle? myTableStyle)
        {
            var table = _section.AddTable();

            if (columnsNumber is not null)
            {
                table.DefaultColumnsNumber = columnsNumber.Value;
            }

            if (myTableStyle is not null)
            {
                var tableStyle = myTableStyle switch
                {
                    MyTableStyle.TableCleared => DefaultTableStyle.TableNormal,
                    MyTableStyle.TableGrid => DefaultTableStyle.TableGrid,
                    _ => throw new ArgumentOutOfRangeException(nameof(myTableStyle), myTableStyle, null)
                };
                table.ApplyStyle(tableStyle);
            }

            var myTable = new MyTable(table, new List<IMyRow>(), cellFormat, paragraphFormat, characterFormat);

            if (tableFormat is not null)
            {
                myTable.ApplyFormat(tableFormat);
            }

            if (rowsNumber is not null)
            {
                for (int i = 0; i < rowsNumber; i++)
                {
                    if (cellFormat is not null)
                    {
                        myTable.AddRow(cellFormat);
                    }
                    else
                    {
                        myTable.AddRow();
                    }
                }
            }

            var tables = Tables.ToList();
            tables.Add(myTable);
            Tables = tables;
            return myTable;
        }
    }
}