namespace POS.Infrastructure.Services.DocumentServices.WordService.Base
{
    public interface IMyWordDocument : IDisposable
    {
        IReadOnlyList<IMySection> Sections { get; }
        IMySection AddSection();
        void Save(string fileName);
        int Replace(string matchString, string newValue);
        void ReplaceTextWithImage(string searchValue, string imagePath);
        void SaveAs(Stream stream, MyFileFormat fileFormat);
        void ReplaceTextWithTable(string searchValue, IMyTable myTable);
    }
}