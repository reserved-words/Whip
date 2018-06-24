using System.Web.Mvc;
using System.Web.UI;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.Web.Models;
using Whip.Web.Interfaces;

namespace Whip.Web.Controllers
{
    public class CurrentPlaylistController : BaseController
    {
        private readonly IAppSettings _appSettings;

        public CurrentPlaylistController(ICloudService cloudService, IPlaylist playlist, IErrorLoggingService logger, IPlaySettings playSettings,
            IAppSettings appSettings)
            : base(cloudService, playlist, logger, playSettings)
        {
            _appSettings = appSettings;
        }

        [OutputCache(Duration = 1800, VaryByParam = "page", Location = OutputCacheLocation.ServerAndClient)]
        public ActionResult Index(int page = 1)
        {
            var tracklist = new TrackListViewModel(Playlist.Tracks, page, _appSettings.TracksPerPage, GetViewModel, 
                p => Url.Action(nameof(Index), new { page = p }), Url.Action(nameof(Restart)));

            var model = new PlayViewModel(Playlist.PlaylistName, tracklist);

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

        [HttpPost]
        public JsonResult Restart()
        {
            return Play();
        }
    }
}