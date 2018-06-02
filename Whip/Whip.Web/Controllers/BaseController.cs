using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.Web.Filters;
using Whip.Web.Models;

namespace Whip.Web.Controllers
{
    [LastFmAuthorize]
    public class BaseController : Controller
    {
        protected static Library Library;
        protected readonly IPlaylist Playlist;
        
        private readonly ICloudService _cloudService;
        private readonly IErrorLoggingService _logger;
        
        public BaseController(ITrackRepository trackRepository, ICloudService cloudService, 
            IPlaylist playlist, IErrorLoggingService logger)
        {
            Playlist = playlist;

            _cloudService = cloudService;
            _logger = logger;

            if (Library == null)
            {
                Library = trackRepository.GetLibrary();
            }
        }

        protected JsonResult Play(string title, List<Track> tracks, Track firstTrack = null, bool shuffle = true, bool doNotSort = false)
        {
            Playlist.Set(title, tracks, firstTrack, shuffle, doNotSort);
            var currentTrack = Playlist.CurrentTrack;
            return new JsonResult
            {
                Data = new
                {
                    Url = _cloudService.GetTrackUrl(currentTrack),
                    ArtworkUrl = _cloudService.GetArtworkUrl(currentTrack.Disc.Album),
                    Description = $"{currentTrack.Title} by {currentTrack.Artist.Name}"
                }
            };
        }

        protected ActionResult GetPlaylist(string title, List<Track> tracks, string playUrl)
        {
            var model = new PlayViewModel
            {
                Title = title,
                Tracks = tracks
                    .Select(t => new TrackViewModel(t, _cloudService.GetTrackUrl(t), _cloudService.GetArtworkUrl(t.Disc.Album)))
                    .ToList(),
                PlayUrl = playUrl
            };

            return PartialView("_Playlist", model);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            _logger.Log(filterContext.Exception);

            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = PartialView("Error");
            }
            else
            {
                filterContext.Result = View("Error");
            }
        }
    }
}