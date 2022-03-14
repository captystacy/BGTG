using POS.Infrastructure.Services.Base;
using Xceed.Words.NET;

namespace POS.Infrastructure.Services;

public class DocumentService : IDocumentService
{
    public int DocumentIndex { get; set; } = 0;
    public int TableIndex { get; set; } = 0;

    private readonly List<DocX> _documents = new();

    public void Load(string path)
    {
        _documents.Add(DocX.Load(path));
    }

    public void ReplaceText(int rowIndex, string searchValue, string newValue)
    {
        _documents[DocumentIndex].Tables[TableIndex].Rows[rowIndex].ReplaceText(searchValue, newValue);
    }

    public void InsertTemplateRow(int templateRowIndex, int insertionIndex)
    {
        var templateRow = _documents[DocumentIndex].Tables[TableIndex].Rows[templateRowIndex];
        _documents[DocumentIndex].Tables[TableIndex].InsertRow(templateRow, insertionIndex);
    }

    public void RemoveRow(int rowIndex)
    {
        _documents[DocumentIndex].Tables[TableIndex].Rows[rowIndex].Remove();
    }

    public int GetRowCount()
    {
        return _documents[DocumentIndex].Tables[TableIndex].RowCount;
    }

    public int GetColumnCount()
    {
        return _documents[DocumentIndex].Tables[TableIndex].ColumnCount;
    }

    public int GetParagraphsCount(int rowIndex)
    {
        return _documents[DocumentIndex].Tables[TableIndex].Rows[rowIndex].Paragraphs.Count;
    }

    public string GetParagraphText(int rowIndex, int paragraphIndex)
    {
        return _documents[DocumentIndex].Tables[TableIndex].Rows[rowIndex].Paragraphs[paragraphIndex].Text;
    }

    public void RemoveText(int rowIndex, int paragraphIndex)
    {
        var paragraph = _documents[DocumentIndex].Tables[TableIndex].Rows[rowIndex].Paragraphs[paragraphIndex];
        paragraph.RemoveText(0, paragraph.Text.Length);
    }

    public void MergeCellsInColumn(int columnIndex, int startRowIndex, int endRowIndex)
    {
        _documents[DocumentIndex].Tables[TableIndex].MergeCellsInColumn(columnIndex, startRowIndex, endRowIndex);
    }

    public void Append(string text, int rowIndex, int cellIndex, int paragraphIndex, int fontSize = 12)
    {
        _documents[DocumentIndex].Tables[TableIndex].Rows[rowIndex].Cells[cellIndex].Paragraphs[paragraphIndex].Append(text).FontSize(fontSize);
    }

    public void InsertDocument(int toDocumentIndex, int fromDocumentIndex)
    {
        _documents[toDocumentIndex].InsertDocument(_documents[fromDocumentIndex]);
    }

    public void SaveAs(Stream stream, int documentIndex)
    {
        _documents[documentIndex].SaveAs(stream);
    }

    public void Dispose()
    {
        foreach (var document in _documents)
        {
            document.Dispose();
        }
    }
}