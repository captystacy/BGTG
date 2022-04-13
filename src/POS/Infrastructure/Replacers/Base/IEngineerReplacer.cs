using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Models;

namespace POS.Infrastructure.Replacers.Base
{
    public interface IEngineerReplacer
    {
        Task ReplaceSecondNameAndSignature(IMyWordDocument document, Engineer engineer, TypeOfEngineer typeOfEngineer);
    }
}