using POS.ViewModels;

namespace POS.Infrastructure.Services.Base;

public interface ITableOfContentsService
{
    MemoryStream Write(TableOfContentsViewModel dto);
}