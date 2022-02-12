using System.IO;
using BGTG.Core;
using BGTG.POS.TableOfContentsTool.Interfaces;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.POSServices.Interfaces;
using BGTG.Web.ViewModels.POSViewModels;
using Microsoft.AspNetCore.Hosting;

namespace BGTG.Web.Infrastructure.Services.POSServices
{
    public class TableOfContentsService : ITableOfContentsService
    {
        private readonly ITableOfContentsWriter _tableOfContentsWriter;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private const string TemplatesPath = @"AppData\Templates\TableOfContentsTemplates";
        private const string UserFilesPath = @"AppData\UserFiles\TableOfContentsFiles";

        public TableOfContentsService(ITableOfContentsWriter tableOfContentsWriter, IWebHostEnvironment webHostEnvironment)
        {
            _tableOfContentsWriter = tableOfContentsWriter;
            _webHostEnvironment = webHostEnvironment;
        }

        public void Write(TableOfContentsViewModel viewModel, string windowsName)
        {
            var templatePath = GetTemplatePath(viewModel, windowsName);
            var savePath = GetSavePath(windowsName);

            _tableOfContentsWriter.Write(viewModel.ObjectCipher, templatePath, savePath);
        }

        private string GetTemplatePath(TableOfContentsViewModel viewModel, string windowsName)
        {
            var templatePath = Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath,
                viewModel.ProjectTemplate.ToString(), viewModel.ChiefProjectEngineer.ToString(),
                $"{windowsName.RemoveBackslashes()}.docx");

            if (!File.Exists(templatePath))
            {
                return Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath,
                    viewModel.ProjectTemplate.ToString(), viewModel.ChiefProjectEngineer.ToString(),
                    $"{AppData.Unknown}.docx");
            }

            return templatePath;
        }

        public string GetSavePath(string windowsName)
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, UserFilesPath, $"{windowsName.RemoveBackslashes()}.docx");
        }
    }
}
