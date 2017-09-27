using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
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
        private LibraryArtistViewModel _artistViewModel;
        private LibraryTracksViewModel _tracksViewModel;
        private string _selectedGenre;
        private string _selectedGrouping;

        public LibraryViewModel(Common.Singletons.Library library, IMessenger messenger, ILibrarySortingService librarySortingService, 
            IArtistInfoService webArtistInfoService, IImageProcessingService imageProcessingService, TrackContextMenuViewModel trackContextMenu, 
            IConfigSettings configSettings, IPlayRequestHandler playRequestHandler, ITrackFilterService trackFilterService)
            :base(TabType.Library, IconType.Book, Resources.TabTitleLibrary)
        {
            _library = library;
            _librarySortingService = librarySortingService;
            _playRequestHandler = playRequestHandler;
            
            Artists = new List<Artist>();
            ArtistViewModel = new LibraryArtistViewModel(webArtistInfoService, imageProcessingService, configSettings);
            TracksViewModel = new LibraryTracksViewModel(trackContextMenu, playRequestHandler, trackFilterService);
            ArtistTypeAlbum = true;

            ShuffleArtistCommand = new RelayCommand<Artist>(OnShuffleArtist);

            _library.Updated += OnLibraryUpdated;
        }

        public List<Artist> Artists
        {
            get { return _artists; }
            private set { Set(ref _artists, value); }
        }

        public bool ArtistTypeAlbum
        {
            get { return _artistTypeAlbum; }
            set
            {
                Set(ref _artistTypeAlbum, value);
                FilterGroupings();
                TracksViewModel.UpdateDisplayTracks(!_artistTypeAlbum);
            }
        }

        public bool ArtistTypeTrack
        {
            get { return _artistTypeTrack; }
            set
            {
                Set(ref _artistTypeTrack, value);
                FilterGroupings();
                TracksViewModel.UpdateDisplayTracks(_artistTypeTrack);
            }
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

        public RelayCommand<Artist> ShuffleArtistCommand { get; private set; }

        public Artist SelectedArtist
        {
            get { return _selectedArtist; }
            set
            {
                Set(ref _selectedArtist, value);
                ArtistViewModel.Artist = _selectedArtist;
                TracksViewModel.Artist = _selectedArtist;
            }
        }

        public LibraryArtistViewModel ArtistViewModel
        {
            get { return _artistViewModel; }
            set { Set(ref _artistViewModel, value); }
        }

        public LibraryTracksViewModel TracksViewModel
        {
            get { return _tracksViewModel; }
            set { Set(ref _tracksViewModel, value); }
        }

        public string SelectedGrouping
        {
            get { return _selectedGrouping; }
            set
            {
                Set(ref _selectedGrouping, value);
                FilterGenres();
            }
        }

        public string SelectedGenre
        {
            get { return _selectedGenre; }
            set
            {
                Set(ref _selectedGenre, value);
                FilterArtists();
            }
        }

        private void OnLibraryUpdated(Track track)
        {
            var returnToArtist = SelectedArtist;

            SelectedArtist = null;

            if (track == null)
            {
                SelectedGrouping = string.Empty;
                SelectedGenre = string.Empty;
                SelectedArtist = null;

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

        private void FilterGroupings()
        {
            var groupings = GetTrackOrAlbumArtists(false, false)
                .Select(a => a.Grouping)
                .Distinct()
                .Where(g => !string.IsNullOrEmpty(g))
                .OrderBy(g => g)
                .ToList();

            groupings.Insert(0, string.Empty);

            Groupings = groupings;

            if (SelectedGrouping == null)
            {
                SelectedGrouping = string.Empty;
            }

            FilterGenres();
        }

        private void FilterGenres()
        {
            var genres = GetTrackOrAlbumArtists(true, false)
                .Select(a => a.Genre)
                .Distinct()
                .Where(g => !string.IsNullOrEmpty(g))
                .OrderBy(g => g)
                .ToList();

            genres.Insert(0, string.Empty);

            Genres = genres;

            if (SelectedGenre == null)
            {
                SelectedGenre = string.Empty;
            }

            FilterArtists();
        }

        private void FilterArtists()
        {
            Artists = GetTrackOrAlbumArtists(true, true).ToList();

            if (SelectedArtist == null)
            {
                SelectedArtist = Artists.FirstOrDefault();
            }
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

        private void OnShuffleArtist(Artist artist)
        {
            _playRequestHandler.PlayArtist(artist, SortType.Random);
        }
    }
}
