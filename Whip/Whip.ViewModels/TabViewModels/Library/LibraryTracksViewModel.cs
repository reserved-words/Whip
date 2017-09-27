using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.ViewModels.TabViewModels.Library
{
    public class LibraryTracksViewModel : ViewModelBase
    {
        private readonly IPlayRequestHandler _playRequestHandler;
        private readonly ITrackFilterService _trackFilterService;

        public LibraryTracksViewModel(TrackContextMenuViewModel trackContextMenuViewModel, IPlayRequestHandler playRequestHandler, ITrackFilterService trackFilterService)
        {
            _playRequestHandler = playRequestHandler;
            _trackFilterService = trackFilterService;

            TrackContextMenu = trackContextMenuViewModel;

            PlayArtistCommand = new RelayCommand(OnPlayArtist);
            PlayAlbumCommand = new RelayCommand(OnPlayAlbum);
        }

        private Artist _artist;
        private Track _selectedTrack;
        private Album _selectedAlbum;
        private IEnumerable<Track> _tracks;
        private bool _displayTracksByArtist;

        public TrackContextMenuViewModel TrackContextMenu { get; }
        public RelayCommand PlayAlbumCommand { get; }
        public RelayCommand PlayArtistCommand { get; }

        public Artist Artist
        {
            get { return _artist; }
            set
            {
                if (value == null || value == _artist)
                    return;

                Set(ref _artist, value);
                UpdateTracks();
            }
        }

        internal void UpdateDisplayTracks(bool displayTracksByArtist)
        {
            _displayTracksByArtist = displayTracksByArtist;
            UpdateTracks();
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

        public IEnumerable<Track> Tracks
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
            _playRequestHandler.PlayArtist(Artist, SortType.Ordered, SelectedTrack);
        }

        private void UpdateTracks()
        {
            Tracks = _artist == null
                ? null
                : (_displayTracksByArtist
                    ? _artist.Tracks
                    : _artist.Albums.SelectMany(a => a.Discs).SelectMany(d => d.Tracks))
                        .OrderBy(t => t.Disc.Album.Artist.SortName)
                        .ThenBy(t => t.Disc.Album.ReleaseType)
                        .ThenBy(t => t.Disc.Album.Year)
                        .ThenBy(t => t.Disc.Album.Title)
                        .ThenBy(t => t.Disc.DiscNo)
                        .ThenBy(t => t.TrackNo)
                        .ToList();
        }

        public void UpdateDisplayTracks()
        {
            _displayTracksByArtist = false;
            UpdateTracks();
        }

        public void DisplayArtistTracks()
        {
            _displayTracksByArtist = true;
            UpdateTracks();
        }
    }
}
