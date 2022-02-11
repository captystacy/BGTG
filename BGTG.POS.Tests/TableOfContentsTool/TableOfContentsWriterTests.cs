using System.IO;
using BGTG.POS.TableOfContentsTool;
using NUnit.Framework;
using Xceed.Words.NET;

namespace BGTG.POS.Tests.TableOfContentsTool
{
    public class TableOfContentsWriterTests
    {
        private TableOfContentsWriter _tableOfContentsWriter;

        private const string TableOfContentsFileName = "TableOfContents.docx";
        private const string TableOfContentsTemplatesDirectory = @"..\..\..\TableOfContentsTool\TableOfContentsTemplates\ECP\";

        [SetUp]
        public void SetUp()
        {
            _tableOfContentsWriter = new TableOfContentsWriter();
        }

        [Test]
        public void Write_ConstructionObject548()
        {
            var templateFileName = "Saiko.docx";
            var templatePath = Path.Combine(TableOfContentsTemplatesDirectory, templateFileName);
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), TableOfContentsFileName);
            var objectCipher = "5.5-20.548";

            _tableOfContentsWriter.Write(objectCipher, templatePath, savePath);

            using var document = DocX.Load(savePath);
        }
    }
}
