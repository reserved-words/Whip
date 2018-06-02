using System.Web.Mvc;
using Whip.Web.Filters;

namespace Whip.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LastFmAuthorizeAttribute());
            filters.Add(new RequireHttpsRemotelyAttribute());
        }
    }
}
