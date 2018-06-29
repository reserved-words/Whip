using System;
using System.Collections.Generic;
using Whip.Common.Model;
using Whip.Services;
using Whip.Services.Interfaces;
using Whip.Web.Interfaces;

namespace Whip.Web.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ITrackCriteriaService _trackCriteriaService;
        private readonly Interfaces.ILibraryService _libraryService;

        public PlaylistService(ITrackCriteriaService trackCriteriaService, IPlaylistRepository playlistRepository,
            Interfaces.ILibraryService libraryService)
        {
            _trackCriteriaService = trackCriteriaService;
            _playlistRepository = playlistRepository;
            _libraryService = libraryService;
        }

        public AllPlaylists GetAll()
        {
            return _playlistRepository.GetPlaylists();
        }

        public Tuple<Playlist,List<Track>> GetCriteriaPlaylist(int id)
        {
            var playlist = _playlistRepository.GetCriteriaPlaylist(id);
            var trackSearchService = GetTrackSearchService();
            var tracks = trackSearchService.GetTracks(playlist);
            return new Tuple<Playlist, List<Track>>(playlist, tracks);
        }

        public Tuple<Playlist, List<Track>> GetOrderedPlaylist(int id)
        {
            var playlist = _playlistRepository.GetOrderedPlaylist(id);
            var trackSearchService = GetTrackSearchService();
            var tracks = trackSearchService.GetTracks(playlist.Tracks);
            return new Tuple<Playlist, List<Track>>(playlist, tracks);
        }

        public Tuple<Playlist, List<Track>> GetQuickPlaylist(int id)
        {
            var playlist = _playlistRepository.GetQuickPlaylist(id);
            var trackSearchService = GetTrackSearchService();
            var tracks = trackSearchService.GetTracks(playlist.FilterType, playlist.FilterValues);
            return new Tuple<Playlist, List<Track>>(playlist, tracks);
        }

        public List<Playlist> GetFavourites()
        {
            return _playlistRepository.GetFavouritePlaylists();
        }

        private ITrackSearchService GetTrackSearchService()
        {
            return new TrackSearchService(_libraryService.Library, _trackCriteriaService);
        }
    }
}