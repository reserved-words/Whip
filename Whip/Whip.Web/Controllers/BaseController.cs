using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.Web.Models;

namespace Whip.Web.Controllers
{
    public class BaseController : Controller
    {
        private static readonly IPlaylist Playlist;
        protected static Library Library;

        private readonly ITrackRepository _trackRepository;
        private readonly ICloudService _cloudService;
        
        static BaseController()
        {
            Playlist = DependencyResolver.Current.GetService<IPlaylist>();
        }

        public BaseController(ITrackRepository trackRepository, ICloudService cloudService)
        {
            _cloudService = cloudService;
            _trackRepository = trackRepository;

            if (Library == null)
            {
                Library = _trackRepository.GetLibrary();
            }
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

        protected ActionResult Play(string title, List<Track> tracks, Track firstTrack = null, bool shuffle = true)
        {
            Playlist.Set(title, tracks, firstTrack, shuffle);
            return View("Playlist", GetViewModel(title, tracks));
        }

        protected JsonResult MoveNext()
        {
            Playlist.MoveNext();
            return GetCurrentTrack();
        }

        protected JsonResult MovePrevious()
        {
            Playlist.MoveNext();
            return GetCurrentTrack();
        }

        protected Track CurrentTrack => Playlist.CurrentTrack;

        private PlayViewModel GetViewModel(string title, List<Track> tracks)
        {
            return new PlayViewModel
            {
                Title = title,
                Tracks = tracks
                    .Select(t => new TrackViewModel(t, _cloudService.GetTrackUrl(t), _cloudService.GetArtworkUrl(t.Disc.Album)))
                    .ToList()
            };
        }
    }
}