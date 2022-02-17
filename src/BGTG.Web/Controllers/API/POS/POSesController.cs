using System;
using System.Threading.Tasks;
using BGTG.Entities.POS;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Calabonga.UnitOfWork.Controllers.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BGTG.Web.Controllers.API.POS
{
    [Route("api/[controller]")]
    public class POSesController : UnitOfWorkController
    {
        public POSesController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        [HttpDelete("[action]/{id:guid}")]
        [ProducesResponseType(200)]
        public virtual async Task<ActionResult<OperationResult<Guid>>> DeleteItem(Guid id)
        {
            var repository = UnitOfWork.GetRepository<POSEntity>();
            var posEntity = await repository.FindAsync(id);

            if (posEntity == null)
            {
                return OperationResultError(id, new MicroserviceNotFoundException());
            }

            repository.Delete(posEntity);

            await UnitOfWork.SaveChangesAsync();

            if (!UnitOfWork.LastSaveChangesResult.IsOk)
            {
                return OperationResultError(id, new MicroserviceSaveChangesException());
            }

            return OperationResultSuccess(id);
        }
    }
}
