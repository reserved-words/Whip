using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.Web.Models;

namespace Whip.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly Library Library;
        protected readonly IPlaylist Playlist;

        private readonly ITrackRepository _trackRepository;
        private readonly ICloudService _cloudService;
        
        public BaseController(ITrackRepository trackRepository, ICloudService cloudService, 
            IPlaylist playlist, Library library)
        {
            Playlist = playlist;
            _cloudService = cloudService;
            _trackRepository = trackRepository;

            if (Library == null)
            {
                Library = _trackRepository.GetLibrary();
            }
        }

        protected ActionResult Play(string title, List<Track> tracks, Track firstTrack = null, bool shuffle = true)
        {
            Playlist.Set(title, tracks, firstTrack, shuffle);
            return View("Playlist", GetViewModel(title, tracks));
        }
        
        private PlayViewModel GetViewModel(string title, List<Track> tracks)
        {
            var track = Playlist.CurrentTrack;

            return new PlayViewModel
            {
                Title = title,
                Tracks = tracks
                    .Select(t => new TrackViewModel(t, _cloudService.GetTrackUrl(t), _cloudService.GetArtworkUrl(t.Disc.Album)))
                    .ToList(),
                FirstTrack = new TrackViewModel(track, _cloudService.GetTrackUrl(track), _cloudService.GetArtworkUrl(track.Disc.Album))
            };
        }
    }
}