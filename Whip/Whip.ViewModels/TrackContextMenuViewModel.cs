using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels
{
    public class TrackContextMenuViewModel : ViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly IPlaylistRepository _playlistRepository;

        public TrackContextMenuViewModel(IMessenger messenger, IPlaylistRepository playlistRepository)
        {
            _messenger = messenger;
            _playlistRepository = playlistRepository;

            EditTrackCommand = new RelayCommand(OnEditTrack);
            AddToPlaylistCommand = new RelayCommand<OrderedPlaylist>(OnAddToPlaylist);

            MenuItems = new List<MenuCommand>
            {
                new MenuCommand { Header = "Edit Track", Command = EditTrackCommand },
                new MenuCommand
                {
                    Header = "Add to Playlist",
                    SubCommands = _playlistRepository.GetPlaylists()
                        .OrderedPlaylists
                        .Select(pl => new MenuCommand
                        {
                            Header = pl.Title,
                            Command = AddToPlaylistCommand,
                            CommandParameter = pl
                        }).ToList()
                }
            };
        }

        public Track Track { get; private set; }
        public RelayCommand EditTrackCommand { get; private set; }
        public RelayCommand<OrderedPlaylist> AddToPlaylistCommand { get; private set; }
        public List<MenuCommand> MenuItems { get; private set; }

        public void SetTrack(Track track)
        {
            Track = track;
        }

        private void OnAddToPlaylist(object playlist)
        {
            var selectedPlaylist = playlist as OrderedPlaylist;

            selectedPlaylist.Tracks.Add(Track.File.FullPath);

            _playlistRepository.Save(selectedPlaylist);
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
