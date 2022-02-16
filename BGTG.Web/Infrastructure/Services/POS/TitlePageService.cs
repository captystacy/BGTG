using System.IO;
using BGTG.POS;
using BGTG.POS.TitlePageTool.Base;
using BGTG.Web.Infrastructure.Auth;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.POS.Base;
using BGTG.Web.ViewModels.POS;
using Microsoft.AspNetCore.Hosting;

namespace BGTG.Web.Infrastructure.Services.POS
{
    public class TitlePageService : ITitlePageService
    {
        private readonly ITitlePageWriter _titlePageWriter;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private const string TemplatesPath = @"AppData\Templates\TitlePageTemplates";
        private const string UserFilesPath = @"AppData\UserFiles\TitlePageFiles";

        public TitlePageService(ITitlePageWriter titlePageWriter, IWebHostEnvironment webHostEnvironment)
        {
            _titlePageWriter = titlePageWriter;
            _webHostEnvironment = webHostEnvironment;
        }

        public void Write(TitlePageViewModel viewModel)
        {
            var templatePath = GetTemplatePath(viewModel.ChiefProjectEngineer);
            var savePath = GetSavePath();

            _titlePageWriter.Write(viewModel.ObjectCipher, viewModel.ObjectName, templatePath, savePath);
        }

        private string GetTemplatePath(ChiefProjectEngineer chiefProjectEngineer)
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath, $"{chiefProjectEngineer}.docx");
        }

        public string GetSavePath()
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, UserFilesPath, $"{IdentityHelper.Instance.User!.Name!.RemoveBackslashes()}.docx");
        }
    }
}
