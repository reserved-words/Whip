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
            if (filterContext.HttpContext.Request.IsAjaxRequest())
                return;

            var lastFmUsername = ConfigurationManager.AppSettings["LastFmUsername"];
            var sessionKey = GetSessionKeyCookie(filterContext);
            if (_lastFmApiClientService.Value.UserApiClient == null)
            {
                SetLastFmClients(sessionKey, lastFmUsername);
            }
        }

        private void SetLastFmClients(string sessionKey, string lastFmUsername)
        {
            var task = Task.Run(async () =>
            {
                await _lastFmApiClientService.Value.SetClients(lastFmUsername, sessionKey);
            })
            .ContinueWith(t =>
            {
                foreach (var ex in t.Exception.InnerExceptions)
                {
                    _logger.Value.Log(ex);
                }
            }, TaskContinuationOptions.OnlyOnFaulted)
            .ContinueWith(t =>
            {
                // This is just here as a workaround to avoid an exception being thrown
                // Ideally need to work out why the task keeps getting cancelled
            }, TaskContinuationOptions.OnlyOnCanceled);
            task.Wait();
        }

        private string GetSessionKeyCookie(ActionExecutingContext filterContext)
        {
            return filterContext.HttpContext.Request.Cookies["SK"]?.Value;
        }
    }
}