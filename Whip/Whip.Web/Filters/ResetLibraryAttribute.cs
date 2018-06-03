using System.Web.Mvc;
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
        }
    }
}