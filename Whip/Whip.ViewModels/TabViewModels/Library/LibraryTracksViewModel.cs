using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.ViewModels.TabViewModels.Library
{
    public class LibraryTracksViewModel : ViewModelBase
    {
        private readonly IPlayRequestHandler _playRequestHandler;
        private readonly ILibrarySortingService _sortingService;

        private Artist _artist;
        private Track _selectedTrack;
        private Album _selectedAlbum;
        private List<Track> _tracks;
        private bool _displayTracksByArtist;

        public LibraryTracksViewModel(TrackContextMenuViewModel trackContextMenuViewModel, IPlayRequestHandler playRequestHandler, ILibrarySortingService sortingService)
        {
            _playRequestHandler = playRequestHandler;
            _sortingService = sortingService;
            TrackContextMenu = trackContextMenuViewModel;
            PlayArtistCommand = new RelayCommand(OnPlayArtist);
            PlayAlbumCommand = new RelayCommand(OnPlayAlbum);
        }

        public TrackContextMenuViewModel TrackContextMenu { get; }
        public RelayCommand PlayAlbumCommand { get; }
        public RelayCommand PlayArtistCommand { get; }

        public Artist Artist
        {
            get { return _artist; }
            set { SetArtist(value); }
        }

        public Track SelectedTrack
        {
            get { return _selectedTrack; }
            set { SetSelectedTrack(value); }
        }

        public List<Track> Tracks
        {
            get { return _tracks; }
            set { Set(ref _tracks, value); }
        }

        public Album SelectedAlbum
        {
            get { return _selectedAlbum; }
            set { Set(ref _selectedAlbum, value); }
        }

        private void OnPlayAlbum()
        {
            _playRequestHandler.PlayAlbum(SelectedAlbum, SortType.Ordered);
        }

        public void OnPlayArtist()
        {
            _playRequestHandler.PlayArtist(Artist, SortType.Random, SelectedTrack);
        }

        private void SetArtist(Artist artist)
        {
            if (artist == null || artist.Equals(_artist))
                return;

            Set(nameof(Artist), ref _artist, artist);
            UpdateTracks();
        }

        private void SetSelectedTrack(Track track)
        {
            Set(nameof(SelectedTrack), ref _selectedTrack, track);
            TrackContextMenu.SetTrack(_selectedTrack);
        }

        public void UpdateTracks(bool? displayTracksByArtist = null)
        {
            if (displayTracksByArtist.HasValue)
                _displayTracksByArtist = displayTracksByArtist.Value;

            Tracks = _artist == null
                ? null
                : (_displayTracksByArtist
                    ? _sortingService.GetArtistTracksInDefaultOrder(_artist)
                    : _sortingService.GetAlbumTracksInDefaultOrder(_artist)
                    ).ToList();
        }
    }
}
