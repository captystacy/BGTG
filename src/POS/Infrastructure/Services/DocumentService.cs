using POS.Infrastructure.Services.Base;
using Xceed.Words.NET;

namespace POS.Infrastructure.Services;

public class DocumentService : IDocumentService
{
    public int BaseDocumentIndex { get; set; }
    public bool ReplaceInBaseDocumentMode { get; set; }
    public int CurrentDocumentIndex { get; set; }
    public int TableIndex { get; set; }
    public int RowIndex { get; set; }
    public int CellIndex { get; set; }
    public int ParagraphIndex { get; set; }
    public int RowCount => _documents[CurrentDocumentIndex].Tables[TableIndex].RowCount;
    public int ColumnCountInTable => _documents[CurrentDocumentIndex].Tables[TableIndex].ColumnCount;
    public int ParagraphsCountInRow => _documents[CurrentDocumentIndex].Tables[TableIndex].Rows[RowIndex].Paragraphs.Count;
    public int ParagraphsCountInTable => _documents[CurrentDocumentIndex].Tables[TableIndex].Paragraphs.Count;
    public int ParagraphsCountInDocument => _documents[CurrentDocumentIndex].Paragraphs.Count;
    public string ParagraphTextInRow => _documents[CurrentDocumentIndex].Tables[TableIndex].Rows[RowIndex].Paragraphs[ParagraphIndex].Text;
    public string ParagraphTextInTable => _documents[CurrentDocumentIndex].Tables[TableIndex].Paragraphs[ParagraphIndex].Text;
    public string ParagraphTextInDocument => _documents[CurrentDocumentIndex].Paragraphs[ParagraphIndex].Text;

    private readonly List<DocX> _documents = new();

    public void Load(string path)
    {
        var document = DocX.Load(path);
        SetNewDocument(document);
    }

    public void Load(Stream stream)
    {
        var document = DocX.Load(stream);
        SetNewDocument(document);
    }

    private void SetNewDocument(DocX document)
    {
        _documents.Add(document);
        CurrentDocumentIndex = _documents.Count - 1;
        ResetIndexesBesidesDocumentIndex();
    }

    public void ResetIndexesBesidesDocumentIndex()
    {
        TableIndex = 0;
        RowIndex = 0;
        CellIndex = 0;
        ParagraphIndex = 0;
    }

    public void ResetAllIndexes()
    {
        BaseDocumentIndex = 0;
        CurrentDocumentIndex = 0;
        TableIndex = 0;
        RowIndex = 0;
        CellIndex = 0;
        ParagraphIndex = 0;
    }

    public void ReplaceTextInRow(string searchValue, string newValue)
    {
        _documents[ReplaceInBaseDocumentMode ? BaseDocumentIndex : CurrentDocumentIndex].Tables[TableIndex].Rows[RowIndex].ReplaceText(searchValue, newValue);
    }

    public void ReplaceTextInDocument(string searchValue, string newValue)
    {
        _documents[ReplaceInBaseDocumentMode ? BaseDocumentIndex : CurrentDocumentIndex].ReplaceText(searchValue, newValue);
    }

    public void ReplaceTextWithTable(string searchValue)
    {
        var table = _documents[CurrentDocumentIndex].Tables[TableIndex];
        _documents[ReplaceInBaseDocumentMode ? BaseDocumentIndex : CurrentDocumentIndex].ReplaceTextWithObject(searchValue, table);
    }

    public void InsertTemplateRow(int templateRowIndex, int insertionIndex)
    {
        var templateRow = _documents[CurrentDocumentIndex].Tables[TableIndex].Rows[templateRowIndex];
        _documents[CurrentDocumentIndex].Tables[TableIndex].InsertRow(templateRow, insertionIndex);
    }

    public void RemoveRow()
    {
        _documents[CurrentDocumentIndex].Tables[TableIndex].Rows[RowIndex].Remove();
    }

    public void EmptyParagraphInRow()
    {
        var paragraph = _documents[CurrentDocumentIndex].Tables[TableIndex].Rows[RowIndex].Paragraphs[ParagraphIndex];
        paragraph.RemoveText(0, paragraph.Text.Length);
    }

    public void MergeCellsInColumn(int columnIndex, int startRowIndex, int endRowIndex)
    {
        _documents[CurrentDocumentIndex].Tables[TableIndex].MergeCellsInColumn(columnIndex, startRowIndex, endRowIndex);
    }

    public void AppendInRow(string text, int fontSize = 12)
    {
        _documents[CurrentDocumentIndex].Tables[TableIndex].Rows[RowIndex].Paragraphs[ParagraphIndex].Append(text).FontSize(fontSize);
    }

    public void InsertDocument(int toDocumentIndex, int fromDocumentIndex)
    {
        _documents[toDocumentIndex].InsertDocument(_documents[fromDocumentIndex]);
    }

    public void SaveAs(Stream stream, int documentIndex = 0)
    {
        _documents[documentIndex].SaveAs(stream);
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