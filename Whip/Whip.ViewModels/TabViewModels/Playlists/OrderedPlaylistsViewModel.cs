using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.TabViewModels.Playlists
{
    public class OrderedPlaylistsViewModel
    {
        private readonly PlaylistsViewModel _parent;
        private readonly IMessenger _messenger;
        private readonly IPlaylistRepository _repository;
        private readonly ITrackSearchService _trackSearchService;
        private readonly IPlayRequestHandler _playRequestHandler;

        public OrderedPlaylistsViewModel(PlaylistsViewModel parent, IMessenger messenger, ITrackSearchService trackSearchService,
            IPlaylistRepository repository, IPlayRequestHandler playRequestHandler)
        {
            _messenger = messenger;
            _parent = parent;
            _repository = repository;
            _trackSearchService = trackSearchService;
            _playRequestHandler = playRequestHandler;

            Playlists = new ObservableCollection<OrderedPlaylist>();

            AddNewPlaylistCommand = new RelayCommand(OnAddNewPlaylist);
            DeleteCommand = new RelayCommand<OrderedPlaylist>(OnDelete);
            EditCommand = new RelayCommand<OrderedPlaylist>(OnEdit);
            PlayCommand = new RelayCommand<OrderedPlaylist>(OnPlay);
            FavouriteCommand = new RelayCommand<OrderedPlaylist>(OnFavourite);
        }

        public RelayCommand AddNewPlaylistCommand { get; }
        public RelayCommand<OrderedPlaylist> DeleteCommand { get; }
        public RelayCommand<OrderedPlaylist> EditCommand { get; }
        public RelayCommand<OrderedPlaylist> PlayCommand { get; }
        public RelayCommand<OrderedPlaylist> FavouriteCommand { get; }

        public ObservableCollection<OrderedPlaylist> Playlists { get; set; }

        public void Update(List<OrderedPlaylist> playlists = null)
        {
            if (playlists == null)
                playlists = new List<OrderedPlaylist>(Playlists);

            Playlists.Clear();
            playlists.ForEach(Playlists.Add);
        }

        private void OnAddNewPlaylist()
        {
            var newPlaylist = new OrderedPlaylist(0, "New Playlist", false);
            _messenger.Send(new EditOrderedPlaylistMessage(newPlaylist));
        }

        private void OnDelete(OrderedPlaylist playlist)
        {
            _parent.Remove(playlist);
        }

        private void OnEdit(OrderedPlaylist playlist)
        {
            _messenger.Send(new EditOrderedPlaylistMessage(playlist));
        }

        private void OnPlay(OrderedPlaylist playlist)
        {
            _playRequestHandler.PlayOrderedPlaylist(playlist.Title, _trackSearchService.GetTracks(playlist.Tracks), SortType.Ordered);
        }

        private void OnFavourite(OrderedPlaylist playlist)
        {
            playlist.Favourite = !playlist.Favourite;
            _repository.Save(playlist);
            Update();
        }
    }
}
