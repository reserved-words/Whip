using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
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
        private readonly IArtistWebInfoService _artistWebInfoService;
        private readonly IImageProcessingService _imageProcessingService;
        private readonly IConfigSettings _configSettings;

        private bool _showingCurrentArtist = true;
        private Track _currentTrack;
        private Artist _artist;
        private List<ArtistWebSimilarArtist> _similarArtists;
        private BitmapImage _image;
        private bool _loadingArtistImage;
        private bool _ukEventsOnly;

        public CurrentArtistViewModel(Common.Singletons.Library library, IArtistWebInfoService artistWebInfoService,
            IImageProcessingService imageProcessingService, IConfigSettings configSettings)
            :base(TabType.CurrentArtist, IconType.Users, "Artist")
        {
            _imageProcessingService = imageProcessingService;
            _library = library;
            _artistWebInfoService = artistWebInfoService;
            _configSettings = configSettings;

            ShowCurrentArtistCommand = new RelayCommand(ShowCurrentArtist);

            _similarArtists = new List<ArtistWebSimilarArtist>();

            for (var i = 0; i < _configSettings.NumberOfSimilarArtistsToDisplay; i++)
            {
                _similarArtists.Add(new ArtistWebSimilarArtist());
            }

            Tweets = new ObservableCollection<Tweet>();
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

        public bool DisplayLargeSimilarArtistsView => Artist?.LatestVideo == null;

        public Artist Artist
        {
            get { return _artist; }
            set
            {
                if (value == null || value == _artist)
                    return;

                _artist = null;
                Tweets.Clear();
                Set(ref _artist, value);
                Task.Run(PopulateMainInfo);
                Task.Run(PopulateEvents);
                Task.Run(PopulateVideo);
                Task.Run(PopulateTweets).ContinueWith((t,o)=> { Artist.Tweets.ForEach(Tweets.Add); }, CancellationToken.None, TaskScheduler.FromCurrentSynchronizationContext());
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

        public ObservableCollection<Tweet> Tweets { get; private set; }

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

        private async Task PopulateMainInfo()
        {
            LoadingArtistImage = true;

            await _artistWebInfoService.PopulateArtistInfo(Artist, _configSettings.NumberOfSimilarArtistsToDisplay);

            Image = await _imageProcessingService.GetImageFromUrl(Artist?.WebInfo.ExtraLargeImageUrl);
            
            for (var i = 0; i < _configSettings.NumberOfSimilarArtistsToDisplay; i++)
            {
                _similarArtists[i] = Artist == null || Artist.WebInfo.SimilarArtists.Count < i + 1
                    ? new ArtistWebSimilarArtist()
                    : Artist.WebInfo.SimilarArtists[i];
            }

            LoadingArtistImage = false;

            RaisePropertyChanged(nameof(Wiki));
            RaisePropertyChanged(nameof(SimilarArtists));
        }

        private async Task PopulateEvents()
        {
            await _artistWebInfoService.PopulateEventsAsync(Artist);

            RaisePropertyChanged(nameof(UpcomingEvents));
            RaisePropertyChanged(nameof(NoUpcomingEvents));
        }

        private async Task PopulateVideo()
        {
            await _artistWebInfoService.PopulateTweets(Artist);

            RaisePropertyChanged(nameof(Video));
            RaisePropertyChanged(nameof(DisplayLargeSimilarArtistsView));
        }

        private async Task PopulateTweets()
        {
            await _artistWebInfoService.PopulateTweets(Artist);
        }
    }
}
