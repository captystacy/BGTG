using Calabonga.OperationResults;
using POS.Models;

namespace POS.Infrastructure.Readers.Base
{
    public interface IDurationByLCReader
    {
        Task<OperationResult<DurationByLC>> GetDurationByLC(Stream stream);
    }
}