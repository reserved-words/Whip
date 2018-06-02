using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Whip.LastFm;

namespace Whip.Web.Filters
{
    public class RequireHttpsRemotelyAttribute : RequireHttpsAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.IsLocal)
                return;

            base.OnAuthorization(filterContext);
        }
    }
}