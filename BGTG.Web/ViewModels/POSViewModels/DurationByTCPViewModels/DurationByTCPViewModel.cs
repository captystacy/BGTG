using System;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.Web.ViewModels.POSViewModels.DurationByTCPViewModels
{
    public class DurationByTCPViewModel : ViewModelBase
    {
        public decimal RoundedDuration { get; set; }
        public decimal PreparatoryPeriod { get; set; }
        public int PipelineDiameter { get; set; }
        public decimal PipelineLength { get; set; }
        public string PipelineMaterial { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}