using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Whip.Common.Model;
using Whip.Common.Validation;
using Whip.Services.Interfaces;
using Whip.ViewModels.Utilities;
using Whip.ViewModels.Validation;
using static Whip.Resources.Resources;

namespace Whip.ViewModels.TabViewModels.EditTrack
{
    public class ArtistViewModel : EditableViewModelBase
    {
        private readonly TrackViewModel _parent;
        private readonly IWebBrowserService _webBrowserService;

        private List<City> _usedCities;
        private List<Artist> _artists;

        private List<string> _groupings;
        private List<string> _genres;
        private List<string> _countries;
        private List<string> _states;
        private List<string> _cities;

        private Artist _artist;

        private string _name;
        private string _grouping;
        private string _genre;
        private string _country;
        private string _state;
        private string _city;
        private string _website;
        private string _twitter;
        private string _facebook;
        private string _lastFm;
        private string _wikipedia;

        public ArtistViewModel(TrackViewModel parent, List<Artist> artists, Artist artist, IWebBrowserService webBrowserService)
        {
            _parent = parent;
            _webBrowserService = webBrowserService;

            Artists = artists;

            TestWebsiteCommand = new RelayCommand(OnTestWebsite, CanTestWebsite);
            TestFacebookCommand = new RelayCommand(OnTestFacebook, CanTestFacebook);
            TestTwitterCommand = new RelayCommand(OnTestTwitter, CanTestTwitter);
            TestWikipediaCommand = new RelayCommand(OnTestWikipedia, CanTestWikipedia);
            TestLastFmCommand = new RelayCommand(OnTestLastFm, CanTestLastFm);

            Populate(artist);

            Modified = false;
        }

        public bool ExistingArtistSelected => Artist != null && Artist.Name != AddNew;

        public string FacebookUrl => string.Format(Resources.Resources.FacebookUrl, Facebook);
        public string TwitterUrl => string.Format(Resources.Resources.TwitterUrl, Twitter);
        public string WikipediaUrl => string.Format(Resources.Resources.WikipediaUrl, Wikipedia);
        public string LastFmUrl => string.Format(Resources.Resources.LastFmUrl, LastFm);

        public RelayCommand TestWebsiteCommand { get; private set; }
        public RelayCommand TestFacebookCommand { get; private set; }
        public RelayCommand TestTwitterCommand { get; private set; }
        public RelayCommand TestWikipediaCommand { get; private set; }
        public RelayCommand TestLastFmCommand { get; private set; }

        public List<Artist> Artists
        {
            get { return _artists; }
            set { Set(ref _artists, value); }
        }

        public List<string> Groupings
        {
            get { return _groupings; }
            set { Set(ref _groupings, value); }
        }

        public List<string> Genres
        {
            get { return _genres; }
            set { Set(ref _genres, value); }
        }

        public List<string> Countries
        {
            get { return _countries; }
            set { Set(ref _countries, value); }
        }

        public List<string> States
        {
            get { return _states; }
            set { Set(ref _states, value); }
        }

        public List<string> Cities
        {
            get { return _cities; }
            set { Set(ref _cities, value); }
        }

        public Artist Artist
        {
            get { return _artist; }
            set
            {
                SetModified(nameof(Artist), ref _artist, value);
                PopulateArtistDetails();
                _parent.SyncArtists();
            }
        }

        [ArtistName]
        [Display(Name = "Artist Name")]
        public string Name
        {
            get { return _name; }
            set
            {
                SetModified(nameof(Name), ref _name, value);
                _parent.SyncArtists();
            }
        }

        [Required(ErrorMessageResourceName = nameof(RequiredErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [MaxLength(TrackValidation.MaxLengthGrouping, ErrorMessageResourceName = nameof(MaxLengthErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "Grouping")]
        public string Grouping
        {
            get { return _grouping; }
            set { SetModified(nameof(Grouping), ref _grouping, value); }
        }

        [Required(ErrorMessageResourceName = nameof(RequiredErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [MaxLength(TrackValidation.MaxLengthGenre, ErrorMessageResourceName = nameof(MaxLengthErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        public string Genre
        {
            get { return _genre; }
            set { SetModified(nameof(Genre), ref _genre, value); }
        }

        [MaxLength(TrackValidation.MaxLengthCountry, ErrorMessageResourceName = nameof(MaxLengthErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        public string Country
        {
            get { return _country; }
            set
            {
                SetModified(nameof(Country), ref _country, value);
                PopulateStateList();
            }
        }

        [MaxLength(TrackValidation.MaxLengthState, ErrorMessageResourceName = nameof(MaxLengthErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        public string State
        {
            get { return _state; }
            set
            {
                SetModified(nameof(State), ref _state, value);
                PopulateCityList();
            }
        }

        [MaxLength(TrackValidation.MaxLengthCity, ErrorMessageResourceName = nameof(MaxLengthErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        public string City
        {
            get { return _city; }
            set { SetModified(nameof(City), ref _city, value); }
        }

        [NullableUrl]
        public string Website
        {
            get { return _website; }
            set
            {
                SetModified(nameof(Website), ref _website, value);
                TestWebsiteCommand.RaiseCanExecuteChanged();
            }
        }

        [TwitterUsername]
        [Display(Name = "Twitter Username")]
        public string Twitter
        {
            get { return _twitter; }
            set
            {
                SetModified(nameof(Twitter), ref _twitter, value);
                TestTwitterCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(TwitterUrl));
            }
        }

        [FacebookUsername]
        [Display(Name = "Facebook Username")]
        public string Facebook
        {
            get { return _facebook; }
            set
            {
                SetModified(nameof(Facebook), ref _facebook, value);
                TestFacebookCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(FacebookUrl));
            }
        }

        [PartialUrlString]
        [Display(Name = "Artist Last.FM page")]
        public string LastFm
        {
            get { return _lastFm; }
            set
            {
                SetModified(nameof(LastFm), ref _lastFm, value);
                TestLastFmCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(LastFmUrl));
            }
        }

        [PartialUrlString]
        [Display(Name = "Artist Wikipedia page")]
        public string Wikipedia
        {
            get { return _wikipedia; }
            set
            {
                SetModified(nameof(Wikipedia), ref _wikipedia, value);
                TestWikipediaCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(WikipediaUrl));
            }
        }

        public void PopulateOptionLists()
        {
            Groupings = Artists.Select(a => a.Grouping).Distinct().OrderBy(g => g).ToList();

            Genres = Artists.Select(a => a.Genre).Distinct().OrderBy(g => g).ToList();

            _usedCities = Artists.Select(a => a.City).ToList();

            Countries = _usedCities.Select(c => c.Country).Distinct().OrderBy(c => c).ToList();
        }

        public void UpdateArtist(Track track)
        {
            var artist = ExistingArtistSelected
                ? Artist
                : new Artist();

            artist.Name = Name;
            artist.Grouping = Grouping;
            artist.Genre = Genre;
            artist.City = new City(City, State, Country);
            artist.Website = Website;
            artist.Facebook = Facebook;
            artist.Twitter = Twitter;
            artist.Wikipedia = Wikipedia;
            artist.LastFm = LastFm;

            track.Artist = artist;
        }

        private bool CanTestFacebook()
        {
            return !string.IsNullOrEmpty(Facebook) && string.IsNullOrEmpty(this[nameof(Facebook)]);
        }

        private bool CanTestLastFm()
        {
            return !string.IsNullOrEmpty(LastFm) && string.IsNullOrEmpty(this[nameof(LastFm)]);
        }

        private bool CanTestTwitter()
        {
            return !string.IsNullOrEmpty(Twitter) && string.IsNullOrEmpty(this[nameof(Twitter)]);
        }

        private bool CanTestWebsite()
        {
            return !string.IsNullOrEmpty(Website) && string.IsNullOrEmpty(this[nameof(Website)]);
        }

        private bool CanTestWikipedia()
        {
            return !string.IsNullOrEmpty(Wikipedia) && string.IsNullOrEmpty(this[nameof(Wikipedia)]);
        }

        private void OnTestFacebook()
        {
            _webBrowserService.Open(FacebookUrl);
        }

        private void OnTestLastFm()
        {
            _webBrowserService.Open(LastFmUrl);
        }

        private void OnTestTwitter()
        {
            _webBrowserService.Open(TwitterUrl);
        }

        private void OnTestWebsite()
        {
            _webBrowserService.Open(Website);
        }

        private void OnTestWikipedia()
        {
            _webBrowserService.Open(WikipediaUrl);
        }

        private void Populate(Artist artist)
        {
            PopulateOptionLists();

            Artist = artist;

            PopulateArtistDetails();
        }

        private void PopulateArtistDetails()
        {
            // Force revalidation when value is set
            Name = string.Empty;

            var artist = Artist?.Name == AddNew ? null : Artist;
            
            Name = artist?.Name ?? string.Empty;
            Grouping = artist?.Grouping ?? string.Empty;
            Genre = artist?.Genre ?? string.Empty;
            Country = artist?.City.Country ?? string.Empty;
            State = artist?.City.State ?? string.Empty;
            City = artist?.City.Name ?? string.Empty;

            Website = artist?.Website ?? string.Empty;
            Facebook = artist?.Facebook ?? string.Empty;
            Twitter = artist?.Twitter ?? string.Empty;
            Wikipedia = artist?.Wikipedia ?? string.Empty;
            LastFm = artist?.LastFm ?? string.Empty;
        }

        private void PopulateCityList()
        {
            Cities = _usedCities
                .Where(c => (string.IsNullOrEmpty(Country) || c.Country == Country)
                    && (string.IsNullOrEmpty(State) || c.State == State))
                .Select(c => c.Name)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
        }

        private void PopulateStateList()
        {
            States = _usedCities
                .Where(c => string.IsNullOrEmpty(Country) || c.Country == Country)
                .Select(c => c.State)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            PopulateCityList();
        }
    }
}
