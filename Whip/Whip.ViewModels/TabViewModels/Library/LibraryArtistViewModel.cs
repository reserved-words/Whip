using GalaSoft.MvvmLight;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.ViewModels.TabViewModels.Library
{
    public class LibraryArtistViewModel : ViewModelBase
    {
        private readonly IImageProcessingService _imageProcessingService;
        private readonly IArtistInfoService _webArtistInfoService;
        private readonly IConfigSettings _configSettings;

        public LibraryArtistViewModel(IArtistInfoService webArtistInfoService, IImageProcessingService imageProcessingService, IConfigSettings configSettings)
        {
            _imageProcessingService = imageProcessingService;
            _webArtistInfoService = webArtistInfoService;
            _configSettings = configSettings;
        }

        private Artist _artist;
        private string _wiki;
        private BitmapImage _image;
        private bool _loadingArtistImage;

        public Artist Artist
        {
            get { return _artist; }
            set { SetArtist(value); }
        }

        public BitmapImage Image
        {
            get { return _image; }
            private set { Set(ref _image, value); }
        }

        public bool LoadingArtistImage
        {
            get { return _loadingArtistImage; }
            set { Set(ref _loadingArtistImage, value); }
        }

        public string Wiki
        {
            get { return _wiki; }
            private set { Set(ref _wiki, value); }
        }

        private async Task UpdateArtistInfo()
        {
            if (string.IsNullOrEmpty(Artist.Name))
            {
                Wiki = "";
                Image = null;
                return;
            }

            LoadingArtistImage = true;
            await _webArtistInfoService.PopulateArtistInfo(Artist, _configSettings.NumberOfSimilarArtistsToDisplay);
            Wiki = Artist?.WebInfo?.Wiki;
            Image = await _imageProcessingService.GetImageFromUrl(Artist?.WebInfo?.ExtraLargeImageUrl);
            LoadingArtistImage = false;
        }

        private void SetArtist(Artist artist)
        {
            if (artist == null || artist.Equals(_artist))
                return;

            Set(nameof(Artist), ref _artist, artist);
            Task.Run(UpdateArtistInfo);
        }
    }
}
