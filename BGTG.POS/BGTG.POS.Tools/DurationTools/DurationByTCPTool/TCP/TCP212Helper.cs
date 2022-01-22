using System;
using System.Linq;
using BGTG.POS.Tools.DurationTools.DurationByTCPTool.TCP.Interfaces;

namespace BGTG.POS.Tools.DurationTools.DurationByTCPTool.TCP
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

        public PipelineCharacteristic GetPipelineCharacteristic(Appendix appendix, string pipelineMaterial, int pipelineDiameter, string pipelineCategoryName)
        {
            return appendix
                .PipelineCategories
                .Single(x => x.Name == pipelineCategoryName)
                .PipelineComponents
                .Single(x => x.PipelineMaterials.Contains(pipelineMaterial))
                .PipelineCharacteristics
                .SingleOrDefault(x => x.DiameterRange.IsInRange(pipelineDiameter));
        }
    }
}
