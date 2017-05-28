using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class CurrentArtistViewModel : TabViewModelBase
    {
        private readonly Common.Singletons.Library _library;
        private readonly IWebArtistInfoService _webArtistInfoService;
        private readonly IImageProcessingService _imageProcessingService;

        private bool _showingCurrentArtist = true;
        private Track _currentTrack;
        private Artist _artist;

        private BitmapImage _image;
        private bool _loadingArtistImage;

        public CurrentArtistViewModel(Common.Singletons.Library library, IMessenger messenger, ITrackFilterService trackFilterService,
            ILibrarySortingService librarySortingService, IWebArtistInfoService webArtistInfoService, IImageProcessingService imageProcessingService)
            :base(TabType.CurrentArtist, IconType.Users, "Artist")
        {
            _imageProcessingService = imageProcessingService;
            _library = library;
            _webArtistInfoService = webArtistInfoService;

            ShowCurrentArtistCommand = new RelayCommand(ShowCurrentArtist);
        }

        public RelayCommand ShowCurrentArtistCommand { get; private set; }

        public List<Artist> Artists => _library.Artists;

        public bool ShowingCurrentArtist
        {
            get { return _showingCurrentArtist; }
            set
            {
                Set(ref _showingCurrentArtist, value);
                RaisePropertyChanged(nameof(NotShowingCurrentArtist));
                if (_showingCurrentArtist)
                {
                    ShowCurrentArtist();
                }
            }
        }

        public bool NotShowingCurrentArtist => !ShowingCurrentArtist;
        
        public Artist Artist
        {
            get { return _artist; }
            set
            {
                if (value == null || value == _artist)
                    return;

                Set(ref _artist, value);
                Task.Run(PopulateLastFmInfo);
            }
        }

        public bool LoadingArtistImage
        {
            get { return _loadingArtistImage; }
            set { Set(ref _loadingArtistImage, value); }
        }

        public BitmapImage Image
        {
            get { return _image; }
            private set { Set(ref _image, value); }
        }

        public string Wiki => Artist?.WebInfo?.Wiki;

        public override void OnCurrentTrackChanged(Track track)
        {
            _currentTrack = track;

            if (!_showingCurrentArtist)
                return;

            if (track == null)
            {
                Artist = null;
                return;
            }

            if (Artist == track.Artist)
                return;

            Artist = track.Artist;
            _showingCurrentArtist = true;
        }

        public override void OnShow(Track currentTrack)
        {
            RaisePropertyChanged(nameof(Artists));
            OnCurrentTrackChanged(currentTrack);
        }

        private void ShowCurrentArtist()
        {
            Artist = _currentTrack.Artist;
            _showingCurrentArtist = true;
        }

        private async Task PopulateLastFmInfo()
        {
            if (Artist == null)
                return;

            LoadingArtistImage = true;

            if (string.IsNullOrEmpty(Artist.WebInfo.Wiki))
            {
                Artist.WebInfo = await _webArtistInfoService.PopulateArtistInfo(Artist);
            }

            Image = await _imageProcessingService.GetImageFromUrl(Artist?.WebInfo.ExtraLargeImageUrl);
            LoadingArtistImage = false;
            RaisePropertyChanged(nameof(Wiki));
        }
    }
}
