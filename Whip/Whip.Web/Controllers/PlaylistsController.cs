using System.Web.Mvc;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.Web.Interfaces;
using Whip.Web.Models;

namespace Whip.Web.Controllers
{
    public class PlaylistsController : BaseController
    {
        private readonly IPlaylistService _playlistsService;

        public PlaylistsController(IPlaylist playlist, Library library, IPlaylistService playlistsService,
            ICloudService cloudService, ITrackRepository trackRepository)
            : base(trackRepository, cloudService, playlist, library)
        {
            _playlistsService = playlistsService;
        }

        public ActionResult Index()
        {
            var playlists = _playlistsService.GetAll();

            var model = new PlaylistsViewModel();

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

            return PartialView("_Index", model);
        }

        public ActionResult StandardPlaylist(int id)
        {
            var playlist = _playlistsService.GetQuickPlaylist(id, Library);
            return GetPlaylist(playlist.Item1.Title, playlist.Item2, Url.Action("PlayStandardPlaylist", new { id }));
        }

        public JsonResult PlayStandardPlaylist(int id)
        {
            var playlist = _playlistsService.GetQuickPlaylist(id, Library);
            return Play(playlist.Item1.Title, playlist.Item2);
        }

        public ActionResult OrderedPlaylist(int id)
        {
            var playlist = _playlistsService.GetOrderedPlaylist(id, Library);
            return GetPlaylist(playlist.Item1.Title, playlist.Item2, Url.Action("PlayOrderedPlaylist", new { id }));
        }

        public JsonResult PlayOrderedPlaylist(int id)
        {
            var playlist = _playlistsService.GetOrderedPlaylist(id, Library);
            return Play(playlist.Item1.Title, playlist.Item2, null, false, true);
        }

        public ActionResult CriteriaPlaylist(int id)
        {
            var playlist = _playlistsService.GetCriteriaPlaylist(id, Library);
            return GetPlaylist(playlist.Item1.Title, playlist.Item2, Url.Action("PlayCriteriaPlaylist", new { id }));
        }

        public JsonResult PlayCriteriaPlaylist(int id)
        {
            var playlist = _playlistsService.GetCriteriaPlaylist(id, Library);
            return Play(playlist.Item1.Title, playlist.Item2);
        }
    }
}