using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Models;

namespace POS.Infrastructure.Appenders.Base
{
    public interface IDurationByLCAppender
    {
        Task AppendAsync(IMySection section, DurationByLC durationByLC);
    }
}