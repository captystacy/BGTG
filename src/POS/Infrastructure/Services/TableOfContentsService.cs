using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Tools.TableOfContentsTool;
using POS.ViewModels;

namespace POS.Infrastructure.Services;

public class TableOfContentsService : ITableOfContentsService
{
    private readonly ITableOfContentsWriter _tableOfContentsWriter;
    private readonly IWebHostEnvironment _webHostEnvironment;

    private const string TemplatesPath = @"AppData\Templates\POSTemplates\TableOfContentsTemplates";

    public TableOfContentsService(ITableOfContentsWriter tableOfContentsWriter, IWebHostEnvironment webHostEnvironment)
    {
        _tableOfContentsWriter = tableOfContentsWriter;
        _webHostEnvironment = webHostEnvironment;
    }

    public void Write(TableOfContentsViewModel viewModel)
    {
        var templatePath = GetTemplatePath(viewModel);

        _tableOfContentsWriter.Write(viewModel.ObjectCipher, templatePath);
    }

    private string GetTemplatePath(TableOfContentsViewModel viewModel)
    {
        var templatePath = Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath,
            viewModel.ProjectTemplate.ToString(), viewModel.ChiefProjectEngineer.ToString(),
            $".docx");

        if (!File.Exists(templatePath))
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath,
                viewModel.ProjectTemplate.ToString(), viewModel.ChiefProjectEngineer.ToString(),
                $"{AppData.Unknown}.docx");
        }

        return templatePath;
    }
}