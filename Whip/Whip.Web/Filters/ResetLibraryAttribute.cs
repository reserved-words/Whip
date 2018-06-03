using System.Collections.Generic;
using System.Web.Mvc;
using Whip.Common.Model;
using Whip.Services.Interfaces.Singletons;
using Whip.Web.Interfaces;

namespace Whip.Web.Filters
{
    public class ResetLibraryAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                return;

            var libraryService = DependencyResolver.Current.GetService<ILibraryService>();
            libraryService.Reset();

            var playlist = DependencyResolver.Current.GetService<IPlaylist>();
            playlist.Set(null, new List<Track>(), null, true);

            filterContext.HttpContext.Response.RemoveOutputCacheItem("/Playlists");
            filterContext.HttpContext.Response.RemoveOutputCacheItem("/Playlists/Index");

            filterContext.HttpContext.Response.RemoveOutputCacheItem("/Library");
            filterContext.HttpContext.Response.RemoveOutputCacheItem("/Library/Index");
        }
    }
}