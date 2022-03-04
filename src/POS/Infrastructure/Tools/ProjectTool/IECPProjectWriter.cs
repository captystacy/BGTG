namespace POS.Infrastructure.Tools.ProjectTool;

public interface IECPProjectWriter
{
    void Write(string objectCipher, string templatePath);
}