using System.ComponentModel.DataAnnotations;

namespace BGTGWeb.Models
{
    public class DurationByTCPVM
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