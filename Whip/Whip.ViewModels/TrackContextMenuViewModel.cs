using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;
using Whip.ViewModels.Windows;

namespace Whip.ViewModels
{
    public class TrackContextMenuViewModel : ViewModelBase
    {
        private const string NewPlaylistCommandName = "New Playlist...";
        private const string ArchiveTrackMessageTitle = "Archive Track";

        private readonly IArchiveService _archiveService;
        private readonly IMessenger _messenger;
        private readonly IPlaylistsService _playlistsService;

        private ObservableCollection<MenuCommand> _menuCommands;
        private List<OrderedPlaylist> _playlists;

        public TrackContextMenuViewModel() { }

        public TrackContextMenuViewModel(IArchiveService archiveService, IMessenger messenger, IPlaylistsService playlistsService)
        {
            _archiveService = archiveService;
            _messenger = messenger;
            _playlistsService = playlistsService;

            ArchiveTrackCommand = new RelayCommand(OnArchiveTrack);
            EditTrackCommand = new RelayCommand(OnEditTrack);
            AddToPlaylistCommand = new RelayCommand<OrderedPlaylist>(OnAddToPlaylist);

            MenuItems = new ObservableCollection<MenuCommand>();
        }

        public void SetCommands()
        {
            _playlists = _playlistsService.GetPlaylists().OrderedPlaylists;
            _playlists.Add(new OrderedPlaylist(0, NewPlaylistCommandName, false));

            _menuCommands.Clear();

            _menuCommands.Add(new MenuCommand { Header = "Edit Track", Command = EditTrackCommand });

            _menuCommands.Add(new MenuCommand
            {
                Header = "Add to Playlist",
                SubCommands = _playlists
                        .Select(pl => new MenuCommand
                        {
                            Header = pl.Title,
                            Command = AddToPlaylistCommand,
                            CommandParameter = pl
                        }).ToList()
            });

            _menuCommands.Add(new MenuCommand { Header = "Archive Track", Command = ArchiveTrackCommand });

            RaisePropertyChanged(nameof(MenuItems));
        }

        public Track Track { get; private set; }
        public RelayCommand ArchiveTrackCommand { get; }
        public RelayCommand EditTrackCommand { get; }
        public RelayCommand<OrderedPlaylist> AddToPlaylistCommand { get; }

        public ObservableCollection<MenuCommand> MenuItems
        {
            get
            {
                if (!_menuCommands.Any())
                {
                    SetCommands();
                }
                return _menuCommands;
            }
            private set { _menuCommands = value; }
        }

        public virtual void SetTrack(Track track)
        {
            Track = track;
        }

        private void OnAddToPlaylist(object playlist)
        {
            var selectedPlaylist = playlist as OrderedPlaylist;

            if (selectedPlaylist.Id == 0)
            {
                var enterTitleViewModel = new EnterStringViewModel(
                    _messenger,
                    "New Playlist",
                    "Enter a name for the new playlist",
                    str => !_playlists.Select(pl => pl.Title).Contains(str),
                    "The selected title is not valid");

                _messenger.Send(new ShowDialogMessage(enterTitleViewModel));

                selectedPlaylist.Title = enterTitleViewModel.Result;
            }

            if (selectedPlaylist.Tracks.Contains(Track.File.FullPath))
            {
                _messenger.Send(new ShowDialogMessage(_messenger, MessageType.Info, "Add to Playlist",
                    "The selected Track already belongs to this playlist"));
                return;
            }

            selectedPlaylist.Tracks.Add(Track.File.FullPath);

            _playlistsService.Save(selectedPlaylist);

            SetCommands();
        }

        private void OnArchiveTrack()
        {
            if (Track.IsCurrentTrack)
            {
                _messenger.Send(new ShowDialogMessage(_messenger, MessageType.Error, ArchiveTrackMessageTitle, "You cannot archive a track which is currently playing"));
                return;
            }

            if (!Confirm(ArchiveTrackMessageTitle, "Are you sure you want to archive the selected track? The track will be removed from any saved playlists") )
                return;

            string errorMessage;

            if (!_archiveService.ArchiveTracks(new List<Track> { Track }, out errorMessage))
            {
                _messenger.Send(new ShowDialogMessage(_messenger, MessageType.Error, ArchiveTrackMessageTitle, errorMessage));
            }
        }

        private void OnEditTrack()
        {
            _messenger.Send(new EditTrackMessage(Track));
        }

        private bool Confirm(string title, string message)
        {
            var confirmationViewModel = new ConfirmationViewModel(_messenger, title, message, ConfirmationViewModel.ConfirmationType.YesNo);
            _messenger.Send(new ShowDialogMessage(confirmationViewModel));
            return confirmationViewModel.Result;
        }
    }

    public class MenuCommand
    {
        public string Header { get; set; }
        public ICommand Command { get; set; }
        public List<MenuCommand> SubCommands { get; set; } = new List<MenuCommand>();
        public object CommandParameter { get; set; }
    }
}
