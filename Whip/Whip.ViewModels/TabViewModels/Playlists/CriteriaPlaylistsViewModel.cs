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
        private readonly IPlaylistService _playlistService;

        public CriteriaPlaylistsViewModel(Common.Singletons.Library library, IMessenger messenger, IPlaylistService playlistService)
        {
            _library = library;
            _messenger = messenger;
            _playlistService = playlistService;

            Playlists = new ObservableCollection<CriteriaPlaylist>();

            EditCommand = new RelayCommand<CriteriaPlaylist>(OnEdit);
            PlayCommand = new RelayCommand<CriteriaPlaylist>(OnPlay);
        }

        public RelayCommand<CriteriaPlaylist> EditCommand { get; private set; }
        public RelayCommand<CriteriaPlaylist> PlayCommand { get; private set; }

        public ObservableCollection<CriteriaPlaylist> Playlists { get; set; }

        public void Update(List<CriteriaPlaylist> playlists)
        {
            Playlists.Clear();
            playlists.ForEach(Playlists.Add);
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
