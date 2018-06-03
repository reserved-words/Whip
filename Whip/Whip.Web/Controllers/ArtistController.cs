using System.Web.Mvc;

namespace Whip.Web.Controllers
{
    public class ArtistController : Controller
    {
        public ActionResult Index()
        {
            return PartialView("_Index");
        }
    }
}