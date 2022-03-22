using POS.DomainModels;
using POS.Infrastructure.Replacers;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;
using POS.ViewModels;

namespace POS.Infrastructure.Writers;

public class TitlePageWriter : ITitlePageWriter
{
    private readonly IWordDocumentService _wordDocumentService;
    private readonly IEngineerReplacer _engineerReplacer;
    private readonly IWebHostEnvironment _webHostEnvironment;

    private const string TemplatesPath = @"Infrastructure\Templates\TitlePageTemplates\TitlePageTemplate.doc";

    private const string NamePattern = "%NAME%";
    private const string CipherPattern = "%CIPHER%";
    private const string YearPattern = "%YEAR%";

    public TitlePageWriter(IWordDocumentService wordDocumentService, IEngineerReplacer engineerReplacer, IWebHostEnvironment webHostEnvironment)
    {
        _wordDocumentService = wordDocumentService;
        _engineerReplacer = engineerReplacer;
        _webHostEnvironment = webHostEnvironment;
    }

    public MemoryStream Write(TitlePageViewModel viewModel)
    {
        var templatePath = Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath);

        _wordDocumentService.Load(templatePath);
        _engineerReplacer.ReplaceEngineerSecondNameAndSignature(viewModel.ChiefProjectEngineer, TypeOfEngineer.ChiefProjectEngineer);
        _engineerReplacer.ReplaceEngineerSecondNameAndSignature(Engineer.Cherota, TypeOfEngineer.ChiefOrganizationEngineer);
        _wordDocumentService.ReplaceTextInDocument(NamePattern, viewModel.ObjectName);
        _wordDocumentService.ReplaceTextInDocument(CipherPattern, viewModel.ObjectCipher);
        _wordDocumentService.ReplaceTextInDocument(YearPattern, DateTime.Now.Year.ToString());

        var memoryStream = new MemoryStream();
        _wordDocumentService.SaveAs(memoryStream, MyFileFormat.Doc);
        _wordDocumentService.DisposeLastDocument();

        return memoryStream;
    }
}