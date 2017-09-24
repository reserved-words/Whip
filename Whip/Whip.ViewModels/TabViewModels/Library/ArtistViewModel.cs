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
        private readonly TrackContextMenuViewModel _trackContextMenuViewModel;
        private readonly IConfigSettings _configSettings;
        private readonly IPlayRequestHandler _playRequestHandler;

        public ArtistViewModel(IMessenger messenger, IArtistInfoService webArtistInfoService, IImageProcessingService imageProcessingService, 
            TrackContextMenuViewModel trackContextMenuViewModel, IConfigSettings configSettings, IPlayRequestHandler playRequestHandler)
        {
            _imageProcessingService = imageProcessingService;
            _messenger = messenger;
            _trackContextMenuViewModel = trackContextMenuViewModel;
            _webArtistInfoService = webArtistInfoService;
            _configSettings = configSettings;
            _playRequestHandler = playRequestHandler;

            TrackContextMenu = trackContextMenuViewModel;

            PlayArtistCommand = new RelayCommand(OnPlayArtist);
            PlayAlbumCommand = new RelayCommand(OnPlayAlbum);
        }

        private Artist _artist;
        private Track _selectedTrack;
        private Album _selectedAlbum;

        private BitmapImage _image;
        private bool _loadingArtistImage;

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
            }
        }

        public bool LoadingArtistImage
        {
            get { return _loadingArtistImage; }
            set { Set(ref _loadingArtistImage, value); }
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
    }
}
