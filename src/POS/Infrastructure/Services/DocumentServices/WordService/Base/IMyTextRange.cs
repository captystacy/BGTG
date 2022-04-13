using POS.Infrastructure.Services.DocumentServices.WordService.Format;

namespace POS.Infrastructure.Services.DocumentServices.WordService.Base
{
    public interface IMyTextRange
    {
        string Text { get; }
        IMyTextRange ApplyFormat(MyCharacterFormat format);
    }
}