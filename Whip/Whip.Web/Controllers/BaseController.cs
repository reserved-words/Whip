using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.Web.Filters;
using Whip.Web.Models;

namespace Whip.Web.Controllers
{
    [GoogleAuthorize]
    public class BaseController : Controller
    {
        protected readonly IPlaylist Playlist;
        
        private readonly ICloudService _cloudService;
        private readonly IErrorLoggingService _logger;
        
        public BaseController(ICloudService cloudService, IPlaylist playlist, IErrorLoggingService logger)
        {
            Playlist = playlist;
            _cloudService = cloudService;
            _logger = logger;
        }

        protected TrackViewModel GetViewModel(Track track)
        {
            return track == null
                ? null
                : new TrackViewModel(track,
                    _cloudService.GetTrackUrl(track),
                    _cloudService.GetArtworkUrl(track.Disc.Album));
        }

        protected JsonResult Play(string title, List<Track> tracks, Track firstTrack = null, bool shuffle = true, bool doNotSort = false)
        {
            Playlist.Set(title, tracks, firstTrack, shuffle, doNotSort);
            ClearPlaylistCache();
            ClearTrackCache();
            return GetCurrentTrack();
        }

        protected ActionResult GetPlaylist(string title, List<Track> tracks, string playUrl)
        {
            var model = new PlayViewModel
            {
                Title = title,
                Tracks = tracks
                    .Select(GetViewModel)
                    .ToList(),
                PlayUrl = playUrl
            };

            return PartialView("_Playlist", model);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            _logger.Log(filterContext.Exception);
            filterContext.Result = RedirectToAction("Index", "Error");
        }

        protected JsonResult GetCurrentTrack()
        {
            var track = Playlist.CurrentTrack;

            return track == null
                ? new JsonResult { Data = new { } }
                : new JsonResult { Data = new {
                    Title = track.Title,
                    Artist = track.Artist.Name,
                    Album = track.Disc.Album.Title,
                    Year = track.Disc.Album.Year,
                    Url = _cloudService.GetTrackUrl(track),
                    ArtworkUrl = _cloudService.GetArtworkUrl(track.Disc.Album)
                }
            };
        }

        protected void ClearPlaylistCache()
        {
            Response.RemoveOutputCacheItem("/CurrentPlaylist");
            Response.RemoveOutputCacheItem("/CurrentPlaylist/Index");
        }

        protected void ClearTrackCache()
        {
            Response.RemoveOutputCacheItem("/CurrentTrack");
            Response.RemoveOutputCacheItem("/CurrentTrack/Index");
        }
    }
}