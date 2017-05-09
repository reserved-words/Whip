using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using Whip.Common;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.MessageHandlers
{
    public class PlayRequestHandler : IStartable
    {
        private const string LibraryPlaylistName = "Library";

        private readonly IMessenger _messenger;
        private readonly IPlaylist _playlist;
        private readonly ITrackFilterService _trackFilterService;
        private readonly IUserSettings _userSettings;

        private bool _shuffleStatusChangedByPlayRequest;

        public PlayRequestHandler(IMessenger messenger, IPlaylist playlist, ITrackFilterService trackFilterService,
            IUserSettings userSettings)
        {
            _messenger = messenger;
            _playlist = playlist;
            _trackFilterService = trackFilterService;
            _userSettings = userSettings;
        }

        public void Start()
        {
            _userSettings.ShufflingStatusChanged += OnShufflingStatusChanged;
            _messenger.Register<PlayArtistMessage>(this, OnPlayArtist);
            _messenger.Register<PlayAlbumMessage>(this, OnPlayAlbum);
            _messenger.Register<PlayGroupingMessage>(this, OnPlayGrouping);
            _messenger.Register<PlayAllMessage>(this, OnPlayAll);
            _messenger.Register<PlayPlaylistMessage>(this, OnPlayPlaylist);
            _messenger.Register<MoveToTrackMessage>(this, OnMoveToTrack);
        }

        public void Stop()
        {
            _userSettings.ShufflingStatusChanged -= OnShufflingStatusChanged;
            _messenger.Unregister<PlayArtistMessage>(this, OnPlayArtist);
            _messenger.Unregister<PlayAlbumMessage>(this, OnPlayAlbum);
            _messenger.Unregister<PlayGroupingMessage>(this, OnPlayGrouping);
            _messenger.Unregister<PlayAllMessage>(this, OnPlayAll);
            _messenger.Unregister<PlayPlaylistMessage>(this, OnPlayPlaylist);
            _messenger.Unregister<MoveToTrackMessage>(this, OnMoveToTrack);
        }

        private void OnMoveToTrack(MoveToTrackMessage message)
        {
            _playlist.Set(_playlist.PlaylistName, _playlist.Tracks, message.Track);
        }

        private void OnPlayAll(PlayAllMessage message)
        {
            ProcessMessage(message, LibraryPlaylistName, _trackFilterService.GetAll(), message.StartAt);
        }

        private void OnPlayGrouping(PlayGroupingMessage message)
        {
            ProcessMessage(message, message.Grouping, _trackFilterService.GetTracksByGrouping(message.Grouping), message.StartAt);
        }

        private void OnPlayAlbum(PlayAlbumMessage message)
        {
            ProcessMessage(message, message.Album.ToString(), _trackFilterService.GetTracksFromAlbum(message.Album), message.StartAt);
        }

        private void OnPlayArtist(PlayArtistMessage message)
        {
            ProcessMessage(message, message.Artist.Name, _trackFilterService.GetTracksByArtist(message.Artist), message.StartAt);
        }
        
        private void OnPlayPlaylist(PlayPlaylistMessage message)
        {
            ProcessMessage(message, message.PlaylistName,message.Tracks, message.StartAt);
        }

        private void ProcessMessage(PlayMessage message, string playlistName, List<Track> tracks, Track startAt)
        {
            UpdateSortType(message);
            _playlist.Set(playlistName, tracks, startAt);
        }

        private void UpdateSortType(PlayMessage message)
        {
            if (!message.SortType.HasValue)
                return;

            var shuffleOn = message.SortType.Value == SortType.Random;
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

            _playlist.Reorder();
        }
    }
}
