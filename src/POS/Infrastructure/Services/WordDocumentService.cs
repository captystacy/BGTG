using POS.DomainModels;
using POS.Infrastructure.Services.Base;
using Spire.Doc;
using Spire.Doc.Documents;

namespace POS.Infrastructure.Services;

public class WordDocumentService : IWordDocumentService
{
    public int BaseDocumentIndex { get; set; }
    public bool ReplaceInBaseDocumentMode { get; set; }
    public int DocumentIndex { get; set; }
    public int SectionIndex { get; set; }
    public int TableIndex { get; set; }
    public int RowIndex { get; set; }
    public int CellIndex { get; set; }
    public int ParagraphIndex { get; set; }
    public int RowCount => _documents[DocumentIndex].Sections[SectionIndex].Tables[TableIndex].Rows.Count;
    public int CellsCountInRow => _documents[DocumentIndex].Sections[SectionIndex].Tables[TableIndex].Rows[RowIndex].Cells.Count;
    public int ParagraphsCountInCell => _documents[DocumentIndex].Sections[SectionIndex].Tables[TableIndex].Rows[RowIndex].Cells[CellIndex].Paragraphs.Count;
    public int ParagraphsCountInDocument => _documents[DocumentIndex].Sections[SectionIndex].Paragraphs.Count;
    public string ParagraphTextInDocument => _documents[DocumentIndex].Sections[SectionIndex].Paragraphs[ParagraphIndex].Text;

    public string ParagraphTextInCell
    {
        get =>
            _documents[DocumentIndex]
                .Sections[SectionIndex]
                .Tables[TableIndex]
                .Rows[RowIndex]
                .Cells[CellIndex]
                .Paragraphs[ParagraphIndex]
                .Text;
        set =>
            _documents[DocumentIndex]
                .Sections[SectionIndex]
                .Tables[TableIndex]
                .Rows[RowIndex]
                .Cells[CellIndex]
                .Paragraphs[ParagraphIndex]
                .Text = value;
    }

    private readonly List<Document> _documents = new();

    public void Load(string path)
    {
        var document = new Document(path);
        SetNewDocument(document);
    }

    public void Load(Stream stream)
    {
        var document = new Document(stream);
        SetNewDocument(document);
    }

    private void SetNewDocument(Document document)
    {
        _documents.Add(document);
        DocumentIndex = _documents.Count - 1;
        SectionIndex = 0;
        TableIndex = 0;
        RowIndex = 0;
        CellIndex = 0;
        ParagraphIndex = 0;
    }

    public void ReplaceTextInCell(string searchValue, string newValue)
    {
        _documents[ReplaceInBaseDocumentMode ? BaseDocumentIndex : DocumentIndex]
            .Sections[SectionIndex]
            .Tables[TableIndex]
            .Rows[RowIndex]
            .Cells[CellIndex]
            .Paragraphs[ParagraphIndex]
            .Replace(searchValue, newValue, true, true);
    }

    public void ReplaceTextInDocument(string searchValue, string newValue)
    {
        _documents[ReplaceInBaseDocumentMode ? BaseDocumentIndex : DocumentIndex]
            .Replace(searchValue, newValue, true, true);
    }

    public void ReplaceTextWithTable(string searchValue)
    {
        var textSelection = _documents[ReplaceInBaseDocumentMode ? BaseDocumentIndex : DocumentIndex].FindString(searchValue, true, true);
        var textRange = textSelection.GetAsOneRange();
        var paragraph = textRange.OwnerParagraph;
        var body = paragraph.OwnerTextBody;
        var index = body.ChildObjects.IndexOf(paragraph);
        var table = _documents[DocumentIndex]
            .Sections[SectionIndex]
            .Tables[TableIndex].Clone();
        body.ChildObjects.Remove(paragraph);
        body.ChildObjects.Insert(index, table);
    }

    public void InsertTemplateRow(int templateRowIndex, int insertionIndex)
    {
        var templateRow = _documents[DocumentIndex]
            .Sections[SectionIndex]
            .Tables[TableIndex]
            .Rows[templateRowIndex]
            .Clone();

        _documents[DocumentIndex]
            .Sections[SectionIndex]
            .Tables[TableIndex]
            .Rows
            .Insert(insertionIndex, templateRow);
    }

    public void RemoveRow()
    {
        _documents[DocumentIndex]
            .Sections[SectionIndex]
            .Tables[TableIndex]
            .Rows
            .RemoveAt(RowIndex);
    }

    public void RemoveParagraphInCell()
    {
        _documents[DocumentIndex]
            .Sections[SectionIndex]
            .Tables[TableIndex]
            .Rows[RowIndex]
            .Cells[CellIndex]
            .Paragraphs
            .RemoveAt(ParagraphIndex);
    }

    public void AddParagraph(string text, int fontSize = 12)
    {
        var paragraph = new Paragraph(_documents[DocumentIndex]);
        paragraph.AppendText(text);

        var style = (ParagraphStyle)_documents[DocumentIndex].Styles.FindByName("Normal", StyleType.ParagraphStyle);
        style.CharacterFormat.FontSize = fontSize;

        paragraph.ApplyStyle(style);

        _documents[DocumentIndex]
            .Sections[SectionIndex]
            .Tables[TableIndex]
            .Rows[RowIndex]
            .Cells[CellIndex]
            .Paragraphs
            .Add(paragraph);
    }

    public void ApplyVerticalMerge(int columnIndex, int startRowIndex, int endRowIndex)
    {
        _documents[DocumentIndex]
            .Sections[SectionIndex]
            .Tables[TableIndex]
            .ApplyVerticalMerge(columnIndex, startRowIndex, endRowIndex);
    }


    public void MergeDocuments(int toDocumentIndex, int fromDocumentIndex)
    {
        foreach (Section section in _documents[fromDocumentIndex].Sections)
        {
            _documents[toDocumentIndex].Sections.Add(section.Clone());
        }
    }

    public void SaveAs(Stream stream, MyFileFormat myFileFormat, int documentIndex = 0)
    {
        var fileFormat = myFileFormat switch
        {
            MyFileFormat.DocX => FileFormat.Docx,
            MyFileFormat.Doc => FileFormat.Doc,
            _ => throw new ArgumentOutOfRangeException(nameof(myFileFormat), myFileFormat, null)
        };

        _documents[documentIndex].SaveToStream(stream, fileFormat);
    }

    public void DisposeLastDocument()
    {
        var lastDocument = _documents.Last();

        lastDocument.Dispose();

        _documents.Remove(lastDocument);
    }

    public void DisposeAllDocuments()
    {
        foreach (var document in _documents)
        {
            document.Dispose();
        }

        _documents.Clear();
    }
}