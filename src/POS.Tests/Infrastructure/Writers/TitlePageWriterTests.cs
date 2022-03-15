using System.IO;
using NUnit.Framework;
using POS.Infrastructure.Writers;
using Xceed.Words.NET;

namespace POS.Tests.Infrastructure.Writers
{
    public class TitlePageWriterTests
    {
        private TitlePageWriter _titlePageWriter = null!;

        private const string TitlePageTemplatesDirectory = @"..\..\..\Infrastructure\Templates\TitlePageTemplates";

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
            var objectName = "Электроснабжение станции катодной защиты (СКЗ)№36 аг.Снов Несвижского района";
            var objectCipher = "5.5-20.548";

            var memoryStream = _titlePageWriter.Write(objectCipher, objectName, templatePath);

            using var document = DocX.Load(memoryStream);
        }
    }
}
