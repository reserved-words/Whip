using System.Collections.Generic;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.Services.Singletons
{
    public class PlayRequestHandler : IPlayRequestHandler
    {
        private const string LibraryPlaylistName = "Library";

        private readonly IPlaylist _playlist;
        private readonly ITrackFilterService _trackFilterService;
        private readonly IUserSettings _userSettings;

        private bool _shuffleStatusChangedByPlayRequest;

        public PlayRequestHandler(IPlaylist playlist, ITrackFilterService trackFilterService, IUserSettings userSettings)
        {
            _playlist = playlist;
            _trackFilterService = trackFilterService;
            _userSettings = userSettings;

            _userSettings.ShufflingStatusChanged += OnShufflingStatusChanged;
        }

        public void MoveToTrack(Track track)
        {
            _playlist.Set(_playlist.PlaylistName, _playlist.Tracks, track, _userSettings.ShuffleOn);
        }

        public void PlayAll(SortType? sortType, Track firstTrack = null)
        {
            UpdatePlaylist(sortType, LibraryPlaylistName, _trackFilterService.GetAll(), firstTrack);
        }

        public void PlayGrouping(string grouping, SortType? sortType, Track firstTrack = null)
        {
            UpdatePlaylist(sortType, grouping, _trackFilterService.GetTracksByGrouping(grouping), firstTrack);
        }

        public void PlayAlbum(Album album, SortType? sortType, Track firstTrack = null)
        {
            UpdatePlaylist(sortType, album.ToString(), _trackFilterService.GetTracksFromAlbum(album), firstTrack);
        }

        public void PlayArtist(Artist artist, SortType? sortType, Track firstTrack = null)
        {
            UpdatePlaylist(sortType, artist.Name, _trackFilterService.GetTracksByArtist(artist), firstTrack);
        }

        public void PlayCriteriaPlaylist(string playlistName, List<Track> tracks, Track firstTrack = null)
        {
            UpdatePlaylist(SortType.Random, playlistName, tracks, firstTrack);
        }

        public void PlayOrderedPlaylist(string playlistName, List<Track> tracks, Track firstTrack = null)
        {
            UpdatePlaylist(SortType.Ordered, playlistName, tracks, firstTrack, true);
        }

        private void UpdatePlaylist(SortType? sortType, string playlistName, List<Track> tracks, Track startAt,
            bool isAnOrderedPlaylist = false)
        {
            UpdateSortType(sortType);
            _playlist.Set(playlistName, tracks, startAt, _userSettings.ShuffleOn, isAnOrderedPlaylist);
        }

        private void UpdateSortType(SortType? sortType)
        {
            if (!sortType.HasValue)
                return;

            var shuffleOn = sortType.Value == SortType.Random;
            if (shuffleOn == _userSettings.ShuffleOn)
                return;

            _shuffleStatusChangedByPlayRequest = true;
            _userSettings.ShuffleOn = shuffleOn;
            _userSettings.SaveAsync();
        }

        private void OnShufflingStatusChanged()
        {
            if (_shuffleStatusChangedByPlayRequest)
            {
                _shuffleStatusChangedByPlayRequest = false;
                return;
            }

            _playlist.Reorder(_userSettings.ShuffleOn);
        }
    }
}
