using POS.Infrastructure.Services.DocumentServices.WordService.Format;

namespace POS.Infrastructure.Services.DocumentServices.WordService.Base
{
    public interface IMyCell : IMyBody
    {
        IMyCell ApplyFormat(MyCellFormat format);
    }
}