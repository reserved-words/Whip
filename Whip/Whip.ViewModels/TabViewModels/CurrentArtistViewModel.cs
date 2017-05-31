using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly string[] ValidUKCountryNames = new string[] 
        {
            "UK",
            "United Kingdom",
            "England",
            "Wales",
            "Scotland",
            "Northern Ireland"
        };

        private readonly Common.Singletons.Library _library;
        private readonly IWebArtistInfoService _webArtistInfoService;
        private readonly IImageProcessingService _imageProcessingService;
        private readonly IWebArtistEventsService _webArtistEventsService;
        private readonly IVideoService _videoService;
        private readonly IConfigSettings _configSettings;

        private bool _showingCurrentArtist = true;
        private Track _currentTrack;
        private Artist _artist;
        private List<ArtistWebSimilarArtist> _similarArtists;
        private BitmapImage _image;
        private bool _loadingArtistImage;
        private bool _ukEventsOnly;

        public CurrentArtistViewModel(Common.Singletons.Library library, IWebArtistInfoService webArtistInfoService, 
            IImageProcessingService imageProcessingService, IWebArtistEventsService webArtistEventsService, IVideoService videoService,
            IConfigSettings configSettings)
            :base(TabType.CurrentArtist, IconType.Users, "Artist")
        {
            _imageProcessingService = imageProcessingService;
            _library = library;
            _webArtistInfoService = webArtistInfoService;
            _webArtistEventsService = webArtistEventsService;
            _videoService = videoService;
            _configSettings = configSettings;

            ShowCurrentArtistCommand = new RelayCommand(ShowCurrentArtist);

            _similarArtists = new List<ArtistWebSimilarArtist>();

            for (var i = 0; i < _configSettings.NumberOfSimilarArtistsToDisplay; i++)
            {
                _similarArtists.Add(new ArtistWebSimilarArtist());
            }
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
                Task.Run(PopulateEvents);
                Task.Run(PopulateVideo);
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

        public List<ArtistWebSimilarArtist> SimilarArtists
        {
            get { return _similarArtists; }
            private set { Set(ref _similarArtists, value); }
        }

        public bool UKEventsOnly
        {
            get { return _ukEventsOnly; }
            set
            {
                Set(ref(_ukEventsOnly), value);
                RaisePropertyChanged(nameof(UpcomingEvents));
                RaisePropertyChanged(nameof(NoUpcomingEvents));
            }
        }

        public string Wiki => Artist?.WebInfo?.Wiki;

        public Video Video => Artist?.LatestVideo;

        public List<ArtistEvent> UpcomingEvents => Artist?.UpcomingEvents
            .Where(ev => !UKEventsOnly || ValidUKCountryNames.Contains(ev.Country)).ToList();

        public string NoUpcomingEvents => Artist == null || UpcomingEvents.Any()
            ? string.Empty
            : UKEventsOnly
            ? "No upcoming UK events"
            : "No upcoming events";

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
            Artist = _currentTrack?.Artist;
            _showingCurrentArtist = true;
        }

        private async Task PopulateLastFmInfo()
        {
            if (Artist == null)
                return;

            LoadingArtistImage = true;

            if (Artist.WebInfo.Updated.AddDays(_configSettings.DaysBeforeUpdatingArtistWebInfo) < DateTime.Now)
            {
                Artist.WebInfo = await _webArtistInfoService.PopulateArtistInfo(Artist, _configSettings.NumberOfSimilarArtistsToDisplay);
            }

            Image = await _imageProcessingService.GetImageFromUrl(Artist?.WebInfo.ExtraLargeImageUrl);
            LoadingArtistImage = false;
            RaisePropertyChanged(nameof(Wiki));
            PopulateSimilarArtists();
        }

        private async Task PopulateEvents()
        {
            if (Artist != null && Artist.UpcomingEventsUpdated.AddDays(_configSettings.DaysBeforeUpdatingArtistWebInfo) < DateTime.Now)
            {
                Artist.UpcomingEvents = await _webArtistEventsService.GetEventsAsync(Artist);
                Artist.UpcomingEventsUpdated = DateTime.Now;
            }

            RaisePropertyChanged(nameof(UpcomingEvents));
            RaisePropertyChanged(nameof(NoUpcomingEvents));
        }

        private async Task PopulateVideo()
        {
            if (Artist != null && Artist.VideoUpdated.AddDays(_configSettings.DaysBeforeUpdatingArtistWebInfo) < DateTime.Now)
            {
                Artist.LatestVideo = await _videoService.GetLatestVideoAsync(Artist);
                Artist.VideoUpdated = DateTime.Now;
            }

            RaisePropertyChanged(nameof(Video));
        }

        private void PopulateSimilarArtists()
        {
            for (var i = 0; i < _configSettings.NumberOfSimilarArtistsToDisplay; i++)
            {
                _similarArtists[i] = Artist.WebInfo.SimilarArtists.Count < i + 1
                    ? new ArtistWebSimilarArtist()
                    : Artist.WebInfo.SimilarArtists[i];
            }

            RaisePropertyChanged(nameof(SimilarArtists));
        }
    }
}
