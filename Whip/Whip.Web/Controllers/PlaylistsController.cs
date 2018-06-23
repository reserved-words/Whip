using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using Whip.Common.Enums;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.Web.Interfaces;
using Whip.Web.Models;

namespace Whip.Web.Controllers
{
    public class PlaylistsController : BaseController
    {
        private readonly IPlaylistService _playlistsService;

        public PlaylistsController(IPlaylist playlist, IErrorLoggingService logger, IPlaylistService playlistsService,
            ICloudService cloudService, IPlaySettings playSettings)
            : base(cloudService, playlist, logger, playSettings)
        {
            _playlistsService = playlistsService;
        }

        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.ServerAndClient)]
        public ActionResult Index()
        {
            var playlists = _playlistsService.GetAll();

            var model = new PlaylistsViewModel();

            foreach (var playlist in playlists.CriteriaPlaylists)
            {
                model.CriteriaPlaylists.Add(new PlaylistViewModel(playlist.Title, Url.Action(nameof(PlayCriteriaPlaylist), new { id = playlist.Id }), Url.Action(nameof(CriteriaPlaylist), new { id = playlist.Id })));
            }

            foreach (var playlist in playlists.OrderedPlaylists)
            {
                model.OrderedPlaylists.Add(new PlaylistViewModel(playlist.Title, Url.Action(nameof(PlayOrderedPlaylist), new { id = playlist.Id }), Url.Action(nameof(OrderedPlaylist), new { id = playlist.Id })));
            }

            foreach (var playlist in playlists.FavouriteQuickPlaylists)
            {
                model.StandardPlaylists.Add(new PlaylistViewModel(playlist.Title, Url.Action(nameof(PlayStandardPlaylist), new { id = playlist.Id }), Url.Action(nameof(StandardPlaylist), new { id = playlist.Id })));
            }

            return PartialView("_Index", model);
        }

        public ActionResult Favourites()
        {
            var playlists = _playlistsService.GetFavourites();
            var model = new List<PlaylistViewModel>();

            foreach (var playlist in playlists)
            {
                var playAction = playlist.Type == PlaylistType.Quick
                    ? nameof(PlayStandardPlaylist)
                    : playlist.Type == PlaylistType.Criteria
                        ? nameof(PlayCriteriaPlaylist)
                        : nameof(PlayOrderedPlaylist);

                var infoAction = playlist.Type == PlaylistType.Quick
                    ? nameof(StandardPlaylist)
                    : playlist.Type == PlaylistType.Criteria
                        ? nameof(CriteriaPlaylist)
                        : nameof(OrderedPlaylist);

                model.Add(new PlaylistViewModel(playlist.Title, Url.Action(playAction, new { id = playlist.Id }), Url.Action(infoAction, new { id = playlist.Id })));
            }

            return PartialView("_FavouritePlaylists", model);
        }

        public ActionResult StandardPlaylist(int id, int page = 1)
        {
            var playlist = _playlistsService.GetQuickPlaylist(id);
            return GetPlaylist(
                playlist.Item1.Title, 
                Url.Action(nameof(PlayStandardPlaylist), new { id }), 
                playlist.Item2,
                p => Url.Action(nameof(StandardPlaylist), new { id = id, page = p }),
                page);
        }

        public JsonResult PlayStandardPlaylist(int id)
        {
            var playlist = _playlistsService.GetQuickPlaylist(id);
            return Play(playlist.Item1.Title, playlist.Item2);
        }

        public ActionResult OrderedPlaylist(int id, int page = 1)
        {
            var playlist = _playlistsService.GetOrderedPlaylist(id);
            return GetPlaylist(
                playlist.Item1.Title, 
                Url.Action("PlayOrderedPlaylist", new { id }), 
                playlist.Item2,
                p => Url.Action(nameof(OrderedPlaylist), new { id = id, page = p }),
                page);
        }

        public JsonResult PlayOrderedPlaylist(int id)
        {
            var playlist = _playlistsService.GetOrderedPlaylist(id);
            return Play(playlist.Item1.Title, playlist.Item2, null, false, true);
        }

        public ActionResult CriteriaPlaylist(int id, int page = 1)
        {
            var playlist = _playlistsService.GetCriteriaPlaylist(id);
            return GetPlaylist(
                playlist.Item1.Title, 
                Url.Action("PlayCriteriaPlaylist", new { id }), 
                playlist.Item2,
                p => Url.Action(nameof(CriteriaPlaylist), new { id = id, page = p }),
                page);
        }

        private ActionResult GetPlaylist(string title, string playUrl, List<Track> tracks, Func<int, string> getPageUrl, int page)
        {
            var trackList = new TrackListViewModel(tracks, page, 30, GetViewModel, getPageUrl);

            var model = new PlayViewModel(title, trackList, playUrl);

            return PartialView("_Playlist", model);
        }

        public JsonResult PlayCriteriaPlaylist(int id)
        {
            var playlist = _playlistsService.GetCriteriaPlaylist(id);
            return Play(playlist.Item1.Title, playlist.Item2);
        }
    }
}