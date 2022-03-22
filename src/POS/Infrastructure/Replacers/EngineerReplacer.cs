using POS.DomainModels;
using POS.Infrastructure.Helpers;
using POS.Infrastructure.Services.Base;

namespace POS.Infrastructure.Replacers;

public class EngineerReplacer : IEngineerReplacer
{
    private readonly IWordDocumentService _wordDocumentService;
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

    public EngineerReplacer(IWordDocumentService wordDocumentService, IWebHostEnvironment webHostEnvironment)
    {
        _wordDocumentService = wordDocumentService;
        _webHostEnvironment = webHostEnvironment;
    }

    public void ReplaceEngineerSecondNameAndSignature(Engineer engineer, TypeOfEngineer typeOfEngineer)
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
                _wordDocumentService.ReplaceTextInDocument(ChiefProjectEngineerFullNamePattern, chiefProjectEngineerFullName);
                break;
            case TypeOfEngineer.ChiefOrganizationEngineer:
                engineerSecondNamePattern = ChiefOrganizationEngineerSecondNamePattern;
                engineerSignaturePattern = ChiefOrganizationEngineerSignaturePattern;
                var chiefOrganizationEngineerFullName = EnumHelper<Engineer>.GetDisplayName(engineer);
                _wordDocumentService.ReplaceTextInDocument(ChiefOrganizationEngineerFullNamePattern, chiefOrganizationEngineerFullName);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(typeOfEngineer), typeOfEngineer, null);
        }

        var engineerSecondName = EnumHelper<Engineer>.GetDisplayShortName(engineer);
        _wordDocumentService.ReplaceTextInDocument(engineerSecondNamePattern, engineerSecondName);

        if (engineer != Engineer.Unknown)
        {
            var engineerSignaturePath = Path.Combine(_webHostEnvironment.ContentRootPath, EngineerSignaturesPath, $"{engineer}.png");
            _wordDocumentService.ReplaceTextWithImage(engineerSignaturePattern, engineerSignaturePath);
            return;
        }

        _wordDocumentService.ReplaceTextInDocument(engineerSignaturePattern, string.Empty);
    }
}