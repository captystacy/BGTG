namespace BGTG.POS.TitlePageTool.Interfaces
{
    public interface ITitlePageWriter
    {
        void Write(string objectCipher, string objectName, string templatePath, string savePath);
    }
}
