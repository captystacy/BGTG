using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Tools;
using POS.Infrastructure.Tools.TitlePageTool;
using POS.ViewModels;

namespace POS.Infrastructure.Services;

public class TitlePageService : ITitlePageService
{
    private readonly ITitlePageWriter _titlePageWriter;
    private readonly IWebHostEnvironment _webHostEnvironment;

    private const string TemplatesPath = @"Templates\TitlePageTemplates";

    public TitlePageService(ITitlePageWriter titlePageWriter, IWebHostEnvironment webHostEnvironment)
    {
        _titlePageWriter = titlePageWriter;
        _webHostEnvironment = webHostEnvironment;
    }

    public MemoryStream Write(TitlePageViewModel viewModel)
    {
        var templatePath = GetTemplatePath(viewModel.ChiefProjectEngineer);

        return _titlePageWriter.Write(viewModel.ObjectCipher, viewModel.ObjectName, templatePath);
    }

    private string GetTemplatePath(ChiefProjectEngineer chiefProjectEngineer)
    {
        return Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath, $"{chiefProjectEngineer}.docx");
    }
}