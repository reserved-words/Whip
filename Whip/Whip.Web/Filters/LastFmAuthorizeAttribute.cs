using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Whip.LastFm;
using Whip.Services.Interfaces;

namespace Whip.Web.Filters
{
    public class LastFmAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly Lazy<ILastFmApiClientService> _lastFmApiClientService = new Lazy<ILastFmApiClientService>(
            () => DependencyResolver.Current.GetService<ILastFmApiClientService>()
        );

        private readonly Lazy<IErrorLoggingService> _logger = new Lazy<IErrorLoggingService>(
            () => DependencyResolver.Current.GetService<IErrorLoggingService>()
        );

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var lastFmUsername = ConfigurationManager.AppSettings["LastFmUsername"];
            var sessionKey = GetSessionKeyCookie(filterContext);
            if (_lastFmApiClientService.Value.AuthorizedApiClient == null)
            {
                SetLastFmClients(sessionKey, lastFmUsername);
                SetSessionKeyCookie(filterContext);
            }
        }

        private void SetLastFmClients(string sessionKey, string lastFmUsername)
        {
            var task = Task.Run(async () => { await _lastFmApiClientService.Value.SetClients(lastFmUsername, sessionKey); })
                    .ContinueWith(t => {
                        foreach (var ex in t.Exception.InnerExceptions)
                        {
                            _logger.Value.Log(ex);
                        }
                    }, TaskContinuationOptions.OnlyOnFaulted);
            task.Wait();
        }

        private void SetSessionKeyCookie(ActionExecutingContext filterContext)
        {
            var sessionKey = _lastFmApiClientService.Value.AuthorizedApiClient.SessionKey;
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