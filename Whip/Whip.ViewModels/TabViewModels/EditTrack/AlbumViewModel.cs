using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Whip.Common;
using Whip.Common.Model;
using Whip.Common.Utilities;
using Whip.Common.Validation;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;
using Whip.ViewModels.Utilities;
using Whip.ViewModels.Validation;
using Whip.ViewModels.Windows;
using static Whip.Resources.Resources;

namespace Whip.ViewModels.TabViewModels.EditTrack
{
    public class AlbumViewModel : EditableViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly IWebAlbumInfoService _webAlbumInfoService;
        private readonly EditTrackViewModel _parent;

        private List<Album> _albums;
        private List<Artist> _artists;

        private Artist _artist;
        private Album _album;

        private string _artistName;
        private string _artwork;
        private string _title;
        private string _year;
        private ReleaseType _releaseType;

        private int? _trackCount;
        private int? _discNo;
        private int? _discCount;
        private int? _trackNo;

        public AlbumViewModel(EditTrackViewModel parent, IMessenger messenger, IWebAlbumInfoService albumInfoService, List<Artist> artists, Track track)
        {
            _messenger = messenger;
            _parent = parent;
            _webAlbumInfoService = albumInfoService;

            Artists = artists;

            GetArtworkFromUrlCommand = new RelayCommand(OnGetArtworkFromUrl);
            GetArtworkFromFileCommand = new RelayCommand(OnGetArtworkFromFile);
            GetArtworkFromWebCommand = new RelayCommand(OnGetArtworkFromWeb, CanGetArtworkFromWeb);
            ClearArtworkCommand = new RelayCommand(OnClearArtwork);

            Populate(track);

            Modified = false;
        }

        public bool ExistingAlbumArtistSelected => Artist != null && Artist.Name != AddNew;
        public bool ExistingAlbumSelected => Album != null && Album.Title != AddNew;

        public RelayCommand GetArtworkFromUrlCommand { get; private set; }
        public RelayCommand GetArtworkFromFileCommand { get; private set; }
        public RelayCommand GetArtworkFromWebCommand { get; private set; }
        public RelayCommand ClearArtworkCommand { get; private set; }

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

        public Album Album
        {
            get { return _album; }
            set
            {
                SetModified(nameof(Album), ref _album, value);
                PopulateAlbumDetails();
            }
        }

        public Artist Artist
        {
            get { return _artist; }
            set
            {
                SetModified(nameof(Artist), ref _artist, value);
                PopulateAlbumArtistDetails();
            }
        }

        public string Artwork
        {
            get { return _artwork; }
            set { SetModified(nameof(Artwork), ref _artwork, value); }
        }

        [Required]
        [Year]
        [Display(Name = "Album Year")]
        public string Year
        {
            get { return _year; }
            set { SetModified(nameof(Year), ref _year, value); }
        }

        [Required]
        [Display(Name = "Release Type")]
        public ReleaseType ReleaseType
        {
            get { return _releaseType; }
            set { SetModified(nameof(ReleaseType), ref _releaseType, value); }
        }

        [Required]
        [MaxLength(TrackValidation.MaxLengthArtistName, ErrorMessageResourceName = nameof(MaxLengthErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "Album Artist Name")]
        public string ArtistName
        {
            get { return _artistName; }
            set
            {
                SetModified(nameof(ArtistName), ref _artistName, value);
                if (ExistingAlbumArtistSelected)
                {
                    _parent.SyncArtistNames(_artistName);
                }
            }
        }

        [Required]
        [MaxLength(TrackValidation.MaxLengthAlbumTitle, ErrorMessageResourceName = nameof(MaxLengthErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "Album Title")]
        public string Title
        {
            get { return _title; }
            set { SetModified(nameof(Title), ref _title, value); }
        }

        [Required]
        [Range(TrackValidation.MinDiscCount, TrackValidation.MaxDiscCount, ErrorMessageResourceName = nameof(RangeErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "Disc Count")]
        public int? DiscCount
        {
            get { return _discCount; }
            set { SetModified(nameof(DiscCount), ref _discCount, value); }
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
        [TrackNo]
        [Display(Name = "Track No")]
        public int? TrackNo
        {
            get { return _trackNo; }
            set { SetModified(nameof(TrackNo), ref _trackNo, value); }
        }

        private void PopulateAlbumDetails()
        {
            var album = Album?.Title == AddNew ? null : Album;

            Title = album?.Title ?? string.Empty;
            ReleaseType = album?.ReleaseType ?? EnumHelpers.GetDefaultValue<ReleaseType>();
            Artwork = album?.Artwork ?? string.Empty;
            Year = album?.Year ?? string.Empty;
            DiscCount = album?.DiscCount ?? 1;

            PopulateDiscDetails();
        }

        private void PopulateAlbumArtistDetails()
        {
            var albumArtist = Artist?.Name == AddNew ? null : Artist;

            ArtistName = albumArtist?.Name ?? string.Empty;

            Albums = albumArtist == null
                ? new List<Album>()
                : Artist.Albums
                    .OrderBy(a => a.Title)
                    .ToList();

            Albums.Insert(0, new Album { Title = AddNew });

            if (Albums.Count == 1)
            {
                Album = Albums.First();
            }

            PopulateAlbumDetails();
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

        private bool CanGetArtworkFromWeb()
        {
            return !string.IsNullOrEmpty(ArtistName) && !string.IsNullOrEmpty(Title);
        }

        private void OnClearArtwork()
        {
            Artwork = null;
        }

        private void OnGetArtworkFromFile()
        {
            var fileDialogRequest = new ShowFileDialogRequest(FileType.Images);
            _messenger.Send(fileDialogRequest);
            var result = fileDialogRequest.Result;

            if (result != null)
            {
                Artwork = result;
            }
        }

        private void OnGetArtworkFromUrl()
        {
            var enterUrlModel = new EnterStringViewModel(_messenger, "Get Artwork", "Enter the URL for the artwork below", UrlValidation.IsValidArtworkUrl, "The value entered is not a valid image URL");
            _messenger.Send(new ShowDialogMessage(enterUrlModel));
            var result = enterUrlModel.Result;

            if (result != null)
            {
                Artwork = result;
            }
        }

        private async void OnGetArtworkFromWeb()
        {
            Artwork = await _webAlbumInfoService.GetArtworkUrl(ArtistName, Title);
        }

        private void Populate(Track track)
        {
            Artist = track.Disc.Album.Artist;
            Album = track.Disc.Album;

            DiscNo = track.Disc.DiscNo;
            TrackNo = track.TrackNo;

            PopulateAlbumArtistDetails();
            PopulateAlbumDetails();
        }
    }
}
