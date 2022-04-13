using POS.Infrastructure.Factories.Base;
using POS.Infrastructure.Services.DocumentServices.WordService;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using Spire.Doc;
using Spire.Doc.Documents;

namespace POS.Infrastructure.Factories
{
    public class MyWordDocumentFactory : IMyWordDocumentFactory
    {
        public Task<IMyWordDocument> CreateAsync(string path)
        {
            var document = new Document(path);

            var myDocument = Convert(document);

            return Task.FromResult<IMyWordDocument>(myDocument);
        }

        public Task<IMyWordDocument> CreateAsync(Stream stream)
        {
            var document = new Document(stream);

            var myDocument = Convert(document);

            return Task.FromResult<IMyWordDocument>(myDocument);
        }

        private MyWordDocument Convert(Document document)
        {
            var mySections = new List<IMySection>();
            foreach (Section section in document.Sections)
            {
                var myTables = new List<IMyTable>();
                foreach (Table table in section.Tables)
                {
                    var myRows = new List<IMyRow>();
                    foreach (TableRow tableRow in table.Rows)
                    {
                        var myCells = new List<IMyCell>();
                        foreach (TableCell cell in tableRow.Cells)
                        {
                            var myParagraphsInCell = new List<IMyParagraph>();
                            foreach (Paragraph paragraph in cell.Paragraphs)
                            {
                                myParagraphsInCell.Add(new MyParagraph(paragraph));
                            }
                            myCells.Add(new MyCell(cell, myParagraphsInCell));
                        }
                        myRows.Add(new MyRow(tableRow, myCells));
                    }
                    myTables.Add(new MyTable(table, myRows));
                }
                var myParagraphsInDocument = new List<IMyParagraph>();
                foreach (Paragraph paragraph in section.Paragraphs)
                {
                    myParagraphsInDocument.Add(new MyParagraph(paragraph));
                }
                mySections.Add(new MySection(section, myTables, myParagraphsInDocument));
            }

            var myDocument = new MyWordDocument(document, mySections);

            return myDocument;
        }

        public Task<IMyWordDocument> CreateAsync()
        {
            var document = new Document();
            document.AddStyle(BuiltinStyle.Normal);
            var style = document.Styles.FindByName("Normal");
            style.CharacterFormat.LocaleIdASCII = 1049;
            var myDocument = new MyWordDocument(document, new List<IMySection>());
            return Task.FromResult<IMyWordDocument>(myDocument);
        }
    }
}