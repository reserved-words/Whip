using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.TabViewModels.Library
{
    public class ArtistViewModel : ViewModelBase
    {
        private readonly IImageProcessingService _imageProcessingService;
        private readonly IMessenger _messenger;
        private readonly IArtistInfoService _webArtistInfoService;
        private readonly IConfigSettings _configSettings;
        private readonly IPlayRequestHandler _playRequestHandler;
        private readonly ITrackFilterService _trackFilterService;

        public ArtistViewModel(IMessenger messenger, IArtistInfoService webArtistInfoService, IImageProcessingService imageProcessingService, 
            TrackContextMenuViewModel trackContextMenuViewModel, IConfigSettings configSettings, IPlayRequestHandler playRequestHandler, ITrackFilterService trackFilterService)
        {
            _imageProcessingService = imageProcessingService;
            _messenger = messenger;
            _webArtistInfoService = webArtistInfoService;
            _configSettings = configSettings;
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
        private BitmapImage _image;
        private bool _loadingArtistImage;
        private bool _displayTracksByArtist;

        public TrackContextMenuViewModel TrackContextMenu { get; private set; }
        public RelayCommand PlayAlbumCommand { get; private set; }
        public RelayCommand PlayArtistCommand { get; private set; }

        public Artist Artist
        {
            get { return _artist; }
            set
            {
                if (value == null || value == _artist)
                    return;

                Set(ref _artist, value);
                Task.Run(PopulateLastFmInfo);
                UpdateTracks();
            }
        }

        public bool LoadingArtistImage
        {
            get { return _loadingArtistImage; }
            set { Set(ref _loadingArtistImage, value); }
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

        public BitmapImage Image
        {
            get { return _image; }
            private set { Set(ref _image, value); }
        }

        public string Wiki => Artist?.WebInfo?.Wiki;

        public void OnEditTrack(Track track)
        {
            _messenger.Send(new EditTrackMessage(track));
        }

        private void OnPlayAlbum()
        {
            _playRequestHandler.PlayAlbum(SelectedAlbum, SortType.Ordered);
        }

        public void OnPlayArtist()
        {
            _playRequestHandler.PlayArtist(Artist, SortType.Ordered, SelectedTrack);
        }

        private async Task PopulateLastFmInfo()
        {
            if (Artist == null)
                return;

            LoadingArtistImage = true;

            await _webArtistInfoService.PopulateArtistInfo(Artist, _configSettings.NumberOfSimilarArtistsToDisplay);

            Image = await _imageProcessingService.GetImageFromUrl(Artist?.WebInfo.ExtraLargeImageUrl);
            LoadingArtistImage = false;
            RaisePropertyChanged(nameof(Wiki));
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
