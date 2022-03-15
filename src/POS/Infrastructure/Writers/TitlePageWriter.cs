using POS.Infrastructure.Services;
using POS.Infrastructure.Writers.Base;

namespace POS.Infrastructure.Writers;

public class TitlePageWriter : ITitlePageWriter
{
    private const string NamePattern = "%NAME%";
    private const string CipherPattern = "%CIPHER%";
    private const string YearPattern = "%YEAR%";

    public MemoryStream Write(string objectCipher, string objectName, string templatePath)
    {
        using var document = DocumentService.Load(templatePath);

        document.ReplaceText(NamePattern, objectName);
        document.ReplaceText(CipherPattern, objectCipher);
        document.ReplaceText(YearPattern, DateTime.Now.Year.ToString());

        var memoryStream = new MemoryStream();
        document.SaveAs(memoryStream);

        return memoryStream;
    }
}