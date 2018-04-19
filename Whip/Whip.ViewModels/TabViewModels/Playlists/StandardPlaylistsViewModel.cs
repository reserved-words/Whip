using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces.Singletons;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.TabViewModels.Playlists
{
    public class StandardPlaylistsViewModel : ViewModelBase
    {
        private readonly Common.Singletons.Library _library;
        private readonly IMessenger _messenger;
        private readonly IPlayRequestHandler _playRequestHandler;

        private Dictionary<string, Predicate<Track>> _dateAddedOptions;

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

        public StandardPlaylistsViewModel(Common.Singletons.Library library, IMessenger messenger, IPlayRequestHandler playRequestHandler)
        {
            _library = library;
            _messenger = messenger;
            _playRequestHandler = playRequestHandler;

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
            if (selectedOption == null)
            {
                _messenger.Send(new ShowDialogMessage(_messenger, MessageType.Error, "Auto Playlist Error", $"No {optionType} selected"));
                return false;
            }

            return true;
        }

        private void OnPlayRecentlyAdded()
        {
            if (!CheckOptionSelected(SelectedAddedDateOption, "time period"))
                return;

            var predicate = _dateAddedOptions[SelectedAddedDateOption];
            var tracks = _library.Artists.SelectMany(a => a.Tracks.Where(t => predicate(t))).ToList();

            if (!tracks.Any())
            {
                _messenger.Send(new ShowDialogMessage(_messenger, MessageType.Error, "Auto Playlist Error", "No tracks meet these criteria"));
                return;
            }

            _playRequestHandler.PlayCriteriaPlaylist(string.Format("Tracks Added {0}", SelectedAddedDateOption), tracks);
        }

        private void OnPlayTag()
        {
            if (!CheckOptionSelected(SelectedTag, "tag"))
                return;

            var tracks = _library.Artists.SelectMany(a => a.Tracks.Where(t => t.Tags.Contains(SelectedTag))).ToList();
            _playRequestHandler.PlayCriteriaPlaylist(SelectedTag, tracks);
        }

        private void OnPlayCity()
        {
            if (!CheckOptionSelected(SelectedCity, "city"))
                return;

            var tracks = _library.Artists.Where(a => a.City == SelectedCity).SelectMany(a => a.Tracks).ToList();
            _playRequestHandler.PlayCriteriaPlaylist(SelectedCity.Description, tracks);
        }

        private void OnPlayState()
        {
            if (!CheckOptionSelected(SelectedState, "state"))
                return;

            var tracks = _library.Artists.Where(a => a.City.State == SelectedState.Name && a.City.Country == SelectedState.Country).SelectMany(a => a.Tracks).ToList();
            _playRequestHandler.PlayCriteriaPlaylist(SelectedState.Description, tracks);
        }

        private void OnPlayCountry()
        {
            if (!CheckOptionSelected(SelectedCountry, "country"))
                return;

            var tracks = _library.Artists.Where(a => a.City.Country == SelectedCountry).SelectMany(a => a.Tracks).ToList();
            _playRequestHandler.PlayCriteriaPlaylist(SelectedCountry, tracks);
        }

        private void OnPlayGenre()
        {
            if (!CheckOptionSelected(SelectedGenre, "genre"))
                return;

            var tracks = _library.Artists.Where(a => a.Genre == SelectedGenre).SelectMany(a => a.Tracks).ToList();
            _playRequestHandler.PlayCriteriaPlaylist(SelectedGenre, tracks);
        }

        private void OnPlayGrouping()
        {
            if (!CheckOptionSelected(SelectedGrouping, "grouping"))
                return;

            var tracks = _library.Artists.Where(a => a.Grouping == SelectedGrouping).SelectMany(a => a.Tracks).ToList();

            _playRequestHandler.PlayCriteriaPlaylist(SelectedGrouping, tracks);
        }

        public RelayCommand PlayGroupingCommand { get; private set; }
        public RelayCommand PlayGenreCommand { get; private set; }
        public RelayCommand PlayCountryCommand { get; private set; }
        public RelayCommand PlayStateCommand { get; private set; }
        public RelayCommand PlayCityCommand { get; private set; }
        public RelayCommand PlayTagCommand { get; private set; }
        public RelayCommand PlayRecentlyAddedCommand { get; private set; }

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

        private Dictionary<string, Predicate<Track>> GetDateAddedOptions()
        {
            return new Dictionary<string, Predicate<Track>>
            {
                { "in the Last Week", t => t.File.DateCreated.AddDays(7) > DateTime.Now },
                { "in the Last 2 Weeks", t => t.File.DateCreated.AddDays(14) > DateTime.Now },
                { "in the Last 30 Days", t => t.File.DateCreated.AddDays(30) > DateTime.Now }
            };
        }

        private List<City> GetCities(List<City> cities)
        {
            return cities.Where(c => c != null && !string.IsNullOrEmpty(c.Name))
                .Distinct()
                .OrderBy(c => c.Country)
                .ThenBy(c => c.State)
                .ThenBy(c => c.Name)
                .ToList();
        }
        private List<State> GetStates(List<City> cities)
        {
            return cities.Where(c => c != null && !string.IsNullOrEmpty(c.State))
                .Select(c => new State(c))
                .Distinct()
                .OrderBy(s => s.Country)
                .ThenBy(s => s.Name)
                .ToList();
        }

        private List<string> GetCountries(List<City> cities)
        {
            return cities.Where(c => c != null && !string.IsNullOrEmpty(c.Country))
                .Select(c => c.Country)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
        }

    }
}
