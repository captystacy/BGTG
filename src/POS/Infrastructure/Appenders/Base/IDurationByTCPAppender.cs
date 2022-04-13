using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Models.DurationByTCPModels;

namespace POS.Infrastructure.Appenders.Base
{
    public interface IDurationByTCPAppender
    {
        Task AppendAsync(IMySection section, DurationByTCP durationByTCP);
    }
}