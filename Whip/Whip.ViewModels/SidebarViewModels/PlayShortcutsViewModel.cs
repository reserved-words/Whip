using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Whip.Common;
using Whip.Common.Enums;
using Whip.Common.Model;
using Whip.Services.Interfaces.Singletons;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels
{
    public class PlayShortcutsViewModel : ViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly IPlayRequestHandler _playRequestHandler;
        private readonly IPlaylistsService _repository;
        private readonly ITrackSearchService _trackSearchService;

        public PlayShortcutsViewModel(IPlayRequestHandler playRequestHandler, IPlaylistsService repository, ITrackSearchService trackSearchService,
            IMessenger messenger)
        {
            _messenger = messenger;
            _repository = repository;
            _playRequestHandler = playRequestHandler;
            _trackSearchService = trackSearchService;

            PlayCommand = new RelayCommand<Playlist>(OnPlay);
            ShuffleLibraryCommand = new RelayCommand(OnShuffleLibrary);

            Playlists = new ObservableCollection<Playlist>();
        }

        public ObservableCollection<Playlist> Playlists { get; set; }

        public RelayCommand<Playlist> PlayCommand { get; }
        public RelayCommand ShuffleLibraryCommand { get; }

        public void LoadPlaylists()
        {
            var playlists = _repository.GetFavouritePlaylists();
            Playlists.Clear();
            playlists.OrderBy(p => p.Title).ToList().ForEach(Playlists.Add);
        }

        private void OnPlay(Playlist playlist)
        {
            switch (playlist.Type)
            {
                case PlaylistType.Criteria:
                    var criteriaPlaylist = _repository.GetCriteriaPlaylist(playlist.Id);
                    Play(criteriaPlaylist.Title, _trackSearchService.GetTracks(criteriaPlaylist), SortType.Random);
                    break;
                case PlaylistType.Ordered:
                    var orderedPlaylist = _repository.GetOrderedPlaylist(playlist.Id);
                    Play(orderedPlaylist.Title, _trackSearchService.GetTracks(orderedPlaylist.Tracks), SortType.Ordered);
                    break;
                case PlaylistType.Quick:
                    var quickPlaylist = _repository.GetQuickPlaylist(playlist.Id);
                    Play(quickPlaylist.Title, _trackSearchService.GetTracks(quickPlaylist.FilterType, quickPlaylist.FilterValues), SortType.Random);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(playlist.Type), playlist.Type, "Not a valid playlist type");
            }
        }

        private void Play(string title, List<Track> tracks, SortType sortType)
        {
            if (!tracks.Any())
            {
                _messenger.Send(new ShowDialogMessage(_messenger, MessageType.Info, "Playlist", "This playlist does not contain any tracks"));
                return;
            }

            _playRequestHandler.PlayPlaylist(title, tracks, sortType);
        }

        private void OnShuffleLibrary()
        {
            _playRequestHandler.PlayAll(SortType.Random);
        }
    }
}
