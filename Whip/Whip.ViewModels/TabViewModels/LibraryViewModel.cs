using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.ViewModels.TabViewModels.Library;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class LibraryViewModel : TabViewModelBase
    {
        private readonly Common.Singletons.Library _library;
        private readonly ILibrarySortingService _librarySortingService;
        private readonly IPlayRequestHandler _playRequestHandler;

        private bool _artistTypeAlbum;
        private bool _artistTypeTrack;
        private List<Artist> _artists;
        private List<string> _genres;
        private List<string> _groupings;
        private Artist _selectedArtist;
        private string _selectedGenre;
        private string _selectedGrouping;

        public LibraryViewModel(Common.Singletons.Library library, ILibrarySortingService librarySortingService, IPlayRequestHandler playRequestHandler,
            LibraryArtistViewModel artistViewModel, LibraryTracksViewModel tracksViewModel)
            :base(TabType.Library, IconType.Book, Resources.TabTitleLibrary)
        {
            _library = library;
            _librarySortingService = librarySortingService;
            _playRequestHandler = playRequestHandler;
            
            Artists = new List<Artist>();
            ArtistViewModel = artistViewModel;
            TracksViewModel = tracksViewModel;
            ArtistTypeAlbum = true;

            ShuffleArtistCommand = new RelayCommand<Artist>(OnShuffleArtist);

            _library.Updated += OnLibraryUpdated;
        }
        public LibraryArtistViewModel ArtistViewModel { get; }
        public LibraryTracksViewModel TracksViewModel { get; }
        public RelayCommand<Artist> ShuffleArtistCommand { get; }

        public List<Artist> Artists
        {
            get { return _artists; }
            private set { Set(ref _artists, value); }
        }

        public bool ArtistTypeAlbum
        {
            get { return _artistTypeAlbum; }
            set { SetArtistTypeAlbum(value); }
        }

        public bool ArtistTypeTrack
        {
            get { return _artistTypeTrack; }
            set { SetArtistTypeTrack(value); }
        }

        public List<string> Genres
        {
            get { return _genres; }
            set { Set(ref _genres, value); }
        }

        public List<string> Groupings
        {
            get { return _groupings; }
            set { Set(ref _groupings, value); }
        }
        
        public Artist SelectedArtist
        {
            get { return _selectedArtist; }
            set { SetSelectedArtist(value); }
        }

        public string SelectedGrouping
        {
            get { return _selectedGrouping; }
            set { SetSelectedGrouping(value); }
        }

        public string SelectedGenre
        {
            get { return _selectedGenre; }
            set { SetSelectedGenre(value); }
        }

        private static List<string> GetOptionList(IEnumerable<string> list)
        {
            var sortedList = list
                .Distinct()
                .Where(g => !string.IsNullOrEmpty(g))
                .OrderBy(g => g)
                .ToList();

            sortedList.Insert(0, string.Empty);

            return sortedList;
        }

        private void FilterArtists()
        {
            Artists = GetTrackOrAlbumArtists(true, true).ToList();

            if (SelectedArtist == null)
            {
                SelectedArtist = Artists.FirstOrDefault();
            }
        }

        private void FilterGenres()
        {
            Genres = GetGenres();

            if (SelectedGenre == null)
            {
                SelectedGenre = string.Empty;
            }

            FilterArtists();
        }

        private void FilterGroupings()
        {
            Groupings = GetGroupings();

            if (SelectedGrouping == null)
            {
                SelectedGrouping = string.Empty;
            }

            FilterGenres();
        }

        private List<string> GetGenres()
        {
            return GetOptionList(GetTrackOrAlbumArtists(true, false)
                .Select(a => a.Genre));
        }

        private List<string> GetGroupings()
        {
            return GetOptionList(GetTrackOrAlbumArtists(false, false)
                .Select(a => a.Grouping));
        }

        private IEnumerable<Artist> GetTrackOrAlbumArtists(bool filterByGrouping, bool filterByGenre)
        {
            var artists = _library.Artists
                .Where(a => 
                    ((ArtistTypeTrack && a.Tracks.Any()) || (ArtistTypeAlbum && a.Albums.Any()))
                    && (!filterByGrouping || string.IsNullOrEmpty(SelectedGrouping) || a.Grouping == SelectedGrouping)
                    && (!filterByGenre || string.IsNullOrEmpty(SelectedGenre) || a.Genre == SelectedGenre));

            return _librarySortingService.GetInDefaultOrder(artists);
        }

        private void OnLibraryUpdated(Track track)
        {
            var returnToArtist = SelectedArtist;

            SelectedArtist = null;

            if (track == null)
            {
                ResetSelections();
                FilterGroupings();
            }
            else
            {
                FilterArtists();
            }

            if (Artists.Contains(returnToArtist))
            {
                SelectedArtist = returnToArtist;
            }
        }

        private void OnShuffleArtist(Artist artist)
        {
            _playRequestHandler.PlayArtist(artist, SortType.Random);
        }

        private void ResetSelections()
        {
            SelectedGrouping = string.Empty;
            SelectedGenre = string.Empty;
            SelectedArtist = null;
        }

        private void SetArtistTypeAlbum(bool value)
        {
            Set(nameof(ArtistTypeAlbum), ref _artistTypeAlbum, value);
            UpdateArtistType();
        }

        private void SetArtistTypeTrack(bool value)
        {
            Set(nameof(ArtistTypeTrack), ref _artistTypeTrack, value);
            UpdateArtistType();
        }

        private void SetSelectedArtist(Artist value)
        {
            Set(nameof(SelectedArtist), ref _selectedArtist, value);
            ArtistViewModel.Artist = _selectedArtist;
            TracksViewModel.Artist = _selectedArtist;
        }

        private void SetSelectedGenre(string value)
        {
            Set(nameof(SelectedGenre), ref _selectedGenre, value);
            FilterArtists();
        }

        private void SetSelectedGrouping(string value)
        {
            Set(nameof(SelectedGrouping), ref _selectedGrouping, value);
            FilterGenres();
        }

        private void UpdateArtistType()
        {
            FilterGroupings();
            TracksViewModel.UpdateTracks(_artistTypeTrack);
        }
    }
}
