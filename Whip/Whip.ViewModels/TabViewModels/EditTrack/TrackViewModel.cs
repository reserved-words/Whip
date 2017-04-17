using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Common.Utilities;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class TrackViewModel : ViewModelBase
    {
        private readonly Library _library;

        private bool _modified;

        private List<City> _usedCities;

        private List<string> _artists;
        private List<string> _albums;
        private List<string> _groupings;
        private List<string> _genres;
        private List<string> _countries;
        private List<string> _states;
        private List<string> _cities;

        private string _title;
        private string _year;
        private string _lyrics;

        private string _albumArtistName;
        private string _albumArtwork;
        private string _albumTitle;
        private string _albumYear;
        private ReleaseType _albumReleaseType;

        private string _artistName;
        private string _artistGrouping;
        private string _artistGenre;
        private string _artistCountry;
        private string _artistState;
        private string _artistCity;
        private string _artistWebsite;
        private string _artistTwitter;
        private string _artistFacebook;
        private string _artistLastFm;
        private string _artistWikipedia;

        private int? _trackNo;
        private int? _trackCount;
        private int? _discNo;
        private int? _discCount;

        public TrackViewModel(Library library)
        {
            _library = library;

            TestWebsiteCommand = new RelayCommand(OnTestWebsite, CanTestWebsite);
        }

        private bool CanTestWebsite()
        {
            return !string.IsNullOrEmpty(ArtistWebsite);
        }

        private void OnTestWebsite()
        {
            Hyperlink.Go(ArtistWebsite);
        }

        public event Action IsModified;

        public RelayCommand TestWebsiteCommand { get; private set; }

        public List<string> Artists
        { 
            get { return _artists; }
            set { Set(ref _artists, value); }
        }

        public List<string> Albums
        {
            get { return _albums; }
            set { Set(ref _albums, value); }
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

        public string Title
        {
            get { return _title; }
            set { SetModified(ref _title, value); }
        }

        public string Year
        {
            get { return _year; }
            set { SetModified(ref _year, value); }
        }

        public string Lyrics
        {
            get { return _lyrics; }
            set { SetModified(ref _lyrics, value); }
        }

        public string AlbumArtwork
        {
            get { return _albumArtwork; }
            set { SetModified(ref _albumArtwork, value); }
        }

        public string AlbumYear
        {
            get { return _albumYear; }
            set { SetModified(ref _albumYear, value); }
        }

        public ReleaseType AlbumReleaseType
        {
            get { return _albumReleaseType; }
            set { SetModified(ref _albumReleaseType, value); }
        }

        public string AlbumArtistName
        {
            get { return _albumArtistName; }
            set
            {
                SetModified(ref _albumArtistName, value);
                PopulateAlbumList();
                PopulateAlbumDetails();
            }
        }

        public string AlbumTitle
        {
            get { return _albumTitle; }
            set
            {
                SetModified(ref _albumTitle, value);
                PopulateAlbumDetails();
            }
        }

        public string ArtistName
        {
            get { return _artistName; }
            set
            {
                SetModified(ref _artistName, value);
                PopulateArtistDetails();
            }
        }

        public string ArtistGrouping
        {
            get { return _artistGrouping; }
            set { SetModified(ref _artistGrouping, value); }
        }

        public string ArtistGenre
        {
            get { return _artistGenre; }
            set { SetModified(ref _artistGenre, value); }
        }
        public string ArtistCountry
        {
            get { return _artistCountry; }
            set
            {
                SetModified(ref _artistCountry, value);
                PopulateStateList();
            }
        }
        public string ArtistState
        {
            get { return _artistState; }
            set
            {
                SetModified(ref _artistState, value);
                PopulateCityList();
            }
        }
        public string ArtistCity
        {
            get { return _artistCity; }
            set { SetModified(ref _artistCity, value); }
        }

        public string ArtistWebsite
        {
            get { return _artistWebsite; }
            set
            {
                SetModified(ref _artistWebsite, value);
                TestWebsiteCommand.RaiseCanExecuteChanged();
            }
        }

        public string ArtistTwitter
        {
            get { return _artistTwitter; }
            set { SetModified(ref _artistTwitter, value); }
        }

        public string ArtistFacebook
        {
            get { return _artistFacebook; }
            set { SetModified(ref _artistFacebook, value); }
        }

        public string ArtistLastFm
        {
            get { return _artistLastFm; }
            set { SetModified(ref _artistLastFm, value); }
        }

        public string ArtistWikipedia
        {
            get { return _artistWikipedia; }
            set { SetModified(ref _artistWikipedia, value); }
        }

        public int? TrackNo
        {
            get { return _trackNo; }
            set { SetModified(ref _trackNo, value); }
        }

        public int? TrackCount
        {
            get { return _trackCount; }
            set { SetModified(ref _trackCount, value); }
        }

        public int? DiscNo
        {
            get { return _discNo; }
            set
            {
                SetModified(ref _discNo, value);
                PopulateDiscDetails();
            }
        }

        public int? DiscCount
        {
            get { return _discCount; }
            set { SetModified(ref _discCount, value); }
        }

        public bool Modified
        {
            get { return _modified; }
            set { Set(ref _modified, value); }
        }

        public void UpdateTrack(Track track)
        {
            track.Title = Title;
            track.Year = Year;

            if (track.Artist.Name != ArtistName)
            {
                track.Artist.Tracks.Remove(track);

                var artist = _library.Artists.SingleOrDefault(a => a.Name == ArtistName);

                if (artist == null)
                {
                    artist = new Artist { Name = ArtistName };
                    _library.Artists.Add(artist);
                }

                track.Artist = artist;
                artist.Tracks.Add(track);
            }
            
            track.Artist.Grouping = ArtistGrouping;
            track.Artist.Genre = ArtistGenre;
            track.Artist.City = new City(ArtistCity, ArtistState, ArtistCountry);

            // etc
        }

        protected void SetModified<T>(ref T property, T value)
        {
            Set(ref property, value);
            if (!Modified)
            {
                Modified = true;
                IsModified?.Invoke();
            }
        }

        public void Populate(Track track)
        {
            PopulateOptionLists();

            Title = track.Title;
            Year = track.Year;
            Lyrics = track.Lyrics;
            TrackNo = track.TrackNo;

            ArtistName = track.Artist.Name;
            AlbumArtistName = track.Disc.Album.Artist.Name;
            AlbumTitle = track.Disc.Album.Title; PopulateAlbumDetails();
            DiscNo = track.Disc.DiscNo;

            Modified = false;
        }

        private void PopulateDiscDetails()
        {
            if (string.IsNullOrEmpty(AlbumArtistName) || string.IsNullOrEmpty(AlbumTitle) || !DiscNo.HasValue)
                return;

            var albumArtist = _library.Artists.SingleOrDefault(a => a.Name == AlbumArtistName);
            var album = albumArtist?.Albums.SingleOrDefault(a => a.Title == AlbumTitle);
            var disc = album?.Discs.SingleOrDefault(d => d.DiscNo == DiscNo);
            
            if (disc != null)
            {
                TrackCount = disc.TrackCount;
            }
        }

        private void PopulateAlbumDetails()
        {
            if (string.IsNullOrEmpty(AlbumArtistName) || string.IsNullOrEmpty(AlbumTitle))
                return;

            var albumArtist = _library.Artists.SingleOrDefault(a => a.Name == AlbumArtistName);
            var album = albumArtist?.Albums.SingleOrDefault(a => a.Title == AlbumTitle);

            AlbumReleaseType = album?.ReleaseType ?? EnumHelpers.GetDefaultValue<ReleaseType>();
            AlbumArtwork = album?.Artwork ?? string.Empty;
            AlbumYear = album?.Year ?? string.Empty;
            DiscCount = album?.DiscCount ?? 1;
        }

        private void PopulateArtistDetails()
        {
            if (string.IsNullOrEmpty(ArtistName))
                return;

            var artist = _library.Artists.SingleOrDefault(a => a.Name == ArtistName);
            
            ArtistGrouping = artist?.Grouping ?? string.Empty;
            ArtistGenre = artist?.Genre ?? string.Empty;
            ArtistCountry = artist?.City.Country ?? string.Empty;
            ArtistState = artist?.City.State ?? string.Empty;
            ArtistCity = artist?.City.Name ?? string.Empty;

            ArtistWebsite = artist?.Website ?? string.Empty;
            ArtistFacebook = artist?.Facebook ?? string.Empty;
            ArtistTwitter = artist?.Twitter ?? string.Empty;
            ArtistWikipedia = artist?.Wikipedia ?? string.Empty;
            ArtistLastFm = artist?.LastFm ?? string.Empty;
        }

        private void PopulateAlbumList()
        {
            Albums = string.IsNullOrEmpty(AlbumArtistName)
                ? new List<string>()
                : _library.Artists
                    .SingleOrDefault(a => a.Name == AlbumArtistName)?
                    .Albums
                    .Select(a => a.Title)
                    .OrderBy(a => a)
                    .ToList();
        }

        private void PopulateCityList()
        {
            Cities = _usedCities
                .Where(c => (string.IsNullOrEmpty(ArtistCountry) || c.Country == ArtistCountry)
                    && (string.IsNullOrEmpty(ArtistState) || c.State == ArtistState))
                .Select(c => c.Name)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
        }

        private void PopulateStateList()
        {
            States = _usedCities
                .Where(c => string.IsNullOrEmpty(ArtistCountry) || c.Country == ArtistCountry)
                .Select(c => c.State)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            PopulateCityList();
        }

        private void PopulateOptionLists()
        {
            var artists = _library.Artists.ToList();

            Artists = artists.Select(a => a.Name).ToList();

            Groupings = artists.Select(a => a.Grouping).Distinct().OrderBy(g => g).ToList();

            Genres = artists.Select(a => a.Genre).Distinct().OrderBy(g => g).ToList();

            _usedCities = artists.Select(a => a.City).ToList();

            Countries = _usedCities.Select(c => c.Country).Distinct().OrderBy(c => c).ToList();

            PopulateAlbumList();
        }
    }
}
