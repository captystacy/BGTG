using POS.Infrastructure.Factories.Base;
using POS.Infrastructure.Replacers.Base;
using POS.Infrastructure.Services.DocumentServices;
using POS.Infrastructure.Writers.Base;
using POS.Models;
using POS.ViewModels;

namespace POS.Infrastructure.Writers
{
    public class TableOfContentsWriter : ITableOfContentsWriter
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEngineerReplacer _engineerReplacer;
        private readonly IProjectReplacer _projectReplacer;
        private readonly IMyWordDocumentFactory _documentFactory;

        private const string TemplatesPath = @"Infrastructure\Templates\TableOfContentsTemplates";

        public TableOfContentsWriter(IMyWordDocumentFactory documentFactory, IWebHostEnvironment webHostEnvironment, IEngineerReplacer engineerReplacer, IProjectReplacer projectReplacer)
        {
            _webHostEnvironment = webHostEnvironment;
            _engineerReplacer = engineerReplacer;
            _projectReplacer = projectReplacer;
            _documentFactory = documentFactory;
        }
        
        public async Task<MemoryStream> GetTableOfContentsStream(TableOfContentsViewModel viewModel)
        {
            var templatePath = Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath, $"{viewModel.ProjectTemplate}TableOfContentsTemplate.doc");

            using var document = await _documentFactory.CreateAsync(templatePath);

            var tasks = new List<Task>
            {
                _engineerReplacer.ReplaceSecondNameAndSignature(document, viewModel.ChiefProjectEngineer, TypeOfEngineer.ChiefProjectEngineer),
                _engineerReplacer.ReplaceSecondNameAndSignature(document, viewModel.NormalInspectionEngineer, TypeOfEngineer.NormalInspectionProjectEngineer),
                _projectReplacer.ReplaceObjectCipher(document, viewModel.ObjectCipher),
                _projectReplacer.ReplaceCurrentDate(document)
            };

            await Task.WhenAll(tasks);

            var memoryStream = new MemoryStream();
            document.SaveAs(memoryStream, MyFileFormat.Doc);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }
    }
}
