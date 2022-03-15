using System.IO;
using NUnit.Framework;
using POS.Infrastructure.Writers;
using Xceed.Words.NET;

namespace POS.Tests.Infrastructure.Writers
{
    public class TableOfContentsWriterTests
    {
        private TableOfContentsWriter _tableOfContentsWriter = null!;

        private const string TableOfContentsTemplatesDirectory = @"..\..\..\Infrastructure\Templates\TableOfContentsTemplates\ECP\Saiko";

        [SetUp]
        public void SetUp()
        {
            _tableOfContentsWriter = new TableOfContentsWriter();
        }

        [Test]
        public void Write_ConstructionObject548()
        {
            var templatePath = Path.Combine(TableOfContentsTemplatesDirectory, "Kapitan.docx");
            var objectCipher = "5.5-20.548";

            var memoryStream = _tableOfContentsWriter.Write(objectCipher, templatePath);

            using var document = DocX.Load(memoryStream);
        }
    }
}
