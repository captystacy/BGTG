namespace POS.Infrastructure.Writers.Base;

public interface ITitlePageWriter
{
    MemoryStream Write(string objectCipher, string objectName, string templatePath);
}