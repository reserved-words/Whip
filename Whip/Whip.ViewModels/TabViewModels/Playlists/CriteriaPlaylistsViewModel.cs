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
    public class CriteriaPlaylistsViewModel
    {
        private readonly IMessenger _messenger;
        private readonly PlaylistsViewModel _parent;
        private readonly ITrackSearchService _playlistService;
        private readonly IPlayRequestHandler _playRequestHandler;
        private readonly IPlaylistsService _repository;

        public CriteriaPlaylistsViewModel(PlaylistsViewModel parent, IMessenger messenger, ITrackSearchService playlistService,
            IPlaylistsService repository, IPlayRequestHandler playRequestHandler)
        {
            _messenger = messenger;
            _playlistService = playlistService;
            _parent = parent;
            _playRequestHandler = playRequestHandler;
            _repository = repository;

            Playlists = new ObservableCollection<CriteriaPlaylist>();

            AddNewPlaylistCommand = new RelayCommand(OnAddNewPlaylist);
            DeleteCommand = new RelayCommand<CriteriaPlaylist>(OnDelete);
            EditCommand = new RelayCommand<CriteriaPlaylist>(OnEdit);
            PlayCommand = new RelayCommand<CriteriaPlaylist>(OnPlay);
            FavouriteCommand = new RelayCommand<CriteriaPlaylist>(OnFavourite);
        }

        public RelayCommand AddNewPlaylistCommand { get; }
        public RelayCommand<CriteriaPlaylist> DeleteCommand { get; }
        public RelayCommand<CriteriaPlaylist> EditCommand { get; }
        public RelayCommand<CriteriaPlaylist> PlayCommand { get; }
        public RelayCommand<CriteriaPlaylist> FavouriteCommand { get; }

        public ObservableCollection<CriteriaPlaylist> Playlists { get; set; }

        public void Update(List<CriteriaPlaylist> playlists = null)
        {
            if (playlists == null)
                playlists = new List<CriteriaPlaylist>(Playlists);

            Playlists.Clear();
            playlists.ForEach(Playlists.Add);
        }

        private void OnAddNewPlaylist()
        {
            var newPlaylist = new CriteriaPlaylist(0, "New Playlist", false);
            _messenger.Send(new EditCriteriaPlaylistMessage(newPlaylist));
        }

        private void OnDelete(CriteriaPlaylist playlist)
        {
            _parent.Remove(playlist);
            if (playlist.Favourite)
            {
                _parent.OnFavouritePlaylistsUpdated();
            }
        }

        private void OnEdit(CriteriaPlaylist playlist)
        {
            _messenger.Send(new EditCriteriaPlaylistMessage(playlist));
        }

        private void OnPlay(CriteriaPlaylist playlist)
        {
            _playRequestHandler.PlayPlaylist(playlist.Title, _playlistService.GetTracks(playlist), SortType.Random);
        }

        private void OnFavourite(CriteriaPlaylist playlist)
        {
            playlist.Favourite = !playlist.Favourite;
            _repository.Save(playlist);
            Update();
            _parent.OnFavouritePlaylistsUpdated();
        }
    }
}
