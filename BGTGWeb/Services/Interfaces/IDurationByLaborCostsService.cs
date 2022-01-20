using System.Collections.Generic;
using BGTGWeb.Models;
using Microsoft.AspNetCore.Http;

namespace BGTGWeb.Services.Interfaces
{
    public interface IDurationByLaborCostsService : ISavable
    {
        void Write(IEnumerable<IFormFile> estimateFiles, DurationByLaborCostsVM durationByLaborCostsVM, string userFullName);
    }
}
