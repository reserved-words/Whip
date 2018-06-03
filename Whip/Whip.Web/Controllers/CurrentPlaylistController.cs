using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.Web.Models;

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

        [OutputCache(Duration = 1800, VaryByParam = "none", Location = OutputCacheLocation.Server)]
        public ActionResult Index()
        {
            var model = new PlayViewModel
            {
                Title = Playlist.PlaylistName,
                Tracks = Playlist.Tracks?
                    .Take(30) // TODO: Add paging
                    .Select(GetViewModel)
                    .ToList()
            };
            return PartialView("_Index", model);
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
            ClearTrackCache();
            return GetCurrentTrack();
        }

        [HttpPost]
        public JsonResult GetPreviousTrack()
        {
            Playlist.MovePrevious();
            ClearTrackCache();
            return GetCurrentTrack();
        }
    }
}