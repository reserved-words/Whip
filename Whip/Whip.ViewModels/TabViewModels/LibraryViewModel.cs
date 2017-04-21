using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Linq;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;
using Whip.ViewModels.TabViewModels.Library;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class LibraryViewModel : TabViewModelBase
    {
        private readonly Common.Singletons.Library _library;
        private readonly ILibrarySortingService _librarySortingService;
        private readonly IMessenger _messenger;

        private bool _artistTypeAlbum;
        private bool _artistTypeTrack;
        private List<Artist> _artists;
        private List<string> _genres;
        private List<string> _groupings;
        private Artist _selectedArtist;
        private ArtistViewModel _selectedArtistViewModel;
        private string _selectedGenre;
        private string _selectedGrouping;

        public LibraryViewModel(Common.Singletons.Library library, IMessenger messenger, ITrackFilterService trackFilterService,
            ILibrarySortingService librarySortingService)
            :base(TabType.Library, IconType.Book, "Library")
        {
            Artists = new List<Artist>();
            SelectedArtistViewModel = new ArtistViewModel(trackFilterService, messenger);

            _library = library;
            _librarySortingService = librarySortingService;
            _messenger = messenger;

            _library.Updated += OnLibraryUpdated;
            
            PlayAlbumCommand = new RelayCommand<Album>(OnPlayAlbum);
            ShuffleArtistCommand = new RelayCommand<Artist>(OnShuffleArtist);

            ArtistTypeAlbum = true;
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
            }
        }

        public bool ArtistTypeTrack
        {
            get { return _artistTypeTrack; }
            set
            {
                Set(ref _artistTypeTrack, value);
                FilterGroupings();
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

        public RelayCommand<Album> PlayAlbumCommand { get; private set; }
        public RelayCommand<Artist> ShuffleArtistCommand { get; private set; }

        public Artist SelectedArtist
        {
            get { return _selectedArtist; }
            set
            {
                Set(ref _selectedArtist, value);
                SelectedArtistViewModel.Artist = _selectedArtist;
            }
        }

        public ArtistViewModel SelectedArtistViewModel
        {
            get { return _selectedArtistViewModel; }
            set { Set(ref _selectedArtistViewModel, value); }
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

        private void OnPlayAlbum(Album album)
        {
            _messenger.Send(new PlayAlbumMessage(album, SortType.Ordered));
        }

        private void OnShuffleArtist(Artist artist)
        {
            _messenger.Send(new PlayArtistMessage(artist, SortType.Random));
        }
    }
}
