using System;
using BGTG.Web.ViewModels.POSViewModels;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.Web.ViewModels.ConstructionObjectViewModels
{
    public class ConstructionObjectViewModel : ViewModelBase
    {
        public string Cipher { get; set; }
        public POSViewModel POSViewModel { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}