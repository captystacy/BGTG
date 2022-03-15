using POS.Infrastructure.Constants;
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

    public MemoryStream Write(TableOfContentsViewModel dto)
    {
        var templatePath = GetTemplatePath(dto);

        return _tableOfContentsWriter.Write(dto.ObjectCipher, templatePath);
    }

    private string GetTemplatePath(TableOfContentsViewModel dto)
    {
        var templatePath = Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath,
            dto.ProjectTemplate.ToString(), dto.ChiefProjectEngineer.ToString(), ".docx");

        if (!File.Exists(templatePath))
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath,
                dto.ProjectTemplate.ToString(), dto.ChiefProjectEngineer.ToString(),
                $"{AppConstants.Unknown}.docx");
        }

        return templatePath;
    }
}