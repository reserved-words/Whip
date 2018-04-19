using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces.Singletons;
using Whip.Services.Interfaces;

namespace Whip.ViewModels
{
    public class PlayShortcutsViewModel : ViewModelBase
    {
        private readonly IPlayRequestHandler _playRequestHandler;
        private readonly IPlaylistRepository _repository;
        private readonly ITrackSearchService _trackSearchService;

        private ObservableCollection<CriteriaPlaylist> _criteriaPlaylists;
        private ObservableCollection<OrderedPlaylist> _orderedPlaylists;

        public PlayShortcutsViewModel(IPlayRequestHandler playRequestHandler, IPlaylistRepository repository, ITrackSearchService trackSearchService)
        {
            _repository = repository;
            _playRequestHandler = playRequestHandler;
            _trackSearchService = trackSearchService;

            PlayCriteriaPlaylistCommand = new RelayCommand<CriteriaPlaylist>(OnPlayCriteriaPlaylistCommand);
            PlayOrderedPlaylistCommand = new RelayCommand<OrderedPlaylist>(OnPlayOrderedPlaylistCommand);
            ShuffleLibraryCommand = new RelayCommand(OnShuffleLibrary);

            CriteriaPlaylists = new ObservableCollection<CriteriaPlaylist>();
            OrderedPlaylists = new ObservableCollection<OrderedPlaylist>();
        }

        public ObservableCollection<CriteriaPlaylist> CriteriaPlaylists
        {
            get { return _criteriaPlaylists; }
            set { Set(ref _criteriaPlaylists, value); }
        }

        public ObservableCollection<OrderedPlaylist> OrderedPlaylists
        {
            get { return _orderedPlaylists; }
            set { Set(ref _orderedPlaylists, value); }
        }

        public RelayCommand<CriteriaPlaylist> PlayCriteriaPlaylistCommand { get; }
        public RelayCommand<OrderedPlaylist> PlayOrderedPlaylistCommand { get; }
        public RelayCommand ShuffleLibraryCommand { get; }

        public void Load()
        {
            var playlists = _repository.GetPlaylists(true);
            playlists.CriteriaPlaylists.ForEach(CriteriaPlaylists.Add);
            playlists.OrderedPlaylists.ForEach(OrderedPlaylists.Add);
        }

        private void OnPlayOrderedPlaylistCommand(OrderedPlaylist playlist)
        {
            _playRequestHandler.PlayOrderedPlaylist(playlist.Title, _trackSearchService.GetTracks(playlist.Tracks));
        }

        private void OnPlayCriteriaPlaylistCommand(CriteriaPlaylist playlist)
        {
            _playRequestHandler.PlayCriteriaPlaylist(playlist.Title, _trackSearchService.GetTracks(playlist));
        }

        private void OnShuffleLibrary()
        {
            _playRequestHandler.PlayAll(SortType.Random);
        }
    }
}
