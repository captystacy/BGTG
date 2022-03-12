namespace POS.Infrastructure.Tools.TitlePageTool;

public interface ITitlePageWriter
{
    MemoryStream Write(string objectCipher, string objectName, string templatePath);
}