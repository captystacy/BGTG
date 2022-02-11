using System.IO;
using BGTG.POS.TitlePageTool;
using NUnit.Framework;
using Xceed.Words.NET;

namespace BGTG.POS.Tests.TitlePageTool
{
    public class TitlePageWriterTests
    {
        private TitlePageWriter _titlePageWriter;

        private const string TitlePageFileName = "TitlePage.docx";
        private const string TitlePageTemplatesDirectory = @"..\..\..\TitlePageTool\TitlePageTemplates";

        [SetUp]
        public void SetUp()
        {
            _titlePageWriter = new TitlePageWriter();
        }

        [Test]
        public void Write_ConstructionObject548()
        {
            var templateFileName = "Saiko.docx";
            var templatePath = Path.Combine(TitlePageTemplatesDirectory, templateFileName);
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), TitlePageFileName);
            var objectName = "Электроснабжение станции катодной защиты (СКЗ)№36 аг.Снов Несвижского района";
            var objectCipher = "5.5-20.548";

            _titlePageWriter.Write(objectCipher, objectName, templatePath, savePath);

            using var document = DocX.Load(savePath);
        }
    }
}
