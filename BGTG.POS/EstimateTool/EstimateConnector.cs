using System.Collections.Generic;
using System.Linq;
using BGTG.POS.EstimateTool.Base;

namespace BGTG.POS.EstimateTool
{
    public class EstimateConnector : IEstimateConnector
    {
        public Estimate Connect(List<Estimate> estimates)
        {
            if (estimates.Count == 1)
            {
                return estimates[0];
            }

            var preparatoryEstimateWorks = estimates.SelectMany(x => x.PreparatoryEstimateWorks);
            var mainEstimateWorks = estimates.SelectMany(x => x.MainEstimateWorks);

            var preparatoryEstimateWorksConnected = ConnectEstimateWorks(preparatoryEstimateWorks);
            var mainEstimateWorksConnected = ConnectEstimateWorks(mainEstimateWorks);

            var laborCosts = estimates.Sum(e => e.LaborCosts);

            return new Estimate(preparatoryEstimateWorksConnected, 
                mainEstimateWorksConnected, 
                estimates[0].ConstructionStartDate, 
                estimates[0].ConstructionDuration, 
                estimates[0].ConstructionDurationCeiling, 
                laborCosts);
        }

        private List<EstimateWork> ConnectEstimateWorks(IEnumerable<EstimateWork> estimateWorks)
        {
            return estimateWorks
                .GroupBy(x => x.WorkName)
                .Select(x=> SumEstimateWork(x.ToList()))
                .ToList();
        }

        private EstimateWork SumEstimateWork(List<EstimateWork> estimateWorks)
        {
            return new EstimateWork(estimateWorks[0].WorkName,
                estimateWorks.Sum(x => x.EquipmentCost),
                estimateWorks.Sum(x => x.OtherProductsCost),
                estimateWorks.Sum(x => x.TotalCost),
                estimateWorks[0].Chapter);
        }
    }
}
