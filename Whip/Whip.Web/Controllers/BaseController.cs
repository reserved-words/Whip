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
        
        private readonly ICloudService _cloudService;
        
        public BaseController(ITrackRepository trackRepository, ICloudService cloudService, 
            IPlaylist playlist, Library library)
        {
            Playlist = playlist;
            _cloudService = cloudService;
            
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
    }
}