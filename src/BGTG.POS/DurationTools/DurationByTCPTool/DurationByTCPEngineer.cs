using System.Collections.Generic;
using System.Linq;
using BGTG.POS.DurationTools.DurationByTCPTool.Base;
using BGTG.POS.DurationTools.DurationByTCPTool.TCP;

namespace BGTG.POS.DurationTools.DurationByTCPTool
{
    public class DurationByTCPEngineer : IDurationByTCPEngineer
    {
        public DurationCalculationType DurationCalculationType { get; private set; }
        public IEnumerable<PipelineStandard> CalculationPipelineStandards { get; set; } = null!;

        public void DefineCalculationType(IEnumerable<PipelineStandard> pipelineStandards, decimal pipelineLength)
        {
            var temp = new PipelineStandard(pipelineLength, 0, 0);

            var ordered =
                pipelineStandards
                .Append(temp)
                .OrderBy(x => x.PipelineLength)
                .ToList();

            var tempIndex = ordered.IndexOf(temp);

            if (tempIndex == 0)
            {
                DurationCalculationType = pipelineLength < ordered[1].PipelineLength / 2
                    ? DurationCalculationType.StepwiseExtrapolationAscending
                    : DurationCalculationType.ExtrapolationAscending;
                CalculationPipelineStandards = new[] { ordered[1] };
            }
            else if (tempIndex == ordered.Count - 1)
            {
                DurationCalculationType = pipelineLength > ordered[^2].PipelineLength * 2
                    ? DurationCalculationType.StepwiseExtrapolationDescending
                    : DurationCalculationType.ExtrapolationDescending;
                CalculationPipelineStandards = new[] { ordered[^2] };
            }
            else
            {
                DurationCalculationType = DurationCalculationType.Interpolation;
                CalculationPipelineStandards = new[] { ordered[tempIndex - 1], ordered[tempIndex + 1] };
            }
        }
    }
}
