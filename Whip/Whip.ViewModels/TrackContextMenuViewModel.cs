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

        private readonly IMessenger _messenger;
        private readonly IPlaylistRepository _playlistRepository;

        private ObservableCollection<MenuCommand> _menuCommands;
        private List<OrderedPlaylist> _playlists;

        public TrackContextMenuViewModel(IMessenger messenger, IPlaylistRepository playlistRepository)
        {
            _messenger = messenger;
            _playlistRepository = playlistRepository;

            EditTrackCommand = new RelayCommand(OnEditTrack);
            AddToPlaylistCommand = new RelayCommand<OrderedPlaylist>(OnAddToPlaylist);

            MenuItems = new ObservableCollection<MenuCommand>();
        }

        public void SetCommands()
        {
            _playlists = _playlistRepository.GetPlaylists().OrderedPlaylists;
            _playlists.Add(new OrderedPlaylist(0, NewPlaylistCommandName));

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

            RaisePropertyChanged(nameof(MenuItems));
        }

        public Track Track { get; private set; }
        public RelayCommand EditTrackCommand { get; private set; }
        public RelayCommand<OrderedPlaylist> AddToPlaylistCommand { get; private set; }

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

        public void SetTrack(Track track)
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

            selectedPlaylist.Tracks.Add(Track.File.FullPath);

            _playlistRepository.Save(selectedPlaylist);

            SetCommands();
        }

        private void OnEditTrack()
        {
            _messenger.Send(new EditTrackMessage(Track));
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
