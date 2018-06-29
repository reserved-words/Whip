using System.Web.Mvc;

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