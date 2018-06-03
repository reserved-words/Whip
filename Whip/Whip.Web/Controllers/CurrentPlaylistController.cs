using System.Web.Mvc;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.Web.Controllers
{
    public class CurrentPlaylistController : BaseController
    {
        private readonly ICloudService _cloudService;

        public CurrentPlaylistController(ICloudService cloudService, IPlaylist playlist, IErrorLoggingService logger)
            : base(cloudService, playlist, logger)
        {
            _cloudService = cloudService;
        }

        public ActionResult Index()
        {
            return PartialView("_Index");
        }

        [HttpPost]
        public new JsonResult GetCurrentTrack()
        {
            return base.GetCurrentTrack();
        }

        [HttpPost]
        public JsonResult GetNextTrack()
        {
            Playlist.MoveNext();
            return GetCurrentTrack();
        }

        [HttpPost]
        public JsonResult GetPreviousTrack()
        {
            Playlist.MoveNext();
            return GetCurrentTrack();
        }
    }
}