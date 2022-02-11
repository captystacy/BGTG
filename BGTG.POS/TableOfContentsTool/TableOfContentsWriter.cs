using System;
using BGTG.Core;
using BGTG.POS.TableOfContentsTool.Interfaces;
using Xceed.Words.NET;

namespace BGTG.POS.TableOfContentsTool
{
    public class TableOfContentsWriter : ITableOfContentsWriter
    {
        private const string CipherPattern = "%CIPHER%";
        private const string DatePattern = "%DATE%";

        public void Write(string objectCipher, string templatePath, string savePath)
        {
            using var document = DocX.Load(templatePath);

            document.ReplaceText(CipherPattern, objectCipher);
            document.ReplaceText(DatePattern, DateTime.Now.ToString(AppData.DateTimeMonthAndYearShortFormat));

            document.SaveAs(savePath);
        }
    }
}
