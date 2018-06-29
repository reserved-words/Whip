using System.Web.Mvc;

namespace Whip.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            if (Request.IsAjaxRequest())
            {
                return View("Index");
            }

            return PartialView("_Index");
        }

        public ActionResult Unauthorized()
        {
            ViewBag.Error = "Unauthorized";

            if (Request.IsAjaxRequest())
            {
                return View("Index");
            }

            return PartialView("_Index");
        }
    }
}