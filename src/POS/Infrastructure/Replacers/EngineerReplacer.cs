using POS.Infrastructure.Helpers;
using POS.Infrastructure.Replacers.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Models;

namespace POS.Infrastructure.Replacers
{
    public class EngineerReplacer : IEngineerReplacer
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        private const string ChiefOrganizationEngineerSecondNamePattern = "%COESN%";
        private const string ChiefOrganizationEngineerFullNamePattern = "%COEFN%";
        private const string ChiefOrganizationEngineerSignaturePattern = "%COES%";
        private const string ChiefProjectEngineerSecondNamePattern = "%CPESN%";
        private const string ChiefProjectEngineerFullNamePattern = "%CPEFN%";
        private const string ChiefProjectEngineerSignaturePattern = "%CPES%";
        private const string ProjectEngineerSecondNamePattern = "%PESN%";
        private const string ProjectEngineerSignaturePattern = "%PES%";
        private const string ChiefEngineerSecondNamePattern = "%CESN%";
        private const string ChiefEngineerSignaturePattern = "%CES%";
        private const string NormalInspectionEngineerSecondNamePattern = "%NIESN%";
        private const string NormalInspectionEngineerSignaturePattern = "%NIES%";

        private const string EngineerSignaturesPath = @"Infrastructure\Templates\EngineerSignatures";

        public EngineerReplacer(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public Task ReplaceSecondNameAndSignature(IMyWordDocument document, Engineer engineer, TypeOfEngineer typeOfEngineer)
        {
            string engineerSecondNamePattern;
            string engineerSignaturePattern;
            switch (typeOfEngineer)
            {
                case TypeOfEngineer.ProjectEngineer:
                    engineerSecondNamePattern = ProjectEngineerSecondNamePattern;
                    engineerSignaturePattern = ProjectEngineerSignaturePattern;
                    break;
                case TypeOfEngineer.NormalInspectionProjectEngineer:
                    engineerSecondNamePattern = NormalInspectionEngineerSecondNamePattern;
                    engineerSignaturePattern = NormalInspectionEngineerSignaturePattern;
                    break;
                case TypeOfEngineer.ChiefEngineer:
                    engineerSecondNamePattern = ChiefEngineerSecondNamePattern;
                    engineerSignaturePattern = ChiefEngineerSignaturePattern;
                    break;
                case TypeOfEngineer.ChiefProjectEngineer:
                    engineerSecondNamePattern = ChiefProjectEngineerSecondNamePattern;
                    engineerSignaturePattern = ChiefProjectEngineerSignaturePattern;
                    var chiefProjectEngineerFullName = EnumHelper<Engineer>.GetDisplayName(engineer);
                    document.Replace(ChiefProjectEngineerFullNamePattern, chiefProjectEngineerFullName);
                    break;
                case TypeOfEngineer.ChiefOrganizationEngineer:
                    engineerSecondNamePattern = ChiefOrganizationEngineerSecondNamePattern;
                    engineerSignaturePattern = ChiefOrganizationEngineerSignaturePattern;
                    var chiefOrganizationEngineerFullName = EnumHelper<Engineer>.GetDisplayName(engineer);
                    document.Replace(ChiefOrganizationEngineerFullNamePattern, chiefOrganizationEngineerFullName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeOfEngineer), typeOfEngineer, null);
            }

            var engineerSecondName = EnumHelper<Engineer>.GetDisplayShortName(engineer);
            document.Replace(engineerSecondNamePattern, engineerSecondName);

            if (engineer != Engineer.Unknown)
            {
                var engineerSignaturePath = Path.Combine(_webHostEnvironment.ContentRootPath, EngineerSignaturesPath, $"{engineer}.png");
                document.ReplaceTextWithImage(engineerSignaturePattern, engineerSignaturePath);
                return Task.CompletedTask;
            }

            document.Replace(engineerSignaturePattern, string.Empty);

            return Task.CompletedTask;
        }
    }
}