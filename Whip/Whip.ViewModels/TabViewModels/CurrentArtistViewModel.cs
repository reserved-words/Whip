using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
        private readonly Common.Singletons.Library _library;
        private readonly IWebArtistInfoService _webArtistInfoService;
        private readonly IImageProcessingService _imageProcessingService;

        private bool _showingCurrentArtist = true;
        private Track _currentTrack;
        private Artist _artist;
        private List<ArtistWebSimilarArtist> _similarArtists;
        private BitmapImage _image;
        private bool _loadingArtistImage;
        private bool _ukEventsOnly;

        public CurrentArtistViewModel(Common.Singletons.Library library, IMessenger messenger, ITrackFilterService trackFilterService,
            ILibrarySortingService librarySortingService, IWebArtistInfoService webArtistInfoService, IImageProcessingService imageProcessingService)
            :base(TabType.CurrentArtist, IconType.Users, "Artist")
        {
            _imageProcessingService = imageProcessingService;
            _library = library;
            _webArtistInfoService = webArtistInfoService;

            ShowCurrentArtistCommand = new RelayCommand(ShowCurrentArtist);

            _similarArtists = new List<ArtistWebSimilarArtist>();

            for (var i = 0; i < ApplicationSettings.NumberOfSimilarArtistsToDisplay; i++)
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
            }
        }

        public string Wiki => Artist?.WebInfo?.Wiki;

        public List<ArtistEvent> UpcomingEvents => Artist?.UpcomingEvents.Where(ev => !UKEventsOnly || ev.Country == "UK").ToList();

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

            if (string.IsNullOrEmpty(Artist.WebInfo.Wiki))
            {
                Artist.WebInfo = await _webArtistInfoService.PopulateArtistInfo(Artist, ApplicationSettings.NumberOfSimilarArtistsToDisplay);
            }

            Image = await _imageProcessingService.GetImageFromUrl(Artist?.WebInfo.ExtraLargeImageUrl);
            LoadingArtistImage = false;
            RaisePropertyChanged(nameof(Wiki));
            PopulateSimilarArtists();
        }

        private async Task PopulateEvents()
        {
            if (Artist != null && Artist.UpcomingEventsUpdated.AddDays(ApplicationSettings.DaysBeforeUpdatingArtistEvents) < DateTime.Now)
            {
                // Change to using service

                Artist.UpcomingEvents = new List<ArtistEvent>
                {
                    new ArtistEvent
                    {
                        Date = DateTime.Now.AddDays(3),
                        Venue = "Venue Number 1",
                        City = "London",
                        Country = "UK",
                        ArtistList = "Manic Street Preachers, Super Furry Animals, Catatonia"
                    },
                    new ArtistEvent
                    {
                        Date = DateTime.Now.AddDays(3),
                        Venue = "Venue Number 2",
                        City = "Bristol",
                        Country = "UK",
                        ArtistList = "Manic Street Preachers, Super Furry Animals, Catatonia"
                    },
                    new ArtistEvent
                    {
                        Date = DateTime.Now.AddDays(3),
                        Venue = "Venue Number 3",
                        City = "Manchester",
                        Country = "UK",
                        ArtistList = "Manic Street Preachers, Super Furry Animals, Catatonia"
                    },
                    new ArtistEvent
                    {
                        Date = DateTime.Now.AddDays(3),
                        Venue = "Venue Number 4",
                        City = "Los Angeles",
                        Country = "USA",
                        ArtistList = "Manic Street Preachers, Super Furry Animals, Catatonia"
                    }
                };
                Artist.UpcomingEventsUpdated = DateTime.Now;
            }

            RaisePropertyChanged(nameof(UpcomingEvents));
        }

        private void PopulateSimilarArtists()
        {
            for (var i = 0; i < ApplicationSettings.NumberOfSimilarArtistsToDisplay; i++)
            {
                _similarArtists[i] = Artist.WebInfo.SimilarArtists.Count < i + 1
                    ? new ArtistWebSimilarArtist()
                    : Artist.WebInfo.SimilarArtists[i];
            }

            RaisePropertyChanged(nameof(SimilarArtists));
        }
    }
}
