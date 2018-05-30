using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Whip.LastFm;

namespace Whip.Web.Filters
{
    public class LastFmAuthorizeAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var lastFmClientService = DependencyResolver.Current.GetService<ILastFmApiClientService>();
            if (lastFmClientService.AuthorizedApiClient == null)
            {
                lastFmClientService.SetClients(ConfigurationManager.AppSettings["LastFmUsername"], null).ConfigureAwait(false);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}