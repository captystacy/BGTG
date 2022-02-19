using System.IO;
using BGTG.Entities.Core;
using BGTG.POS.TableOfContentsTool.Base;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels;
using Microsoft.AspNetCore.Hosting;

namespace BGTG.Web.Infrastructure.Services.POSServices
{
    public class TableOfContentsService : ITableOfContentsService
    {
        private readonly ITableOfContentsWriter _tableOfContentsWriter;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private const string TemplatesPath = @"AppData\Templates\POSTemplates\TableOfContentsTemplates";
        private const string UserFilesPath = @"AppData\UserFiles\POSFiles\TableOfContentsFiles";

        public TableOfContentsService(ITableOfContentsWriter tableOfContentsWriter, IWebHostEnvironment webHostEnvironment)
        {
            _tableOfContentsWriter = tableOfContentsWriter;
            _webHostEnvironment = webHostEnvironment;
        }

        public void Write(TableOfContentsViewModel viewModel)
        {
            var templatePath = GetTemplatePath(viewModel);
            var savePath = GetSavePath();

            _tableOfContentsWriter.Write(viewModel.ObjectCipher, templatePath, savePath);
        }

        private string GetTemplatePath(TableOfContentsViewModel viewModel)
        {
            var templatePath = Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath,
                viewModel.ProjectTemplate.ToString(), viewModel.ChiefProjectEngineer.ToString(),
                $"{IdentityHelper.Instance.User!.Name!.RemoveBackslashes()}.docx");

            if (!File.Exists(templatePath))
            {
                return Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath,
                    viewModel.ProjectTemplate.ToString(), viewModel.ChiefProjectEngineer.ToString(),
                    $"{AppData.Unknown}.docx");
            }

            return templatePath;
        }

        public string GetSavePath()
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, UserFilesPath, $"{IdentityHelper.Instance.User!.Name!.RemoveBackslashes()}.docx");
        }
    }
}
