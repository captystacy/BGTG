using POS.DomainModels;

namespace POS.Infrastructure.Replacers;

public interface IEngineerReplacer
{
    void ReplaceEngineerSecondNameAndSignature(Engineer engineer, TypeOfEngineer typeOfEngineer);
}