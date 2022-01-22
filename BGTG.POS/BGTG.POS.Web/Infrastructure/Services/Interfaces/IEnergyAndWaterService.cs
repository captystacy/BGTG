using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace BGTG.POS.Web.Infrastructure.Services.Interfaces
{
    public interface IEnergyAndWaterService : ISavable
    {
        void Write(IFormFileCollection estimateFiles, string userFullName);
    }
}
