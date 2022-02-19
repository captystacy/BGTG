using System.Collections.Generic;

namespace BGTG.Web.Infrastructure.TagHelpers.PagedListTagHelper.Base
{
    /// <summary>
    /// Pager TagHelperService for Razor
    /// </summary>
    public interface IPagerTagHelperService
    {
        PagerContext GetPagerContext(int pageIndex, int pageSize, int totalPages, int pagesInGroup);

        List<PagerPageBase> GetPages(PagerContext pagerContext);
    }
}
