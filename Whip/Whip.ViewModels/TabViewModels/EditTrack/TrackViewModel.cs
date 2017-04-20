using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Whip.Common;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Common.Utilities;
using Whip.ViewModels.Utilities;
using static Whip.Resources.Resources;
using Whip.ViewModels.Windows;
using GalaSoft.MvvmLight.Messaging;
using Whip.ViewModels.Messages;
using Whip.Services.Interfaces;
using Whip.ViewModels.Validation;
using Whip.Common.Validation;
using System.Collections.ObjectModel;

namespace Whip.ViewModels.TabViewModels
{
    public class TrackViewModel : EditableViewModelBase
    {
        private const string AddNew = "- Add New...";

        private readonly IWebAlbumInfoService _webAlbumInfoService;
        private readonly Library _library;
        private readonly IMessenger _messenger;

        private List<City> _usedCities;
        private List<Artist> _artists;
        private List<Album> _albums;

        private List<string> _allTags;
        private List<string> _groupings;
        private List<string> _genres;
        private List<string> _countries;
        private List<string> _states;
        private List<string> _cities;

        private string _title;
        private string _year;
        private string _lyrics;
        private string _newTag;

        private Artist _artist;
        private Artist _albumArtist;
        private Album _album;
        
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

        public TrackViewModel(Library library, IMessenger messenger, IWebAlbumInfoService albumInfoService)
        {
            _webAlbumInfoService = albumInfoService;
            _library = library;
            _messenger = messenger;

            RemoveTagCommand = new RelayCommand<string>(OnRemoveTrack);

            GetArtworkFromUrlCommand = new RelayCommand(OnGetArtworkFromUrl);
            GetArtworkFromFileCommand = new RelayCommand(OnGetArtworkFromFile);
            GetArtworkFromWebCommand = new RelayCommand(OnGetArtworkFromWeb, CanGetArtworkFromWeb);
            ClearArtworkCommand = new RelayCommand(OnClearArtwork);

            TestWebsiteCommand = new RelayCommand(OnTestWebsite, CanTestWebsite);
            TestFacebookCommand = new RelayCommand(OnTestFacebook, CanTestFacebook);
            TestTwitterCommand = new RelayCommand(OnTestTwitter, CanTestTwitter);
            TestWikipediaCommand = new RelayCommand(OnTestWikipedia, CanTestWikipedia);
            TestLastFmCommand = new RelayCommand(OnTestLastFm, CanTestLastFm);
        }

        private void OnRemoveTrack(string tag)
        {
            Tags.Remove(tag);
        }

        private void OnClearArtwork()
        {
            AlbumArtwork = null;
        }

        private async void OnGetArtworkFromWeb()
        {
            AlbumArtwork = await _webAlbumInfoService.GetArtworkUrl(AlbumArtistName, AlbumTitle);
        }

        public bool ExistingArtistSelected => Artist != null && Artist.Name != AddNew;
        public bool ExistingAlbumArtistSelected => AlbumArtist != null && AlbumArtist.Name != AddNew;
        public bool ExistingAlbumSelected => Album != null && Album.Title != AddNew;

        private void OnGetArtworkFromFile()
        {
            var fileDialogRequest = new ShowFileDialogRequest(FileType.Images);
            _messenger.Send(fileDialogRequest);
            var result = fileDialogRequest.Result;

            if (result != null)
            {
                AlbumArtwork = result;
            }
        }

        private bool CanGetArtworkFromWeb()
        {
            return !string.IsNullOrEmpty(AlbumArtistName) && !string.IsNullOrEmpty(AlbumTitle);
        }

        private void OnGetArtworkFromUrl()
        {
            var enterUrlModel = new EnterStringViewModel(_messenger, "Get Artwork", "Enter the URL for the artwork below", UrlValidation.IsValidArtworkUrl, "The value entered is not a valid image URL");
            _messenger.Send(new ShowDialogMessage(enterUrlModel));
            var result = enterUrlModel.Result;

            if (result != null)
            {
                AlbumArtwork = result;
            }
        }

        public RelayCommand<string> RemoveTagCommand { get; private set; }

        public RelayCommand GetArtworkFromUrlCommand { get; private set; }
        public RelayCommand GetArtworkFromFileCommand { get; private set; }
        public RelayCommand GetArtworkFromWebCommand { get; private set; }
        public RelayCommand ClearArtworkCommand { get; private set; }

        public RelayCommand TestWebsiteCommand { get; private set; }
        public RelayCommand TestFacebookCommand { get; private set; }
        public RelayCommand TestTwitterCommand { get; private set; }
        public RelayCommand TestWikipediaCommand { get; private set; }
        public RelayCommand TestLastFmCommand { get; private set; }

        public string ArtistFacebookUrl => string.Format(FacebookUrl, ArtistFacebook);
        public string ArtistTwitterUrl => string.Format(TwitterUrl, ArtistTwitter);
        public string ArtistWikipediaUrl => string.Format(WikipediaUrl, ArtistWikipedia);
        public string ArtistLastFmUrl => string.Format(LastFmUrl, ArtistLastFm);

        public List<string> AllTags
        {
            get { return _allTags; }
            set { Set(ref _allTags, value); }
        }

        public List<Artist> Artists
        { 
            get { return _artists; }
            set { Set(ref _artists, value); }
        }

        public List<Album> Albums
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

        public Artist Artist
        {
            get { return _artist; }
            set
            {
                SetModified(nameof(Artist), ref _artist, value);
                PopulateArtistDetails();
            }
        }

        public Album Album
        {
            get { return _album; }
            set
            {
                SetModified(nameof(Album), ref _album, value);
                PopulateAlbumDetails();
            }
        }

        public Artist AlbumArtist
        {
            get { return _albumArtist; }
            set
            {
                SetModified(nameof(AlbumArtist), ref _albumArtist, value);
                PopulateAlbumArtistDetails();
            }
        }

        [Required]
        [MaxLength(TrackValidation.MaxLengthTrackTitle, ErrorMessageResourceName = nameof(MaxLengthErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        public string Title
        {
            get { return _title; }
            set { SetModified(nameof(Title), ref _title, value); }
        }

        [Required]
        [Year]
        [Display(Name = "Track Year")]
        public string Year
        {
            get { return _year; }
            set { SetModified(nameof(Year), ref _year, value); }
        }

        [MaxLength(TrackValidation.MaxLengthLyrics, ErrorMessageResourceName = nameof(MaxLengthErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        public string Lyrics
        {
            get { return _lyrics; }
            set { SetModified(nameof(Lyrics), ref _lyrics, value); }
        }

        public string AlbumArtwork
        {
            get { return _albumArtwork; }
            set { SetModified(nameof(AlbumArtwork), ref _albumArtwork, value); }
        }

        [Required]
        [Year]
        [Display(Name = "Album Year")]
        public string AlbumYear
        {
            get { return _albumYear; }
            set { SetModified(nameof(AlbumYear), ref _albumYear, value); }
        }

        [Required]
        [Display(Name = "Release Type")]
        public ReleaseType AlbumReleaseType
        {
            get { return _albumReleaseType; }
            set { SetModified(nameof(AlbumReleaseType), ref _albumReleaseType, value); }
        }

        [Required]
        [MaxLength(TrackValidation.MaxLengthArtistName, ErrorMessageResourceName = nameof(MaxLengthErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "Album Artist Name")]
        public string AlbumArtistName
        {
            get { return _albumArtistName; }
            set
            {
                SetModified(nameof(AlbumArtistName), ref _albumArtistName, value);
                SyncArtistNames(_albumArtistName);
            }
        }

        private bool _syncingArtistNames = false;

        private void SyncArtistNames(string latestName)
        {
            if (!_syncingArtistNames && ExistingAlbumArtistSelected && ExistingArtistSelected && Artist == AlbumArtist && ArtistName != AlbumArtistName)
            {
                _syncingArtistNames = true;
                ArtistName = latestName;
                AlbumArtistName = latestName;
                _syncingArtistNames = false;
            }
        }

        [Required]
        [MaxLength(TrackValidation.MaxLengthAlbumTitle, ErrorMessageResourceName = nameof(MaxLengthErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "Album Title")]
        public string AlbumTitle
        {
            get { return _albumTitle; }
            set { SetModified(nameof(AlbumTitle), ref _albumTitle, value); }
        }

        [Required]
        [MaxLength(TrackValidation.MaxLengthArtistName, ErrorMessageResourceName = nameof(MaxLengthErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "Artist Name")]
        public string ArtistName
        {
            get { return _artistName; }
            set
            {
                SetModified(nameof(ArtistName), ref _artistName, value);
                SyncArtistNames(_artistName);
            }
        }

        [Required]
        [MaxLength(TrackValidation.MaxLengthGrouping, ErrorMessageResourceName = nameof(MaxLengthErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "Grouping")]
        public string ArtistGrouping
        {
            get { return _artistGrouping; }
            set { SetModified(nameof(ArtistGrouping), ref _artistGrouping, value); }
        }

        [Required]
        [MaxLength(TrackValidation.MaxLengthGenre, ErrorMessageResourceName = nameof(MaxLengthErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "Genre")]
        public string ArtistGenre
        {
            get { return _artistGenre; }
            set { SetModified(nameof(ArtistGenre), ref _artistGenre, value); }
        }

        [MaxLength(TrackValidation.MaxLengthCountry, ErrorMessageResourceName = nameof(MaxLengthErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "Country")]
        public string ArtistCountry
        {
            get { return _artistCountry; }
            set
            {
                SetModified(nameof(ArtistCountry), ref _artistCountry, value);
                PopulateStateList();
            }
        }

        [MaxLength(TrackValidation.MaxLengthState, ErrorMessageResourceName = nameof(MaxLengthErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "State")]
        public string ArtistState
        {
            get { return _artistState; }
            set
            {
                SetModified(nameof(ArtistState), ref _artistState, value);
                PopulateCityList();
            }
        }

        [MaxLength(TrackValidation.MaxLengthCity, ErrorMessageResourceName = nameof(MaxLengthErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "City")]
        public string ArtistCity
        {
            get { return _artistCity; }
            set { SetModified(nameof(ArtistCity), ref _artistCity, value); }
        }

        [NullableUrl]
        [Display(Name = "Website")]
        public string ArtistWebsite
        {
            get { return _artistWebsite; }
            set
            {
                SetModified(nameof(ArtistWebsite), ref _artistWebsite, value);
                TestWebsiteCommand.RaiseCanExecuteChanged();
            }
        }

        [TwitterUsername]
        [Display(Name = "Twitter Username")]
        public string ArtistTwitter
        {
            get { return _artistTwitter; }
            set
            {
                SetModified(nameof(ArtistTwitter), ref _artistTwitter, value);
                TestTwitterCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(ArtistTwitterUrl));
            }
        }

        [FacebookUsername]
        [Display(Name = "Facebook Username")]
        public string ArtistFacebook
        {
            get { return _artistFacebook; }
            set
            {
                SetModified(nameof(ArtistFacebook), ref _artistFacebook, value);
                TestFacebookCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(ArtistFacebookUrl));
            }
        }

        [PartialUrlString]
        [Display(Name = "Artist Last.FM page")]
        public string ArtistLastFm
        {
            get { return _artistLastFm; }
            set
            {
                SetModified(nameof(ArtistLastFm), ref _artistLastFm, value);
                TestLastFmCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(ArtistLastFmUrl));
            }
        }

        [PartialUrlString]
        [Display(Name = "Artist Wikipedia page")]
        public string ArtistWikipedia
        {
            get { return _artistWikipedia; }
            set
            {
                SetModified(nameof(ArtistWikipedia), ref _artistWikipedia, value);
                TestWikipediaCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(ArtistWikipediaUrl));
            }
        }

        [Required]
        [TrackNo]
        [Display(Name = "Track No")]
        public int? TrackNo
        {
            get { return _trackNo; }
            set { SetModified(nameof(TrackNo), ref _trackNo, value); }
        }

        [Required]
        [Range(TrackValidation.MinTrackCount, TrackValidation.MaxTrackCount, ErrorMessageResourceName = nameof(RangeErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "Track Count")]
        public int? TrackCount
        {
            get { return _trackCount; }
            set { SetModified(nameof(TrackCount), ref _trackCount, value); }
        }

        [Required]
        [DiscNo]
        [Display(Name = "Disc No")]
        public int? DiscNo
        {
            get { return _discNo; }
            set
            {
                SetModified(nameof(DiscNo), ref _discNo, value);
                PopulateDiscDetails();
            }
        }

        [Required]
        [Range(TrackValidation.MinDiscCount, TrackValidation.MaxDiscCount, ErrorMessageResourceName = nameof(RangeErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "Disc Count")]
        public int? DiscCount
        {
            get { return _discCount; }
            set { SetModified(nameof(DiscCount), ref _discCount, value); }
        }

        public ObservableCollection<string> Tags { get; set; }

        [TrackTag]
        public string NewTag
        {
            get { return _newTag; }
            set
            {
                SetModified(nameof(NewTag), ref _newTag, value);
                if (!string.IsNullOrEmpty(value) && TrackTagAttribute.Validate(value, this))
                {
                    Tags.Add(value);
                    SetModified(nameof(NewTag), ref _newTag, string.Empty);
                }
            }
        }

        private bool CanTestWebsite()
        {
            return !string.IsNullOrEmpty(ArtistWebsite) && string.IsNullOrEmpty(this[nameof(ArtistWebsite)]);
        }

        private void OnTestWebsite()
        {
            Hyperlink.Go(ArtistWebsite);
        }

        private bool CanTestFacebook()
        {
            return !string.IsNullOrEmpty(ArtistFacebook) && string.IsNullOrEmpty(this[nameof(ArtistFacebook)]);
        }

        private void OnTestFacebook()
        {
            Hyperlink.Go(ArtistFacebookUrl);
        }

        private bool CanTestTwitter()
        {
            return !string.IsNullOrEmpty(ArtistTwitter) && string.IsNullOrEmpty(this[nameof(ArtistTwitter)]);
        }

        private void OnTestTwitter()
        {
            Hyperlink.Go(ArtistTwitterUrl);
        }

        private bool CanTestWikipedia()
        {
            return !string.IsNullOrEmpty(ArtistWikipedia) && string.IsNullOrEmpty(this[nameof(ArtistWikipedia)]);
        }

        private void OnTestWikipedia()
        {
            Hyperlink.Go(ArtistWikipediaUrl);
        }

        private bool CanTestLastFm()
        {
            return !string.IsNullOrEmpty(ArtistLastFm) && string.IsNullOrEmpty(this[nameof(ArtistLastFm)]);
        }

        private void OnTestLastFm()
        {
            Hyperlink.Go(ArtistLastFmUrl);
        }

        public void Populate(Track track)
        {
            PopulateOptionLists();

            Title = track.Title;
            Year = track.Year;
            Lyrics = track.Lyrics;
            TrackNo = track.TrackNo;
            Tags = new ObservableCollection<string>(track.Tags);

            Artist = track.Artist;
            AlbumArtist = track.Disc.Album.Artist;
            Album = track.Disc.Album;

            DiscNo = track.Disc.DiscNo;

            Modified = false;
        }

        private void PopulateDiscDetails()
        {
            var album = Album?.Title == AddNew ? null : Album;

            var disc = album?.Discs.SingleOrDefault(d => d.DiscNo == DiscNo);
            
            if (disc != null)
            {
                TrackCount = disc.TrackCount;
            }
        }

        private void PopulateAlbumArtistDetails()
        {
            var albumArtist = AlbumArtist?.Name == AddNew ? null : AlbumArtist;

            AlbumArtistName = albumArtist?.Name ?? string.Empty;

            Albums = albumArtist == null
                ? new List<Album>()
                : AlbumArtist.Albums
                    .OrderBy(a => a.Title)
                    .ToList();

            Albums.Insert(0, new Album { Title = AddNew });

            if (Albums.Count == 1)
            {
                Album = Albums.First();
            }

            PopulateAlbumDetails();
        }

        private void PopulateAlbumDetails()
        {
            var album = Album?.Title == AddNew ? null : Album;

            AlbumTitle = album?.Title ?? string.Empty;
            AlbumReleaseType = album?.ReleaseType ?? EnumHelpers.GetDefaultValue<ReleaseType>();
            AlbumArtwork = album?.Artwork ?? string.Empty;
            AlbumYear = album?.Year ?? string.Empty;
            DiscCount = album?.DiscCount ?? 1;

            PopulateDiscDetails();
        }

        private void PopulateArtistDetails()
        {
            var artist = Artist?.Name == AddNew ? null : Artist;

            ArtistName = artist?.Name ?? string.Empty;
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

            Artists = new List<Artist>(artists);
            Artists.Insert(0, new Artist { Name = AddNew });

            Groupings = artists.Select(a => a.Grouping).Distinct().OrderBy(g => g).ToList();

            Genres = artists.Select(a => a.Genre).Distinct().OrderBy(g => g).ToList();

            AllTags = artists.SelectMany(a => a.Tracks).SelectMany(t => t.Tags).Distinct().OrderBy(t => t).ToList();

            _usedCities = artists.Select(a => a.City).ToList();

            Countries = _usedCities.Select(c => c.Country).Distinct().OrderBy(c => c).ToList();
        }
    }
}
