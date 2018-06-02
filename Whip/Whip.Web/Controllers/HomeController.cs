using System.Web.Mvc;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.Web.Interfaces;

namespace Whip.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IPlaylist playlist, Library library, IPlaylistService playlistsService, 
            ICloudService cloudService, ITrackRepository trackRepository)
            :base(trackRepository, cloudService, playlist, library)
        {
        }

        public ActionResult Index()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Index");
            }

            return View("_Index");
        }
    }
}