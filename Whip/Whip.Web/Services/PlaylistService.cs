using System;
using System.Collections.Generic;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Services;
using Whip.Services.Interfaces;
using Whip.Web.Interfaces;

namespace Whip.Web.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ITrackRepository _trackRepository;
        private readonly ITrackCriteriaService _trackCriteriaService;

        public PlaylistService(ITrackCriteriaService trackCriteriaService,
            IPlaylistRepository playlistRepository, ITrackRepository trackRepository)
        {
            _trackCriteriaService = trackCriteriaService;
            _playlistRepository = playlistRepository;
            _trackRepository = trackRepository;
        }

        public AllPlaylists GetAll()
        {
            return _playlistRepository.GetPlaylists();
        }

        public Tuple<Playlist,List<Track>> GetCriteriaPlaylist(int id, Library library)
        {
            var playlist = _playlistRepository.GetCriteriaPlaylist(id);
            var trackSearchService = GetTrackSearchService(library);
            var tracks = trackSearchService.GetTracks(playlist);
            return new Tuple<Playlist, List<Track>>(playlist, tracks);
        }

        public Tuple<Playlist, List<Track>> GetOrderedPlaylist(int id, Library library)
        {
            var playlist = _playlistRepository.GetOrderedPlaylist(id);
            var trackSearchService = GetTrackSearchService(library);
            var tracks = trackSearchService.GetTracks(playlist.Tracks);
            return new Tuple<Playlist, List<Track>>(playlist, tracks);
        }

        public Tuple<Playlist, List<Track>> GetQuickPlaylist(int id, Library library)
        {
            var playlist = _playlistRepository.GetQuickPlaylist(id);
            var trackSearchService = GetTrackSearchService(library);
            var tracks = trackSearchService.GetTracks(playlist.FilterType, playlist.FilterValues);
            return new Tuple<Playlist, List<Track>>(playlist, tracks);
        }

        private ITrackSearchService GetTrackSearchService(Library library)
        {
            return new TrackSearchService(library, _trackCriteriaService);
        }
    }
}