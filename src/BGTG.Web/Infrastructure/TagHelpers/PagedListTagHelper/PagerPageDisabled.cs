using BGTG.Web.Infrastructure.TagHelpers.PagedListTagHelper.Base;

namespace BGTG.Web.Infrastructure.TagHelpers.PagedListTagHelper
{
    public class PagerPageDisabled : PagerPageBase
    {
        public PagerPageDisabled(string title, int value) : base(title, value, false, true)
        {
        }
    }
}