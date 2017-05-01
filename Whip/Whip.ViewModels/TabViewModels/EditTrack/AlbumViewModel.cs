using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
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
        private readonly IImageProcessingService _imageProcessingService;
        private readonly IFileDialogService _fileDialogService;

        private readonly DiscViewModel _disc;

        private List<Album> _albums;
        private List<Artist> _artists;

        private Artist _artist;
        private Album _album;

        private string _artistName;
        private bool _artistSameAsTrackArtist;
        private string _title;
        private string _year;
        private ReleaseType _releaseType;
        private string _discCount;

        private byte[] _artworkBytes;
        private BitmapImage _artwork;
        private bool _loadingArtwork;

        public AlbumViewModel(DiscViewModel disc, IMessenger messenger, IWebAlbumInfoService albumInfoService,
            IImageProcessingService imageProcessingService, IFileDialogService fileDialogService, List<Artist> artists, Track track)
        {
            _fileDialogService = fileDialogService;
            _imageProcessingService = imageProcessingService;
            _messenger = messenger;
            _webAlbumInfoService = albumInfoService;

            _disc = disc;

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

        public bool ArtistSameAsTrackArtist
        {
            get { return _artistSameAsTrackArtist; }
            set
            {
                SetModified(nameof(ArtistSameAsTrackArtist), ref _artistSameAsTrackArtist, value);
                _disc.SyncArtists(value);
            }
        }

        public BitmapImage Artwork
        {
            get { return _artwork; }
            set { SetModified(nameof(Artwork), ref _artwork, value); }
        }

        public bool LoadingArtwork
        {
            get { return _loadingArtwork; }
            set { Set(ref _loadingArtwork, value); }
        }

        public void UpdateAlbum(Album album)
        {
            album.Title = Title;
            album.Year = Year;
            album.ReleaseType = ReleaseType;
            album.DiscCount = Convert.ToInt16(DiscCount);
            album.Artwork = _artworkBytes;
        }

        [Required(ErrorMessageResourceName = nameof(RequiredErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [Year]
        [Display(Name = "Album Year")]
        public string Year
        {
            get { return _year; }
            set { SetModified(nameof(Year), ref _year, value); }
        }

        [Required(ErrorMessageResourceName = nameof(RequiredErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "Release Type")]
        public ReleaseType ReleaseType
        {
            get { return _releaseType; }
            set { SetModified(nameof(ReleaseType), ref _releaseType, value); }
        }

        [ArtistName]
        [Display(Name = "Album Artist Name")]
        public string ArtistName
        {
            get { return _artistName; }
            set { SetModified(nameof(ArtistName), ref _artistName, value); }
        }

        [AlbumTitle]
        [Display(Name = "Album Title")]
        public string Title
        {
            get { return _title; }
            set { SetModified(nameof(Title), ref _title, value); }
        }

        [Required(ErrorMessageResourceName = nameof(RequiredErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [DiscCount]
        [Display(Name = "Disc Count")]
        public string DiscCount
        {
            get { return _discCount; }
            set
            {
                SetModified(nameof(DiscCount), ref _discCount, value);

                _disc.ValidateDiscNo();
            }
        }

        private void PopulateAlbumArtistDetails()
        {            
            ArtistName = string.Empty;
            
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

        private void PopulateAlbumDetails()
        {
            Title = string.Empty;

            var album = Album?.Title == AddNew ? null : Album;

            Title = album?.Title ?? string.Empty;
            ReleaseType = album?.ReleaseType ?? EnumHelpers.GetDefaultValue<ReleaseType>();
            Year = album?.Year ?? string.Empty;
            DiscCount = album?.DiscCount.ToString() ?? string.Empty;
            UpdateArtwork(album?.Artwork);

            _disc.PopulateDiscDetails();
        }

        private bool CanGetArtworkFromWeb()
        {
            return !string.IsNullOrEmpty(ArtistName) && !string.IsNullOrEmpty(Title);
        }

        private void OnClearArtwork()
        {
            UpdateArtwork(null);
        }

        private void OnGetArtworkFromFile()
        {
            var result = _fileDialogService.OpenFileDialog(FileType.Images);

            if (result == null)
                return;

            UpdateArtwork(_imageProcessingService.GetImageBytesFromFile(result));
        }

        private async void OnGetArtworkFromUrl()
        {
            var enterUrlModel = new EnterStringViewModel(_messenger, "Get Artwork", "Enter the URL for the artwork below", UrlValidation.IsValidArtworkUrl, "The value entered is not a valid image URL");
            _messenger.Send(new ShowDialogMessage(enterUrlModel));
            var result = enterUrlModel.Result;

            if (result == null)
                return;

            LoadingArtwork = true;
            UpdateArtwork(await GetImageBytesFromUrl(result));            
        }

        private async void OnGetArtworkFromWeb()
        {
            LoadingArtwork = true;
            var result = await _webAlbumInfoService.GetArtworkUrl(ArtistName, Title);

            if (string.IsNullOrEmpty(result))
            {
                _messenger.Send(new ShowDialogMessage(_messenger, MessageType.Info, "Artwork", "No artwork found for this album"));
            }

            UpdateArtwork(await GetImageBytesFromUrl(result));
        }

        private async Task<byte[]> GetImageBytesFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            var result = await _imageProcessingService.GetImageBytesFromUrl(url);

            if (result == null)
            {
                _messenger.Send(new ShowDialogMessage(_messenger, MessageType.Error, ImageErrorTitle, ImageInternetErrorText));
            }

            return result;
        }

        private void Populate(Track track)
        {
            Artist = track.Disc.Album.Artist;
            Album = track.Disc.Album;

            ArtistSameAsTrackArtist = Artist == track.Artist;

            PopulateAlbumArtistDetails();
            PopulateAlbumDetails();
        }

        private void UpdateArtwork(byte[] bytes)
        {
            _artworkBytes = bytes;
            Artwork = _imageProcessingService.GetImageFromBytes(bytes);
            LoadingArtwork = false;
        }
    }
}
