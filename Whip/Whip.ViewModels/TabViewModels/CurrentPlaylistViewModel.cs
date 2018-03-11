using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using Whip.Common;
using Whip.Common.Model;
using Whip.ViewModels.Utilities;
using Whip.Services.Interfaces.Singletons;

namespace Whip.ViewModels.TabViewModels
{
    public class CurrentPlaylistViewModel : TabViewModelBase
    {
        private readonly IPlaylist _playlist;
        private readonly IPlayRequestHandler _playRequestHandler;

        private Track _selectedTrack;

        public CurrentPlaylistViewModel(IPlaylist playlist, Common.Singletons.Library library, TrackContextMenuViewModel trackContextMenu,
            IPlayRequestHandler playRequestHandler)
            :base(TabType.CurrentPlaylist, IconType.ListOl, "Current Playlist")
        {
            _playlist = playlist;
            _playRequestHandler = playRequestHandler;

            _playlist.ListUpdated += OnPlaylistUpdated;
            library.Updated += OnLibraryUpdated;
            
            PlayCommand = new RelayCommand(OnPlay);

            TrackContextMenu = trackContextMenu;
        }

        private void OnLibraryUpdated(Track track)
        {
            if (track != null)
            {
                RaisePropertyChanged(nameof(Tracks));
            }
        }

        public TrackContextMenuViewModel TrackContextMenu { get; }

        public RelayCommand PlayCommand { get; }

        public string PlaylistName => _playlist.PlaylistName;

        public Track SelectedTrack
        {
            get { return _selectedTrack; }
            set
            {
                Set(ref _selectedTrack, value);
                TrackContextMenu.SetTrack(_selectedTrack);
            }
        }

        public List<Track> Tracks => _playlist.Tracks;

        private void OnPlay()
        {
            if (SelectedTrack != null)
            {
                _playRequestHandler.MoveToTrack(SelectedTrack);
            }
        }

        private void OnPlaylistUpdated()
        {
            RaisePropertyChanged(nameof(PlaylistName));
            RaisePropertyChanged(nameof(Tracks));
        }
    }
}
