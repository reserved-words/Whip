using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class EditOrderedPlaylistViewModel : EditableTabViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly IPlaylistRepository _repository;
        private readonly ITrackSearchService _trackSearchService;

        private OrderedPlaylist _playlist;
        
        private ObservableCollection<Track> _tracks;
        private string _playlistTitle;
        private Track _selectedTrack;
        
        public EditOrderedPlaylistViewModel(IMessenger messenger, IPlaylistRepository repository, ITrackSearchService trackSearchService,
            TrackContextMenuViewModel trackContextMenu)
            :base(TabType.Playlists, IconType.Cog, "Edit Playlist", messenger, false)
        {
            _trackSearchService = trackSearchService;
            _messenger = messenger;
            _repository = repository;

            TrackContextMenu = trackContextMenu;

            MoveUpCommand = new RelayCommand(OnMoveUp);
            MoveDownCommand = new RelayCommand(OnMoveDown);
            RemoveCommand = new RelayCommand(OnRemove);
        }

        public TrackContextMenuViewModel TrackContextMenu { get; private set; }

        public RelayCommand MoveUpCommand { get; private set; }
        public RelayCommand MoveDownCommand { get; private set; }
        public RelayCommand RemoveCommand { get; private set; }

        public string PlaylistTitle
        {
            get { return _playlistTitle; }
            set { SetModified(nameof(PlaylistTitle), ref _playlistTitle, value); }
        }

        public ObservableCollection<Track> Tracks
        {
            get { return _tracks; }
            set { Set(ref _tracks, value); }
        }

        public Track SelectedTrack
        {
            get { return _selectedTrack; }
            set
            {
                Set(ref _selectedTrack, value);
                TrackContextMenu.SetTrack(_selectedTrack);
            }
        }

        private void OnMoveUp()
        {
            var trackToMove = SelectedTrack;

            var index = Tracks.IndexOf(trackToMove);

            if (index == 0)
                return;

            Tracks.Remove(trackToMove);

            Tracks.Insert(index - 1, trackToMove);

            Modified = true;
        }

        public void Insert(Track selected, Track currentPosition)
        {
            Tracks.Remove(selected);
            Tracks.Insert(Tracks.IndexOf(currentPosition), selected);
            Modified = true;
        }

        public void Edit(OrderedPlaylist playlist)
        {
            _playlist = playlist;

            PlaylistTitle = _playlist.Title;

            Tracks = new ObservableCollection<Track>(_trackSearchService.GetTracks(playlist.Tracks));
        }

        protected override string ErrorMessage
        {
            get
            {
                var errorMessage = "";

                if (string.IsNullOrEmpty(PlaylistTitle))
                {
                    errorMessage = errorMessage + "You must select a playlist title" + Environment.NewLine;
                }
                else if (!_repository.ValidatePlaylistTitle(PlaylistTitle, _playlist.Id))
                {
                    errorMessage = errorMessage + string.Format("There is already a playlist called {0}{1}", PlaylistTitle, Environment.NewLine);
                }

                return errorMessage;
            }
        }

        protected override bool CustomCancel()
        {
            return true;
        }

        protected override bool CustomSave()
        {
            _playlist = CreatePlaylist(_playlist);

            _repository.Save(_playlist);

            TrackContextMenu.SetCommands();

            return true;
        }

        private void OnMoveDown()
        {
            var trackToMove = SelectedTrack;

            var index = Tracks.IndexOf(trackToMove);

            if (index == Tracks.Count - 1)
                return;

            Tracks.Remove(trackToMove);

            Tracks.Insert(index + 1, trackToMove);

            Modified = true;
        }

        private void OnRemove()
        {
            Tracks.Remove(SelectedTrack);
            Modified = true;
        }
        
        private OrderedPlaylist CreatePlaylist(OrderedPlaylist playlist = null)
        {
            if (playlist == null)
            {
                playlist = new OrderedPlaylist(0, "");
            }

            playlist.Title = PlaylistTitle;

            playlist.Tracks = _tracks.Select(t => t.File.FullPath).ToList();

            return playlist;
        }
    }
}
