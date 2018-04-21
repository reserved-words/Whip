using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
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

        private readonly Dictionary<string, int> _dateAddedOptions;

        private string _selectedGrouping;
        private string _selectedGenre;
        private string _selectedCountry;
        private State _selectedState;
        private City _selectedCity;
        private string _selectedTag;
        private string _selectedAddedDateOption;

        private List<string> _groupings;
        private List<string> _genres;
        private List<string> _countries;
        private List<State> _states;
        private List<City> _cities;
        private List<string> _tags;

        public StandardPlaylistsViewModel(Common.Singletons.Library library, IMessenger messenger, IPlayRequestHandler playRequestHandler,
            ITrackSearchService trackSearchService)
        {
            _library = library;
            _messenger = messenger;
            _playRequestHandler = playRequestHandler;
            _trackSearchService = trackSearchService;

            _dateAddedOptions = GetDateAddedOptions();

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
            
            Play($"Tracks Added {SelectedAddedDateOption}", FilterType.DateAdded, _dateAddedOptions[SelectedAddedDateOption].ToString());
        }

        private void OnPlayTag()
        {
            if (!CheckOptionSelected(SelectedTag, "tag"))
                return;
            
            Play($"Tag: {SelectedTag}", FilterType.Tag, SelectedCity.Name, SelectedTag);
        }

        private void OnPlayCity()
        {
            if (!CheckOptionSelected(SelectedCity, "city"))
                return;
            
            Play($"City: {SelectedCity.Description}", FilterType.City, SelectedCity.Name, SelectedCity.State, SelectedCity.Country);
        }

        private void OnPlayState()
        {
            if (!CheckOptionSelected(SelectedState, "state"))
                return;
            
            Play($"State: {SelectedState.Description}", FilterType.State, SelectedState.Name, SelectedState.Country);
        }

        private void OnPlayCountry()
        {
            if (!CheckOptionSelected(SelectedCountry, "country"))
                return;

            Play($"Country: {SelectedCountry}", FilterType.Country, SelectedCountry);
        }

        private void OnPlayGenre()
        {
            if (!CheckOptionSelected(SelectedGenre, "genre"))
                return;

            Play($"Genre: {SelectedGenre}", FilterType.Genre, SelectedGenre);
        }

        private void OnPlayGrouping()
        {
            if (!CheckOptionSelected(SelectedGrouping, "grouping"))
                return;
            
            Play($"Grouping: {SelectedGrouping}", FilterType.Grouping, SelectedGrouping);
        }

        private void Play(string title, FilterType type, params string[] values)
        {
            var tracks = _trackSearchService.GetTracks(type, values);

            if (!tracks.Any())
            {
                _messenger.Send(new ShowDialogMessage(_messenger, MessageType.Error, "Auto Playlist Error", "No tracks meet these criteria"));
                return;
            }

            _playRequestHandler.PlayPlaylist(title, tracks, SortType.Random);
        }

        public RelayCommand PlayGroupingCommand { get; }
        public RelayCommand PlayGenreCommand { get; }
        public RelayCommand PlayCountryCommand { get; }
        public RelayCommand PlayStateCommand { get; }
        public RelayCommand PlayCityCommand { get; }
        public RelayCommand PlayTagCommand { get; }
        public RelayCommand PlayRecentlyAddedCommand { get; }

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

        public List<State> States
        {
            get { return _states; }
            set { Set(ref _states, value); }
        }

        public List<City> Cities
        {
            get { return _cities; }
            set { Set(ref _cities, value); }
        }

        public List<string> Tags
        {
            get { return _tags; }
            set { Set(ref _tags, value); }
        }

        public List<string> AddedDateOptions => _dateAddedOptions.Keys.ToList();

        public string SelectedGrouping
        {
            get { return _selectedGrouping; }
            set
            {
                Set(ref _selectedGrouping, value);
                ClearSelections(nameof(SelectedGrouping));
            }
        }

        public string SelectedGenre
        {
            get { return _selectedGenre; }
            set
            {
                Set(ref _selectedGenre, value);
                ClearSelections(nameof(SelectedGenre));
            }
        }

        public string SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                Set(ref _selectedCountry, value);
                ClearSelections(nameof(SelectedCountry));
            }
        }

        public State SelectedState
        {
            get { return _selectedState; }
            set
            {
                Set(ref _selectedState, value);
                ClearSelections(nameof(SelectedState));
            }
        }

        public City SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                Set(ref _selectedCity, value);
                ClearSelections(nameof(SelectedCity));
            }
        }

        public string SelectedTag
        {
            get { return _selectedTag; }
            set
            {
                Set(ref _selectedTag, value);
                ClearSelections(nameof(SelectedTag));
            }
        }

        public string SelectedAddedDateOption
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
            Groupings = _library.Artists.Select(a => a.Grouping).Distinct().OrderBy(g => g).ToList();
            Genres = _library.Artists.Select(a => a.Genre).Distinct().OrderBy(g => g).ToList();
            Tags = _library.Artists.SelectMany(a => a.Tracks).SelectMany(t => t.Tags).Distinct().OrderBy(t => t).ToList();

            var cities = _library.Artists.Select(a => a.City).Distinct().ToList();
            Cities = GetCities(cities);
            States = GetStates(cities);
            Countries = GetCountries(cities);

            ClearSelections(string.Empty);
        }

        private static Dictionary<string, int> GetDateAddedOptions()
        {
            return new Dictionary<string, int>
            {
                { "in the Last Week",7 },
                { "in the Last 2 Weeks", 14 },
                { "in the Last 30 Days", 30 }
            };
        }

        private static List<City> GetCities(IEnumerable<City> cities)
        {
            return cities.Where(c => !string.IsNullOrEmpty(c?.Name))
                .Distinct()
                .OrderBy(c => c.Country)
                .ThenBy(c => c.State)
                .ThenBy(c => c.Name)
                .ToList();
        }
        private static List<State> GetStates(IEnumerable<City> cities)
        {
            return cities.Where(c => !string.IsNullOrEmpty(c?.State))
                .Select(c => new State(c))
                .Distinct()
                .OrderBy(s => s.Country)
                .ThenBy(s => s.Name)
                .ToList();
        }

        private static List<string> GetCountries(IEnumerable<City> cities)
        {
            return cities.Where(c => !string.IsNullOrEmpty(c?.Country))
                .Select(c => c.Country)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
        }

    }
}
