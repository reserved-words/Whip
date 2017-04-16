using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using Whip.Common;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.ViewModels.Utilities;
using GalaSoft.MvvmLight.Messaging;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.TabViewModels
{
    public class CurrentPlaylistViewModel : TabViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly Playlist _playlist;

        private Track _selectedTrack;

        public CurrentPlaylistViewModel(Playlist playlist, IMessenger messenger)
            :base(TabType.CurrentPlaylist, IconType.ListOl, "Current Playlist")
        {
            _messenger = messenger;
            _playlist = playlist;

            _playlist.ListUpdated += OnPlaylistUpdated;

            PlayCommand = new RelayCommand(OnPlay);
        }

        public RelayCommand PlayCommand { get; private set; }

        public string PlaylistName => _playlist.PlaylistName;

        public Track SelectedTrack
        {
            get { return _selectedTrack; }
            set { Set(ref _selectedTrack, value); }
        }

        public List<Track> Tracks => _playlist.Tracks;

        private void OnPlay()
        {
            if (SelectedTrack != null)
            {
                _messenger.Send(new MoveToTrackMessage(SelectedTrack));
            }
        }

        private void OnPlaylistUpdated()
        {
            RaisePropertyChanged(nameof(PlaylistName));
            RaisePropertyChanged(nameof(Tracks));
        }
    }
}
