using System;
using BGTG.POS.TitlePageTool.Interfaces;
using Xceed.Words.NET;

namespace BGTG.POS.TitlePageTool
{
    public class TitlePageWriter : ITitlePageWriter
    {
        private const string NamePattern = "%NAME%";
        private const string CipherPattern = "%CIPHER%";
        private const string YearPattern = "%YEAR%";

        public void Write(string objectCipher, string objectName, string templatePath, string savePath)
        {
            using var document = DocX.Load(templatePath);

            document.ReplaceText(NamePattern, objectName);
            document.ReplaceText(CipherPattern, objectCipher);
            document.ReplaceText(YearPattern, DateTime.Now.Year.ToString());

            document.SaveAs(savePath);
        }
    }
}
