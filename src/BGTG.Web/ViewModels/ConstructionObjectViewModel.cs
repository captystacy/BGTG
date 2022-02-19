using System;
using BGTG.Web.ViewModels.POSViewModels;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.Web.ViewModels
{
    public class ConstructionObjectViewModel : ViewModelBase
    {
        public string Cipher { get; set; } = null!;
        public POSViewModel? POSViewModel { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; } = null!;
    }
}