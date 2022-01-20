using System.ComponentModel.DataAnnotations;

namespace BGTGWeb.ViewModels
{
    public class DurationByTCPViewModel
    {
        public char AppendixKey { get; set; }
        public string PipelineCategoryName { get; set; }
        public string PipelineMaterial { get; set; }

        [Range(0, int.MaxValue)]
        public int PipelineDiameter { get; set; }

        [Range(0, double.MaxValue)]
        public decimal PipelineLength { get; set; }
    }
}