using System.Configuration;
using System.Threading.Tasks;
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
                var task = Task.Run(async () => { await lastFmClientService.SetClients(ConfigurationManager.AppSettings["LastFmUsername"], null); });
                task.Wait();
            }

            base.OnActionExecuting(filterContext);
        }
    }
}