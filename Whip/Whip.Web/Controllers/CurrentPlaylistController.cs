using System.Web.Mvc;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.Web.Controllers
{
    public class CurrentPlaylistController : BaseController
    {
        private readonly ICloudService _cloudService;

        public CurrentPlaylistController(ICloudService cloudService, ITrackRepository trackRepository,
            IPlaylist playlist, IErrorLoggingService logger)
            : base(trackRepository, cloudService, playlist, logger)
        {
            _cloudService = cloudService;
        }

        [HttpPost]
        public JsonResult GetCurrentTrack()
        {
            var track = Playlist.CurrentTrack;

            return new JsonResult
            {
                Data = track == null
                    ? null
                    : new
                    {
                        Description = track.Title + " by " + track.Artist.Name,
                        Url = _cloudService.GetTrackUrl(track),
                        ArtworkUrl = _cloudService.GetArtworkUrl(track.Disc.Album)
                    }
            };
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