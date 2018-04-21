using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly IPlaylistRepository _repository;
        private readonly ITrackSearchService _trackSearchService;
        private readonly PlaylistsViewModel _parent;
        
        public StandardPlaylistsViewModel(PlaylistsViewModel parent, Common.Singletons.Library library, IMessenger messenger, IPlayRequestHandler playRequestHandler,
            ITrackSearchService trackSearchService, IPlaylistRepository repository)
        {
            _library = library;
            _messenger = messenger;
            _playRequestHandler = playRequestHandler;
            _trackSearchService = trackSearchService;
            _repository = repository;
            _parent = parent;

            PlayCommand = new RelayCommand<StandardFilterViewModel>(OnPlay);
            FavouriteCommand = new RelayCommand<StandardFilterViewModel>(OnFavourite);

            Filters = new ObservableCollection<StandardFilterViewModel>();
        }

        public RelayCommand<StandardFilterViewModel> PlayCommand { get; }
        public RelayCommand<StandardFilterViewModel> FavouriteCommand { get; }
        
        public ObservableCollection<StandardFilterViewModel> Filters { get; }
        
        //private void ClearSelections(string except)
        //{
        //    if (except != nameof(SelectedGrouping))
        //    {
        //        _selectedGrouping = null;
        //    }

        //    if (except != nameof(SelectedGenre))
        //    {
        //        _selectedGenre = null;
        //    }

        //    if (except != nameof(SelectedCountry))
        //    {
        //        _selectedCountry = null;
        //    }

        //    if (except != nameof(SelectedState))
        //    {
        //        _selectedState = null;
        //    }

        //    if (except != nameof(SelectedCity))
        //    {
        //        _selectedCity = null;
        //    }

        //    if (except != nameof(SelectedTag))
        //    {
        //        _selectedTag = null;
        //    }

        //    if (except != nameof(SelectedDateAddedOption))
        //    {
        //        _selectedDateAddedOption = null;
        //    }

        //    RaisePropertyChanged(nameof(SelectedGrouping));
        //    RaisePropertyChanged(nameof(SelectedGenre));
        //    RaisePropertyChanged(nameof(SelectedCountry));
        //    RaisePropertyChanged(nameof(SelectedState));
        //    RaisePropertyChanged(nameof(SelectedCity));
        //    RaisePropertyChanged(nameof(SelectedTag));
        //    RaisePropertyChanged(nameof(SelectedDateAddedOption));
        //}

        public void Update(List<QuickPlaylist> favourites)
        {
            var cities = _library.Artists.Select(a => a.City).Distinct().ToList();

            Filters.Add(new StandardFilterViewModel("Grouping:", GetGroupings(_library.Artists, favourites)));
            Filters.Add(new StandardFilterViewModel("Genre:", GetGenres(_library.Artists, favourites)));
            Filters.Add(new StandardFilterViewModel("Country:", GetCountries(cities, favourites)));
            Filters.Add(new StandardFilterViewModel("State:", GetStates(cities, favourites)));
            Filters.Add(new StandardFilterViewModel("City:", GetCities(cities, favourites)));
            Filters.Add(new StandardFilterViewModel("Tag:", GetTags(_library.Artists.SelectMany(a => a.Tracks), favourites)));
            Filters.Add(new StandardFilterViewModel("Added:", GetDateAddedOptions(favourites)));

            //ClearSelections(string.Empty);
        }

        private void OnPlay(StandardFilterViewModel filter)
        {
            if (filter.SelectedPlaylist == null)
            {
                _messenger.Send(new ShowDialogMessage(_messenger, MessageType.Error, "Auto Playlist Error", "No option selected"));
                return;
            }

            Play(filter.SelectedPlaylist);
        }

        private void OnFavourite(StandardFilterViewModel filter)
        {
            if (filter.SelectedPlaylist == null)
                return;

            filter.SelectedPlaylist.Favourite = !filter.SelectedPlaylist.Favourite;
            _repository.Save(filter.SelectedPlaylist);
            _parent.OnFavouritePlaylistsUpdated();
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

        private static List<QuickPlaylist> GetDateAddedOptions(List<QuickPlaylist> favourites)
        {
            return new List<QuickPlaylist>
            {
                CreatePlaylist(favourites, "in the last week", FilterType.DateAdded, "7"),
                CreatePlaylist(favourites, "in the last 2 weeks", FilterType.DateAdded, "14"),
                CreatePlaylist(favourites, "in the last 30 days", FilterType.DateAdded, "30"),
                CreatePlaylist(favourites, "in the last year", FilterType.DateAdded, "365")
            };
        }

        private static List<QuickPlaylist> GetCities(IEnumerable<City> cities, List<QuickPlaylist> favourites)
        {
            return cities.Where(c => !string.IsNullOrEmpty(c?.Name))
                .Distinct()
                .OrderBy(c => c.Country)
                .ThenBy(c => c.State)
                .ThenBy(c => c.Name)
                .Select(c => CreatePlaylist(favourites, c.CityStateDescription, FilterType.City, c.Name, c.State, c.Country))
                .ToList();
        }
        private static List<QuickPlaylist> GetStates(IEnumerable<City> cities, List<QuickPlaylist> favourites)
        {
            return cities.Where(c => !string.IsNullOrEmpty(c?.State))
                .Select(c => new State(c))
                .Distinct()
                .OrderBy(s => s.Country)
                .ThenBy(s => s.Name)
                .Select(s => CreatePlaylist(favourites, s.Name, FilterType.State, s.Name, s.Country))
                .ToList();
        }

        private static List<QuickPlaylist> GetCountries(IEnumerable<City> cities, List<QuickPlaylist> favourites)
        {
            return cities.Where(c => !string.IsNullOrEmpty(c?.Country))
                .Select(c => c.Country)
                .Distinct()
                .OrderBy(c => c)
                .Select(c => CreatePlaylist(favourites, c, FilterType.Country, c))
                .ToList();
        }

        private static List<QuickPlaylist> GetTags(IEnumerable<Track> tracks, List<QuickPlaylist> favourites)
        {
            return tracks
                .SelectMany(t => t.Tags)
                .Distinct()
                .OrderBy(t => t)
                .Select(t => CreatePlaylist(favourites, t, FilterType.Tag, t))
                .ToList();
        }

        private static List<QuickPlaylist> GetGenres(IEnumerable<Artist> artists, List<QuickPlaylist> favourites)
        {
            return artists
                .Select(a => a.Genre)
                .Distinct()
                .OrderBy(g => g)
                .Select(g => CreatePlaylist(favourites, g, FilterType.Genre, g))
                .ToList();
        }

        private static List<QuickPlaylist> GetGroupings(IEnumerable<Artist> artists, List<QuickPlaylist> favourites)
        {
            return artists
                .Select(a => a.Grouping)
                .Distinct()
                .OrderBy(g => g)
                .Select(g => CreatePlaylist(favourites, g, FilterType.Grouping, g))
                .ToList();
        }

        private static QuickPlaylist CreatePlaylist(IEnumerable<QuickPlaylist> favourites, string title, FilterType filterType,
            params string[] filterValues)
        {
            var favourite = favourites.SingleOrDefault(pl => pl.FilterType == filterType && pl.FilterValues.SequenceEqual(filterValues));
            return new QuickPlaylist(favourite?.Id ?? 0, title, favourite != null, filterType, filterValues);
        }
    }
}
