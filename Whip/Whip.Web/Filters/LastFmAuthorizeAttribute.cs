using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Whip.LastFm;

namespace Whip.Web.Filters
{
    public class LastFmAuthorizeAttribute : ActionFilterAttribute
    {
        private ILastFmApiClientService _lastFmApiClientService;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var lastFmUsername = ConfigurationManager.AppSettings["LastFmUsername"];
            var sessionKey = GetSessionKeyCookie(filterContext);

            _lastFmApiClientService = DependencyResolver.Current.GetService<ILastFmApiClientService>();
            if (_lastFmApiClientService.AuthorizedApiClient == null)
            {
                var task = Task.Run(async () => { await _lastFmApiClientService.SetClients(lastFmUsername, sessionKey); });
                task.Wait();
                SetSessionKeyCookie(filterContext);
            }
        }

        private void SetSessionKeyCookie(ActionExecutingContext filterContext)
        {
            var sessionKey = _lastFmApiClientService.AuthorizedApiClient.SessionKey;
            filterContext.HttpContext.Response.SetCookie(
                new HttpCookie("SK", sessionKey)
                {
                    Expires = DateTime.Now.AddDays(30),
                    HttpOnly = true,
                    Secure = true
                });
        }

        private string GetSessionKeyCookie(ActionExecutingContext filterContext)
        {
            return filterContext.HttpContext.Request.Cookies["SK"]?.Value;
        }
    }
}