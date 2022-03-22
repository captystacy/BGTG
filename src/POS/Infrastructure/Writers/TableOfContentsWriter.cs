using POS.DomainModels;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Replacers;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;
using POS.ViewModels;

namespace POS.Infrastructure.Writers;

public class TableOfContentsWriter : ITableOfContentsWriter
{
    private readonly IWordDocumentService _wordDocumentService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IEngineerReplacer _engineerReplacer;

    private const string CipherPattern = "%CIPHER%";
    private const string DatePattern = "%DATE%";

    private const string TemplatesPath = @"Infrastructure\Templates\TableOfContentsTemplates";

    public TableOfContentsWriter(IWordDocumentService wordDocumentService, IWebHostEnvironment webHostEnvironment, IEngineerReplacer engineerReplacer)
    {
        _wordDocumentService = wordDocumentService;
        _webHostEnvironment = webHostEnvironment;
        _engineerReplacer = engineerReplacer;
    }

    public MemoryStream Write(TableOfContentsViewModel viewModel)
    {
        var templatePath = Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath, $"{viewModel.ProjectTemplate}TableOfContentsTemplate.doc");

        _wordDocumentService.Load(templatePath);

        _engineerReplacer.ReplaceEngineerSecondNameAndSignature(viewModel.ChiefProjectEngineer, TypeOfEngineer.ChiefProjectEngineer);
        _engineerReplacer.ReplaceEngineerSecondNameAndSignature(viewModel.NormalInspectionEngineer, TypeOfEngineer.NormalInspectionProjectEngineer);
        _wordDocumentService.ReplaceTextInDocument(CipherPattern, viewModel.ObjectCipher);
        _wordDocumentService.ReplaceTextInDocument(DatePattern, DateTime.Now.ToString(AppConstants.DateTimeMonthAndYearShortFormat));

        var memoryStream = new MemoryStream();
        _wordDocumentService.SaveAs(memoryStream, MyFileFormat.Doc);
        _wordDocumentService.DisposeLastDocument();

        return memoryStream;
    }
}