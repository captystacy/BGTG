using System;
using System.Linq;
using BGTG.POS.DurationTools.DurationByTCPTool.TCP.Interfaces;

namespace BGTG.POS.DurationTools.DurationByTCPTool.TCP
{
    public class TCP212Helper : ITCP212Helper
    {
        public Appendix GetAppendix(char appendixKey)
        {
            return appendixKey switch
            {
                'A' => TCP212.AppendixA,
                'B' => TCP212.AppendixB,
                _ => throw new ArgumentOutOfRangeException(nameof(appendixKey), appendixKey, null)
            };
        }

        public PipelineCharacteristic? GetPipelineCharacteristic(Appendix appendix, string pipelineMaterial, int pipelineDiameter, string pipelineCategoryName)
        {
            return appendix
                .PipelineCategories
                .First(x => x.Name == pipelineCategoryName)
                .PipelineComponents
                .First(x => x.PipelineMaterials.Contains(pipelineMaterial))
                .PipelineCharacteristics
                .FirstOrDefault(x => x.DiameterRange.IsInRange(pipelineDiameter));
        }
    }
}
