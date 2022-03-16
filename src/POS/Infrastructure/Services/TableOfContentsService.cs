using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;
using POS.ViewModels;

namespace POS.Infrastructure.Services;

public class TableOfContentsService : ITableOfContentsService
{
    private readonly ITableOfContentsWriter _tableOfContentsWriter;
    private readonly IWebHostEnvironment _webHostEnvironment;

    private const string TemplatesPath = @"Infrastructure\Templates\TableOfContentsTemplates";

    public TableOfContentsService(ITableOfContentsWriter tableOfContentsWriter, IWebHostEnvironment webHostEnvironment)
    {
        _tableOfContentsWriter = tableOfContentsWriter;
        _webHostEnvironment = webHostEnvironment;
    }

    public MemoryStream Write(TableOfContentsViewModel viewModel)
    {
        var templatePath = GetTemplatePath(viewModel);

        return _tableOfContentsWriter.Write(viewModel.ObjectCipher, templatePath);
    }

    private string GetTemplatePath(TableOfContentsViewModel viewModel)
    {
        return Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath, viewModel.ProjectTemplate.ToString(),
            viewModel.ChiefProjectEngineer.ToString(), viewModel.ProjectEngineer + ".docx");
    }
}