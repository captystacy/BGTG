using Calabonga.Microservices.Core.QueryParams;

namespace BGTG.Web.Infrastructure.QueryParams
{
    public class DefaultPagedListQueryParams : PagedListQueryParams
    {
        public DefaultPagedListQueryParams()
        {
            PageSize = 30;
            SortDirection = QueryParamsSortDirection.Descending;
        }

    }
}
