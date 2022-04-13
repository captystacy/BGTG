using POS.Infrastructure.Factories.Base;
using POS.Infrastructure.Replacers.Base;
using POS.Infrastructure.Services.DocumentServices;
using POS.Infrastructure.Writers.Base;
using POS.Models;
using POS.ViewModels;

namespace POS.Infrastructure.Writers
{
    public class TitlePageWriter : ITitlePageWriter
    {
        private readonly IMyWordDocumentFactory _documentFactory;
        private readonly IEngineerReplacer _engineerReplacer;
        private readonly IProjectReplacer _projectReplacer;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private const string TemplatesPath = @"Infrastructure\Templates\TitlePageTemplates\TitlePageTemplate.doc";

        public TitlePageWriter(IMyWordDocumentFactory documentFactory, IEngineerReplacer engineerReplacer, IProjectReplacer projectReplacer, IWebHostEnvironment webHostEnvironment)
        {
            _documentFactory = documentFactory;
            _engineerReplacer = engineerReplacer;
            _projectReplacer = projectReplacer;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<MemoryStream> GetTitlePageStream(TitlePageViewModel viewModel)
        {
            var templatePath = Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath);

            using var document = await _documentFactory.CreateAsync(templatePath);

            var tasks = new List<Task>
            {
                _engineerReplacer.ReplaceSecondNameAndSignature(document, viewModel.ChiefProjectEngineer, TypeOfEngineer.ChiefProjectEngineer),
                _engineerReplacer.ReplaceSecondNameAndSignature(document, Engineer.Cherota, TypeOfEngineer.ChiefOrganizationEngineer),
                _projectReplacer.ReplaceObjectName(document, viewModel.ObjectName),
                _projectReplacer.ReplaceObjectCipher(document, viewModel.ObjectCipher),
                _projectReplacer.ReplaceCurrentYear(document),
            };

            await Task.WhenAll(tasks);

            var memoryStream = new MemoryStream();
            document.SaveAs(memoryStream, MyFileFormat.Doc);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }
    }
}