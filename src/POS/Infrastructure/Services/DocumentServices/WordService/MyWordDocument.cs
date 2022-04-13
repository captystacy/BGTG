using System.Reflection;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;

namespace POS.Infrastructure.Services.DocumentServices.WordService
{
    public class MyWordDocument : IMyWordDocument
    {
        private readonly Document _document;

        public IReadOnlyList<IMySection> Sections { get; private set; }

        public MyWordDocument(Document document, IReadOnlyList<IMySection> mySections)
        {
            _document = document;
            Sections = mySections;
        }

        public IMySection AddSection()
        {
            var section = _document.AddSection();
            var mySection = new MySection(section, new List<IMyTable>(), new List<IMyParagraph>());
            var sections = Sections.ToList();
            sections.Add(mySection);
            Sections = sections;
            return mySection;
        }

        public int Replace(string matchString, string newValue)
        {
            return _document.Replace(matchString, newValue, true, true);
        }

        public void ReplaceTextWithImage(string searchValue, string imagePath)
        {
            var selections = _document.FindAllString(searchValue, true, true);

            foreach (var selection in selections)
            {
                var pic = new DocPicture(_document);
                pic.LoadImage(imagePath);
                pic.TextWrappingStyle = TextWrappingStyle.Inline;
                pic.Width = 40;
                pic.Height = 15;
                var range = selection.GetAsOneRange();
                var index = range.OwnerParagraph.ChildObjects.IndexOf(range);
                range.OwnerParagraph.ChildObjects.Insert(index, pic);
                range.OwnerParagraph.ChildObjects.Remove(range);
            }
        }

        public void ReplaceTextWithTable(string searchValue, IMyTable myTable)
        {
            var textSelection = _document.FindString(searchValue, true, true);
            var textRange = textSelection.GetAsOneRange();
            var paragraph = textRange.OwnerParagraph;
            var body = paragraph.OwnerTextBody;
            var index = body.ChildObjects.IndexOf(paragraph);
            var table = (Table)typeof(MyTable).GetField("_table", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(myTable)!;
            body.ChildObjects.Remove(paragraph);
            body.ChildObjects.Insert(index, table.Clone());
        }

        public void Save(string fileName)
        {
            _document.SaveToFile(fileName);
        }

        public void SaveAs(Stream stream, MyFileFormat fileFormat)
        {
            var myFileFormat = fileFormat switch
            {
                MyFileFormat.DocX => FileFormat.Docx,
                MyFileFormat.Doc => FileFormat.Doc,
                _ => throw new ArgumentOutOfRangeException(nameof(fileFormat), fileFormat, null)
            };

            _document.SaveToStream(stream, myFileFormat);
        }

        public void Dispose()
        {
            _document.Dispose();
        }
    }
}