namespace POS.Infrastructure.Tools.TitlePageTool;

public interface ITitlePageWriter
{
    void Write(string objectCipher, string objectName, string templatePath);
}