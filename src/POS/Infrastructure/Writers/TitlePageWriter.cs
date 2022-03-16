using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;

namespace POS.Infrastructure.Writers;

public class TitlePageWriter : ITitlePageWriter
{
    private readonly IDocumentService _documentService;
    private const string NamePattern = "%NAME%";
    private const string CipherPattern = "%CIPHER%";
    private const string YearPattern = "%YEAR%";

    public TitlePageWriter(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    public MemoryStream Write(string objectCipher, string objectName, string templatePath)
    {
        _documentService.Load(templatePath);

        _documentService.ReplaceTextInDocument(NamePattern, objectName);
        _documentService.ReplaceTextInDocument(CipherPattern, objectCipher);
        _documentService.ReplaceTextInDocument(YearPattern, DateTime.Now.Year.ToString());

        var memoryStream = new MemoryStream();
        _documentService.SaveAs(memoryStream);
        _documentService.DisposeLastDocument();

        return memoryStream;
    }
}