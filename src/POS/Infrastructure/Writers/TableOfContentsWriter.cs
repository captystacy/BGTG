using POS.Infrastructure.Constants;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;

namespace POS.Infrastructure.Writers;

public class TableOfContentsWriter : ITableOfContentsWriter
{
    private readonly IDocumentService _documentService;

    private const string CipherPattern = "%CIPHER%";
    private const string DatePattern = "%DATE%";

    public TableOfContentsWriter(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    public MemoryStream Write(string objectCipher, string templatePath)
    {
        _documentService.Load(templatePath);

        _documentService.ReplaceTextInDocument(CipherPattern, objectCipher);
        _documentService.ReplaceTextInDocument(DatePattern, DateTime.Now.ToString(AppConstants.DateTimeMonthAndYearShortFormat));

        var memoryStream = new MemoryStream();
        _documentService.SaveAs(memoryStream);
        _documentService.DisposeLastDocument();

        return memoryStream;
    }
}