using AutoMapper;
using BGTG.POS.Entities;
using BGTG.POS.Web.ViewModels.LogViewModels;
using Calabonga.AspNetCore.Controllers.Handlers;
using Calabonga.AspNetCore.Controllers.Queries;
using Calabonga.Microservices.Core.QueryParams;
using Calabonga.UnitOfWork;

namespace BGTG.POS.Web.Mediator.LogsReadonly
{
    /// <summary>
    /// Request for paged list of Logs
    /// </summary>
    public class LogGetPagedRequest : GetPagedQuery<LogViewModel, PagedListQueryParams>
    {
        public LogGetPagedRequest(PagedListQueryParams queryParams) : base(queryParams)
        {
        }
    }

    /// <summary>
    /// Request for paged list of Logs
    /// </summary>
    public class LogGetPagedRequestHandler : GetPagedHandlerBase<LogGetPagedRequest, Log, PagedListQueryParams, LogViewModel>
    {
        public LogGetPagedRequestHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
    }
}
