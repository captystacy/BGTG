using System.Collections.Generic;
using BGTGWeb.ViewModels;
using Microsoft.AspNetCore.Http;

namespace BGTGWeb.Services.Interfaces
{
    public interface IDurationByLaborCostsService : ISavable
    {
        void Write(IEnumerable<IFormFile> estimateFiles, DurationByLaborCostsViewModel durationByLaborCostsViewModel, string userFullName);
    }
}
