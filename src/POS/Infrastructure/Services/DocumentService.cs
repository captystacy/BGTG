using POS.DomainModels.DocumentDomainModels;
using Xceed.Words.NET;

namespace POS.Infrastructure.Services;

public static class DocumentService
{
    public static MyDocument Load(string path)
    {
        return new MyDocument(DocX.Load(path));
    }

    public static MyDocument Load(Stream stream)
    {
        return new MyDocument(DocX.Load(stream));
    }
}