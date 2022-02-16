using System;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.Web.ViewModels.POS.EnergyAndWaterViewModels
{
    public class EnergyAndWaterViewModel : ViewModelBase
    {
        public decimal Energy { get; set; }
        public decimal Water { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
    }
}