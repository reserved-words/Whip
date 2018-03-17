using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Whip.Common.Model;
using Whip.ViewModels.Utilities;
using Whip.ViewModels.Validation;
using Whip.Common.Validation;
using System.Collections.ObjectModel;
using static Whip.Common.Resources;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using Whip.Services.Interfaces;
using System;

namespace Whip.ViewModels.TabViewModels.EditTrack
{
    public class TrackViewModel : EditableViewModelBase
    {
        private const string InstrumentalLyrics = "(instrumental)";

        private List<string> _allTags;
        private string _title;
        private string _year;
        private string _lyrics;
        private string _newTag;
        private string _trackNo;
        private bool _instrumental;
        private bool _syncArtistNames;

        public TrackViewModel(IMessenger messenger, IAlbumInfoService albumInfoService, IImageProcessingService imageProcessingService,
            IWebBrowserService webBrowserService, IFileDialogService fileDialogService, List<Artist> artists, List<string> tags, Track track)
        {
            Artist = new ArtistViewModel(this, artists, track.Artist, webBrowserService);
            Disc = new DiscViewModel(this, messenger, albumInfoService, imageProcessingService, fileDialogService, artists, track);

            RemoveTagCommand = new RelayCommand<string>(OnRemoveTag);

            AllTags = tags;

            Populate(track);

            Modified = false;
        }

        public override string Error
        {
            get
            {
                var errorMessages = new List<string>
                {
                    base.Error,
                    Artist.Error,
                    Disc.Error,
                    Disc.Album.Error
                };

                return string.Join(Environment.NewLine, errorMessages.Where(e => !string.IsNullOrEmpty(e)));
            }
        }

        public ArtistViewModel Artist { get; }

        public DiscViewModel Disc { get; }

        public RelayCommand<string> RemoveTagCommand { get; }

        public List<string> AllTags
        {
            get { return _allTags; }
            set { Set(ref _allTags, value); }
        }

        [Required(ErrorMessageResourceName = nameof(RequiredErrorMessage), ErrorMessageResourceType = typeof(Common.Resources))]
        [MaxLength(TrackValidation.MaxLengthTrackTitle, ErrorMessageResourceName = nameof(MaxLengthErrorMessage), ErrorMessageResourceType = typeof(Common.Resources))]
        public string Title
        {
            get { return _title; }
            set { SetModified(nameof(Title), ref _title, value); }
        }

        [Required(ErrorMessageResourceName = nameof(RequiredErrorMessage), ErrorMessageResourceType = typeof(Common.Resources))]
        [Year]
        [Display(Name = "Track Year")]
        public string Year
        {
            get { return _year; }
            set { SetModified(nameof(Year), ref _year, value); }
        }

        [MaxLength(TrackValidation.MaxLengthLyrics, ErrorMessageResourceName = nameof(MaxLengthErrorMessage), ErrorMessageResourceType = typeof(Common.Resources))]
        public string Lyrics
        {
            get { return _lyrics; }
            set { SetModified(nameof(Lyrics), ref _lyrics, value); }
        }

        public bool Instrumental
        {
            get { return _instrumental; }
            set
            {
                SetModified(nameof(Instrumental), ref _instrumental, value);
                if (_instrumental)
                {
                    Lyrics = InstrumentalLyrics;
                }
                else if (Lyrics == InstrumentalLyrics)
                {
                    Lyrics = "";
                }
            }
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

        [Required(ErrorMessageResourceName = nameof(RequiredErrorMessage), ErrorMessageResourceType = typeof(Common.Resources))]
        [TrackNo]
        [Display(Name = "Track No")]
        public string TrackNo
        {
            get { return _trackNo; }
            set { SetModified(nameof(TrackNo), ref _trackNo, value); }
        }
        
        public void SyncArtists()
        {
            if (Artist == null || Disc == null || !_syncArtistNames)
                return;

            Disc.Album.Artist = Artist.Artist;
            Disc.Album.ArtistName = Artist.Name;
        }

        public void SyncArtists(bool sync)
        {
            _syncArtistNames = sync;

            SyncArtists();
        }

        public void UpdateTrack(Track track)
        {
            track.Title = Title;
            track.Year = Year;
            track.Tags = Tags.ToList();
            track.Lyrics = Lyrics;
            track.TrackNo = Convert.ToInt16(TrackNo);
            track.Instrumental = Instrumental;

            Artist.UpdateArtist(track);
            Disc.UpdateDisc(track);
        }

        public void ValidateTrackNo()
        {
            var trackNo = TrackNo;
            TrackNo = string.Empty;
            TrackNo = trackNo;
        }
        
        private void OnRemoveTag(string tag)
        {
            Tags.Remove(tag);
            Modified = true;
        }

        private void Populate(Track track)
        {
            Title = track.Title;
            Year = track.Year;
            Lyrics = track.Lyrics;
            TrackNo = track.TrackNo.ToString();
            Instrumental = track.Instrumental;

            Tags = new ObservableCollection<string>(track.Tags);
        }
    }
}
