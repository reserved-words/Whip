using System.Linq;
using System.Web.Mvc;
using Whip.AzureSync;
using Whip.Services;
using Whip.Services.Interfaces;
using Whip.Web.Models;
using Whip.XmlDataAccess;

namespace Whip.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICloudService _cloudService;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ITrackRepository _trackRepository;
        private readonly ITrackCriteriaService _trackCriteriaService;

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

            var tracks = trackSearchService.GetTracks(playlist.FilterType, playlist.FilterValues)
                .Select(t => new TrackViewModel(t, _cloudService.GetTrackUrl(t), _cloudService.GetArtworkUrl(t.Disc.Album)))
                .ToList();

            var model = new PlaylistViewModel
            {
                Id = playlist.Id,
                Title = playlist.Title,
                Tracks = tracks
            };

            return View(model);
        }

        public ActionResult OrderedPlaylist(int id)
        {
            var playlist = _playlistRepository.GetOrderedPlaylist(id);

            var library = _trackRepository.GetLibrary();

            var trackSearchService = new TrackSearchService(library, _trackCriteriaService);

            var tracks = trackSearchService.GetTracks(playlist.Tracks)
                .Select(t => new TrackViewModel(t, _cloudService.GetTrackUrl(t), _cloudService.GetArtworkUrl(t.Disc.Album)))
                .ToList();

            var model = new PlaylistViewModel
            {
                Id = playlist.Id,
                Title = playlist.Title,
                Tracks = tracks
            };

            return View(model);
        }

        public ActionResult CriteriaPlaylist(int id)
        {
            var playlist = _playlistRepository.GetCriteriaPlaylist(id);

            var library = _trackRepository.GetLibrary();

            var trackSearchService = new TrackSearchService(library, _trackCriteriaService);

            var tracks = trackSearchService.GetTracks(playlist)
                .Select(t => new TrackViewModel(t, _cloudService.GetTrackUrl(t), _cloudService.GetArtworkUrl(t.Disc.Album)))
                .ToList();

            var model = new PlaylistViewModel
            {
                Id = playlist.Id,
                Title = playlist.Title,
                Tracks = tracks
            };

            return View(model);
        }
    }
}