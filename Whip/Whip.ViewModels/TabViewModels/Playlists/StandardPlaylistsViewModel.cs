using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Linq;
using Whip.Common;
using Whip.Common.Enums;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.TabViewModels.Playlists
{
    public class StandardPlaylistsViewModel : ViewModelBase
    {
        private readonly Common.Singletons.Library _library;
        private readonly IMessenger _messenger;
        private readonly IPlayRequestHandler _playRequestHandler;
        private readonly ITrackSearchService _trackSearchService;
        
        private QuickPlaylist _selectedGrouping;
        private QuickPlaylist _selectedGenre;
        private QuickPlaylist _selectedCountry;
        private QuickPlaylist _selectedState;
        private QuickPlaylist _selectedCity;
        private QuickPlaylist _selectedTag;
        private QuickPlaylist _selectedAddedDateOption;
        
        private List<QuickPlaylist> _groupings;
        private List<QuickPlaylist> _genres;
        private List<QuickPlaylist> _countries;
        private List<QuickPlaylist> _states;
        private List<QuickPlaylist> _cities;
        private List<QuickPlaylist> _tags;

        public StandardPlaylistsViewModel(Common.Singletons.Library library, IMessenger messenger, IPlayRequestHandler playRequestHandler,
            ITrackSearchService trackSearchService)
        {
            _library = library;
            _messenger = messenger;
            _playRequestHandler = playRequestHandler;
            _trackSearchService = trackSearchService;

            AddedDateOptions = GetDateAddedOptions();

            PlayGroupingCommand = new RelayCommand(OnPlayGrouping);
            PlayGenreCommand = new RelayCommand(OnPlayGenre);
            PlayCountryCommand = new RelayCommand(OnPlayCountry);
            PlayStateCommand = new RelayCommand(OnPlayState);
            PlayCityCommand = new RelayCommand(OnPlayCity);
            PlayTagCommand = new RelayCommand(OnPlayTag);
            PlayRecentlyAddedCommand = new RelayCommand(OnPlayRecentlyAdded);
        }

        private bool CheckOptionSelected(object selectedOption, string optionType)
        {
            if (selectedOption != null)
                return true;

            _messenger.Send(new ShowDialogMessage(_messenger, MessageType.Error, "Auto Playlist Error", $"No {optionType} selected"));
            return false;
        }

        private void OnPlayRecentlyAdded()
        {
            if (!CheckOptionSelected(SelectedAddedDateOption, "time period"))
                return;
            
            Play(SelectedAddedDateOption);
        }

        private void OnPlayTag()
        {
            if (!CheckOptionSelected(SelectedTag, "tag"))
                return;
            
            Play(SelectedTag);
        }

        private void OnPlayCity()
        {
            if (!CheckOptionSelected(SelectedCity, "city"))
                return;
            
            Play(SelectedCity);
        }

        private void OnPlayState()
        {
            if (!CheckOptionSelected(SelectedState, "state"))
                return;
            
            Play(SelectedState);
        }

        private void OnPlayCountry()
        {
            if (!CheckOptionSelected(SelectedCountry, "country"))
                return;

            Play(SelectedCountry);
        }

        private void OnPlayGenre()
        {
            if (!CheckOptionSelected(SelectedGenre, "genre"))
                return;

            Play(SelectedGenre);
        }

        private void OnPlayGrouping()
        {
            if (!CheckOptionSelected(SelectedGrouping, "grouping"))
                return;
            
            Play(SelectedGrouping);
        }

        private void Play(QuickPlaylist playlist)
        {
            var tracks = _trackSearchService.GetTracks(playlist.FilterType, playlist.FilterValues);

            if (!tracks.Any())
            {
                _messenger.Send(new ShowDialogMessage(_messenger, MessageType.Error, "Auto Playlist Error", "No tracks meet these criteria"));
                return;
            }

            _playRequestHandler.PlayPlaylist(playlist.GetDefaultTitle(), tracks, SortType.Random);
        }

        public RelayCommand PlayGroupingCommand { get; }
        public RelayCommand PlayGenreCommand { get; }
        public RelayCommand PlayCountryCommand { get; }
        public RelayCommand PlayStateCommand { get; }
        public RelayCommand PlayCityCommand { get; }
        public RelayCommand PlayTagCommand { get; }
        public RelayCommand PlayRecentlyAddedCommand { get; }

        public List<QuickPlaylist> Groupings
        {
            get { return _groupings; }
            set { Set(ref _groupings, value); }
        }

        public List<QuickPlaylist> Genres
        {
            get { return _genres; }
            set { Set(ref _genres, value); }
        }

        public List<QuickPlaylist> Countries
        {
            get { return _countries; }
            set { Set(ref _countries, value); }
        }

        public List<QuickPlaylist> States
        {
            get { return _states; }
            set { Set(ref _states, value); }
        }

        public List<QuickPlaylist> Cities
        {
            get { return _cities; }
            set { Set(ref _cities, value); }
        }

        public List<QuickPlaylist> Tags
        {
            get { return _tags; }
            set { Set(ref _tags, value); }
        }

        public List<QuickPlaylist> AddedDateOptions { get; }

        public QuickPlaylist SelectedGrouping
        {
            get { return _selectedGrouping; }
            set
            {
                Set(ref _selectedGrouping, value);
                ClearSelections(nameof(SelectedGrouping));
            }
        }

        public QuickPlaylist SelectedGenre
        {
            get { return _selectedGenre; }
            set
            {
                Set(ref _selectedGenre, value);
                ClearSelections(nameof(SelectedGenre));
            }
        }

        public QuickPlaylist SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                Set(ref _selectedCountry, value);
                ClearSelections(nameof(SelectedCountry));
            }
        }

        public QuickPlaylist SelectedState
        {
            get { return _selectedState; }
            set
            {
                Set(ref _selectedState, value);
                ClearSelections(nameof(SelectedState));
            }
        }

        public QuickPlaylist SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                Set(ref _selectedCity, value);
                ClearSelections(nameof(SelectedCity));
            }
        }

        public QuickPlaylist SelectedTag
        {
            get { return _selectedTag; }
            set
            {
                Set(ref _selectedTag, value);
                ClearSelections(nameof(SelectedTag));
            }
        }

        public QuickPlaylist SelectedAddedDateOption
        {
            get { return _selectedAddedDateOption; }
            set
            {
                Set(ref _selectedAddedDateOption, value);
                ClearSelections(nameof(SelectedAddedDateOption));
            }
        }

        private void ClearSelections(string except)
        {
            if (except != nameof(SelectedGrouping))
            {
                _selectedGrouping = null;
            }

            if (except != nameof(SelectedGenre))
            {
                _selectedGenre = null;
            }

            if (except != nameof(SelectedCountry))
            {
                _selectedCountry = null;
            }

            if (except != nameof(SelectedState))
            {
                _selectedState = null;
            }

            if (except != nameof(SelectedCity))
            {
                _selectedCity = null;
            }

            if (except != nameof(SelectedTag))
            {
                _selectedTag = null;
            }

            if (except != nameof(SelectedAddedDateOption))
            {
                _selectedAddedDateOption = null;
            }

            RaisePropertyChanged(nameof(SelectedGrouping));
            RaisePropertyChanged(nameof(SelectedGenre));
            RaisePropertyChanged(nameof(SelectedCountry));
            RaisePropertyChanged(nameof(SelectedState));
            RaisePropertyChanged(nameof(SelectedCity));
            RaisePropertyChanged(nameof(SelectedTag));
            RaisePropertyChanged(nameof(SelectedAddedDateOption));
        }

        public void UpdateOptions()
        {
            Groupings = _library.Artists.Select(a => a.Grouping).Distinct().OrderBy(g => g).Select(g => new QuickPlaylist(0, g, false, FilterType.Grouping, g)).ToList();
            Genres = _library.Artists.Select(a => a.Genre).Distinct().OrderBy(g => g).Select(g => new QuickPlaylist(0, g, false, FilterType.Genre, g)).ToList();
            Tags = _library.Artists.SelectMany(a => a.Tracks).SelectMany(t => t.Tags).Distinct().OrderBy(t => t).Select(t => new QuickPlaylist(0, t, false, FilterType.Tag, t)).ToList();

            var cities = _library.Artists.Select(a => a.City).Distinct().ToList();
            Cities = GetCities(cities);
            States = GetStates(cities);
            Countries = GetCountries(cities);

            ClearSelections(string.Empty);
        }

        private static List<QuickPlaylist> GetDateAddedOptions()
        {
            return new List<QuickPlaylist>
            {
                new QuickPlaylist(0, "in the last week", false, FilterType.DateAdded, "7"),
                new QuickPlaylist(0, "in the last 2 weeks", false, FilterType.DateAdded, "14"),
                new QuickPlaylist(0, "in the last 30 days", false, FilterType.DateAdded, "30"),
                new QuickPlaylist(0, "in the last year", false, FilterType.DateAdded, "365")
            };
        }

        private static List<QuickPlaylist> GetCities(IEnumerable<City> cities)
        {
            return cities.Where(c => !string.IsNullOrEmpty(c?.Name))
                .Distinct()
                .OrderBy(c => c.Country)
                .ThenBy(c => c.State)
                .ThenBy(c => c.Name)
                .Select(c => new QuickPlaylist(0, c.CityStateDescription, false, FilterType.City, c.Name, c.State, c.Country))
                .ToList();
        }
        private static List<QuickPlaylist> GetStates(IEnumerable<City> cities)
        {
            return cities.Where(c => !string.IsNullOrEmpty(c?.State))
                .Select(c => new State(c))
                .Distinct()
                .OrderBy(s => s.Country)
                .ThenBy(s => s.Name)
                .Select(c => new QuickPlaylist(0, c.Name, false, FilterType.State, c.Name, c.Country))
                .ToList();
        }

        private static List<QuickPlaylist> GetCountries(IEnumerable<City> cities)
        {
            return cities.Where(c => !string.IsNullOrEmpty(c?.Country))
                .Select(c => c.Country)
                .Distinct()
                .OrderBy(c => c)
                .Select(c => new QuickPlaylist(0, c, false, FilterType.Country, c))
                .ToList();
        }

    }
}
