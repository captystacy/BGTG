using POSCore.EstimateLogic.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace POSCore.EstimateLogic
{
    public class EstimateConnector : IEstimateConnector
    {
        public Estimate Connect(List<Estimate> estimates)
        {
            if (estimates.Count == 1)
            {
                return estimates[0];
            }

            var preparatoryEstimateWorksLists = estimates.Select(x => x.PreparatoryEstimateWorks).ToList();
            var mainEstimateWorksLists = estimates.Select(x => x.MainEstimateWorks).ToList();

            var preparatoryEstimateWorksConnected = ConnectEstimateWorks(preparatoryEstimateWorksLists);
            var mainEstimateWorksConnected = ConnectEstimateWorks(mainEstimateWorksLists);

            var laborCosts = estimates.Sum(e => e.LaborCosts);
            return new Estimate(preparatoryEstimateWorksConnected, mainEstimateWorksConnected, estimates[0].ConstructionStartDate, estimates[0].ConstructionDuration, estimates[0].ObjectCipher, laborCosts);
        }

        private List<EstimateWork> ConnectEstimateWorks(List<List<EstimateWork>> estimateWorksLists)
        {
            var estimateWorksConnected = estimateWorksLists[0];
            for (int i = 1; i < estimateWorksLists.Count; i++)
            {
                var tempEstimateWorks = estimateWorksConnected;
                var nextEstimateWorks = estimateWorksLists[i];

                var insertedOneByOne = tempEstimateWorks.Count > nextEstimateWorks.Count
                    ? InsertOneByOne(tempEstimateWorks, nextEstimateWorks)
                    : InsertOneByOne(nextEstimateWorks, tempEstimateWorks);

                estimateWorksConnected = InterconnectEstimateWorks(insertedOneByOne);
            }

            return estimateWorksConnected;
        }

        private List<EstimateWork> InterconnectEstimateWorks(List<EstimateWork> estimateWorks)
        {
            var estimateWorksConnected = new List<EstimateWork>();

            foreach (var estimateWork in estimateWorks)
            {
                var idenitcalWorks = estimateWorks.FindAll(x => x.WorkName == estimateWork.WorkName);

                if (!estimateWorksConnected.Exists(x => x.WorkName == estimateWork.WorkName))
                {
                    if (idenitcalWorks.Count == 1)
                    {
                        estimateWorksConnected.Add(estimateWork);
                        continue;
                    }

                    var connectedEstimateWork = ConnectEstimateWork(idenitcalWorks[0], idenitcalWorks[1]);
                    estimateWorksConnected.Add(connectedEstimateWork);
                }
            }

            return estimateWorksConnected;
        }

        private EstimateWork ConnectEstimateWork(EstimateWork estimateWork1, EstimateWork estimateWork2)
        {
            return new EstimateWork(estimateWork1.WorkName,
                    estimateWork1.EquipmentCost + estimateWork2.EquipmentCost,
                    estimateWork1.OtherProductsCost + estimateWork2.OtherProductsCost,
                    estimateWork1.TotalCost + estimateWork2.TotalCost, estimateWork1.Chapter, estimateWork1.Percentages);
        }

        private List<EstimateWork> InsertOneByOne(List<EstimateWork> biggerOne, List<EstimateWork> smallerOne)
        {
            var instertedOneByOne = new List<EstimateWork>();

            for (int i = 0; i < biggerOne.Count; i++)
            {
                instertedOneByOne.Add(biggerOne[i]);

                if (i < smallerOne.Count)
                {
                    instertedOneByOne.Add(smallerOne[i]);
                }
            }

            return instertedOneByOne;
        }
    }
}
