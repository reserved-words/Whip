using System.Linq;
using System.Web.Mvc;
using Whip.AzureSync;
using Whip.Common.TrackSorters;
using Whip.Services;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.Services.Singletons;
using Whip.Web.Models;
using Whip.XmlDataAccess;

namespace Whip.Web.Controllers
{
    public class HomeController : Controller
    {
        private static readonly IPlaylist Playlist;

        private readonly ICloudService _cloudService;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ITrackRepository _trackRepository;
        private readonly ITrackCriteriaService _trackCriteriaService;

        static HomeController()
        {
            var trackQueue = new TrackQueue();
            var defaultTrackSorter = new DefaultTrackSorter();
            var randomTrackSorter = new RandomTrackSorter();
            Playlist = new Playlist(trackQueue, defaultTrackSorter, randomTrackSorter);
        }

        public HomeController()
        {
            _cloudService = new AzureService();
            _trackCriteriaService = new TrackCriteriaService();

            var trackXmlParser = new TrackXmlParser();
            var playlistXmlProvider = new Services.PlaylistXmlProvider(_cloudService);
            var trackXmlProvider = new Services.TrackXmlProvider(_cloudService);

            _playlistRepository = new PlaylistRepository(_trackCriteriaService, playlistXmlProvider);
            _trackRepository = new TrackRepository(trackXmlParser, trackXmlProvider);
        }

        public ActionResult Index()
        {
            var model = new PlaylistsViewModel();

            var playlists = _playlistRepository.GetPlaylists();

            foreach (var playlist in playlists.CriteriaPlaylists)
            {
                model.CriteriaPlaylists.Add(new PlaylistViewModel { Id = playlist.Id, Title = playlist.Title });
            }

            foreach (var playlist in playlists.OrderedPlaylists)
            {
                model.OrderedPlaylists.Add(new PlaylistViewModel { Id = playlist.Id, Title = playlist.Title });
            }

            foreach (var playlist in playlists.FavouriteQuickPlaylists)
            {
                model.StandardPlaylists.Add(new PlaylistViewModel { Id = playlist.Id, Title = playlist.Title });
            }

            return View(model);
        }

        public ActionResult StandardPlaylist(int id)
        {
            var playlist = _playlistRepository.GetQuickPlaylist(id);

            var library = _trackRepository.GetLibrary();

            var trackSearchService = new TrackSearchService(library, _trackCriteriaService);

            var tracks = trackSearchService.GetTracks(playlist.FilterType, playlist.FilterValues);

            Playlist.Set(playlist.Title, tracks, null, true);

            var model = new PlaylistViewModel
            {
                Id = playlist.Id,
                Title = playlist.Title,
                Tracks = tracks
                    .Select(t => new TrackViewModel(t, _cloudService.GetTrackUrl(t), _cloudService.GetArtworkUrl(t.Disc.Album)))
                    .ToList()
            };

            return View(model);
        }

        public ActionResult OrderedPlaylist(int id)
        {
            var playlist = _playlistRepository.GetOrderedPlaylist(id);

            var library = _trackRepository.GetLibrary();

            var trackSearchService = new TrackSearchService(library, _trackCriteriaService);

            var tracks = trackSearchService.GetTracks(playlist.Tracks);

            Playlist.Set(playlist.Title, tracks, null, true);

            var model = new PlaylistViewModel
            {
                Id = playlist.Id,
                Title = playlist.Title,
                Tracks = tracks
                    .Select(t => new TrackViewModel(t, _cloudService.GetTrackUrl(t), _cloudService.GetArtworkUrl(t.Disc.Album)))
                    .ToList()
            };

            return View(model);
        }

        public ActionResult CriteriaPlaylist(int id)
        {
            var playlist = _playlistRepository.GetCriteriaPlaylist(id);

            var library = _trackRepository.GetLibrary();

            var trackSearchService = new TrackSearchService(library, _trackCriteriaService);

            var tracks = trackSearchService.GetTracks(playlist);

            Playlist.Set(playlist.Title, tracks, null, true);

            var model = new PlaylistViewModel
            {
                Id = playlist.Id,
                Title = playlist.Title,
                Tracks = tracks
                    .Select(t => new TrackViewModel(t, _cloudService.GetTrackUrl(t), _cloudService.GetArtworkUrl(t.Disc.Album)))
                    .ToList()
            };

            return View(model);
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
            Playlist.MovePrevious();
            return GetCurrentTrack();
        }
    }
}