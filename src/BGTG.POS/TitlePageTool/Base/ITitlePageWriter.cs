namespace BGTG.POS.TitlePageTool.Base
{
    public interface ITitlePageWriter
    {
        void Write(string objectCipher, string objectName, string templatePath, string savePath);
    }
}
