using POS.DomainModels;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;
using POS.ViewModels;

namespace POS.Infrastructure.Services;

public class TitlePageService : ITitlePageService
{
    private readonly ITitlePageWriter _titlePageWriter;
    private readonly IWebHostEnvironment _webHostEnvironment;

    private const string TemplatesPath = @"Infrastructure\Templates\TitlePageTemplates";

    public TitlePageService(ITitlePageWriter titlePageWriter, IWebHostEnvironment webHostEnvironment)
    {
        _titlePageWriter = titlePageWriter;
        _webHostEnvironment = webHostEnvironment;
    }

    public MemoryStream Write(TitlePageViewModel dto)
    {
        var templatePath = GetTemplatePath(dto.ChiefProjectEngineer);

        return _titlePageWriter.Write(dto.ObjectCipher, dto.ObjectName, templatePath);
    }

    private string GetTemplatePath(ChiefProjectEngineer chiefProjectEngineer)
    {
        return Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath, $"{chiefProjectEngineer}.docx");
    }
}