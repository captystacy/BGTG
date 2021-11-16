using POSCore.EstimateLogic.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace POSCore.EstimateLogic
{
    public class EstimateConnector : IEstimateConnector
    {
        public Estimate Connect(Estimate estimateVatFree, Estimate estimateVat)
        {
            var estimateWorksVatFree = estimateVatFree.EstimateWorks.ToList();
            var estimateWorksVat = estimateVat.EstimateWorks.ToList();

            var insertedOneByOne = estimateWorksVatFree.Count > estimateWorksVat.Count 
                ? InsertOneByOne(estimateWorksVatFree, estimateWorksVat)
                : InsertOneByOne(estimateWorksVat, estimateWorksVatFree);

            var estimateWorksConnected = ConnectEstimateWorks(insertedOneByOne);

            return new Estimate(estimateWorksConnected);
        }

        private List<EstimateWork> ConnectEstimateWorks(List<EstimateWork> estimateWorks)
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
                    estimateWork1.TotalCost + estimateWork2.TotalCost);
        }

        private List<EstimateWork> InsertOneByOne(IList<EstimateWork> insertedInto, IList<EstimateWork> insertedFrom)
        {
            var instertedOneByOne = new List<EstimateWork>(insertedInto);

            var j = 0;
            for (int i = 0; i < insertedFrom.Count; i++)
            {
                instertedOneByOne.Insert(j, insertedFrom[i]);
                if (j + 1 + i < instertedOneByOne.Count)
                {
                    j += 2 + i;
                }
            }

            return instertedOneByOne;
        }
    }
}
