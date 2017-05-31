using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.TabViewModels.Library
{
    public class ArtistViewModel : ViewModelBase
    {
        private readonly IImageProcessingService _imageProcessingService;
        private readonly IMessenger _messenger;
        private readonly ITrackFilterService _trackFilterService;
        private readonly IWebArtistInfoService _webArtistInfoService;
        private readonly TrackContextMenuViewModel _trackContextMenuViewModel;
        private readonly IConfigSettings _configSettings;

        public ArtistViewModel(ITrackFilterService trackFilterService, IMessenger messenger, IWebArtistInfoService webArtistInfoService,
            IImageProcessingService imageProcessingService, TrackContextMenuViewModel trackContextMenuViewModel, IConfigSettings configSettings)
        {
            _imageProcessingService = imageProcessingService;
            _messenger = messenger;
            _trackContextMenuViewModel = trackContextMenuViewModel;
            _trackFilterService = trackFilterService;
            _webArtistInfoService = webArtistInfoService;
            _configSettings = configSettings;
        }

        private Artist _artist;
        private List<AlbumViewModel> _albums;

        private BitmapImage _image;
        private bool _loadingArtistImage;

        public Artist Artist
        {
            get { return _artist; }
            set
            {
                if (value == null || value == _artist)
                    return;

                Set(ref _artist, value);
                Task.Run(PopulateLastFmInfo);
                PopulateAlbums();
            }
        }

        public bool LoadingArtistImage
        {
            get { return _loadingArtistImage; }
            set { Set(ref _loadingArtistImage, value); }
        }

        public List<AlbumViewModel> Albums
        {
            get { return _albums; }
            set { Set(ref _albums, value); }
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

        public void OnPlay(Track startAt)
        {
            _messenger.Send(new PlayArtistMessage(Artist, SortType.Ordered, startAt));
        }

        public void OnPlayAlbum(AlbumViewModel album)
        {
            _messenger.Send(new PlayAlbumMessage(album.Album, SortType.Ordered));
        }

        private string Format(int? count, string description)
        {
            return !count.HasValue
                ? string.Empty
                : count == 1
                ? string.Format("1 {0}", description)
                : string.Format("{0} {1}s", count, description);
        }

        private async Task PopulateLastFmInfo()
        {
            if (Artist == null)
                return;

            LoadingArtistImage = true;

            if (string.IsNullOrEmpty(Artist.WebInfo.Wiki))
            {
                Artist.WebInfo = await _webArtistInfoService.PopulateArtistInfo(Artist, _configSettings.NumberOfSimilarArtistsToDisplay);
            }

            Image = await _imageProcessingService.GetImageFromUrl(Artist?.WebInfo.ExtraLargeImageUrl);
            LoadingArtistImage = false;
            RaisePropertyChanged(nameof(Wiki));
        }

        private void PopulateAlbums()
        {
            if (Artist == null)
            {
                Albums = new List<AlbumViewModel>();
                return;
            }

            var tracks = _trackFilterService.GetTracksByArtist(Artist);

            Albums = tracks.Select(t => t.Disc)
                .Select(d => d.Album)
                .Distinct()
                .Select(a => new AlbumViewModel(a, this, _trackContextMenuViewModel))
                .ToList();
        }
    }
}
