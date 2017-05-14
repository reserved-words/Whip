using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.TabViewModels.Playlists
{
    public class CriteriaPlaylistsViewModel
    {
        private readonly Common.Singletons.Library _library;
        private readonly IMessenger _messenger;
        private readonly PlaylistsViewModel _parent;
        private readonly IPlaylistService _playlistService;

        public CriteriaPlaylistsViewModel(PlaylistsViewModel parent, Common.Singletons.Library library, IMessenger messenger, IPlaylistService playlistService)
        {
            _library = library;
            _messenger = messenger;
            _playlistService = playlistService;
            _parent = parent;

            Playlists = new ObservableCollection<CriteriaPlaylist>();

            AddNewPlaylistCommand = new RelayCommand(OnAddNewPlaylist);
            DeleteCommand = new RelayCommand<CriteriaPlaylist>(OnDelete);
            EditCommand = new RelayCommand<CriteriaPlaylist>(OnEdit);
            PlayCommand = new RelayCommand<CriteriaPlaylist>(OnPlay);
        }

        public RelayCommand AddNewPlaylistCommand { get; private set; }
        public RelayCommand<CriteriaPlaylist> DeleteCommand { get; private set; }
        public RelayCommand<CriteriaPlaylist> EditCommand { get; private set; }
        public RelayCommand<CriteriaPlaylist> PlayCommand { get; private set; }

        public ObservableCollection<CriteriaPlaylist> Playlists { get; set; }

        public void Update(List<CriteriaPlaylist> playlists)
        {
            Playlists.Clear();
            playlists.ForEach(Playlists.Add);
        }

        private void OnAddNewPlaylist()
        {
            var newPlaylist = new CriteriaPlaylist(0, "New Playlist");
            _messenger.Send(new EditCriteraPlaylistMessage(newPlaylist));
        }

        private void OnDelete(CriteriaPlaylist playlist)
        {
            _parent.Remove(playlist);
        }

        private void OnEdit(CriteriaPlaylist playlist)
        {
            _messenger.Send(new EditCriteraPlaylistMessage(playlist));
        }

        private void OnPlay(CriteriaPlaylist playlist)
        {
            _messenger.Send(new PlayPlaylistMessage(playlist.Title, SortType.Random, _playlistService.GetTracks(playlist)));
        }
    }
}
