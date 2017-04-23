using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Linq;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;
using Whip.ViewModels.TabViewModels.EditTrack;
using Whip.ViewModels.Utilities;
using Whip.ViewModels.Windows;
using static Whip.Resources.Resources;

namespace Whip.ViewModels.TabViewModels
{
    public class EditTrackViewModel : EditableTabViewModelBase
    {
        private readonly Common.Singletons.Library _library;
        private readonly IImageProcessingService _imageProcessingService;
        private readonly IMessenger _messenger;
        private readonly IWebAlbumInfoService _webAlbumInfoService;
        private readonly ITrackUpdateService _trackUpdateService;
        private readonly IWebBrowserService _webBrowserService;

        private TrackViewModel _track;

        private Track _editedTrack;
        
        public EditTrackViewModel(Common.Singletons.Library library, IMessenger messenger, IWebAlbumInfoService webAlbumInfoService,
             ITrackUpdateService trackUpdateService, IImageProcessingService imageProcessingService, IWebBrowserService webBrowserService)
            : base(TabType.EditTrack, IconType.Edit, "Edit Track", false)
        {
            _library = library;

            _imageProcessingService = imageProcessingService;
            _messenger = messenger;
            _trackUpdateService = trackUpdateService;
            _webAlbumInfoService = webAlbumInfoService;
            _webBrowserService = webBrowserService;

            LyricsWebSearchCommand = new RelayCommand(OnLyricsWebSearch);
        }

        public RelayCommand LyricsWebSearchCommand { get; private set; }
        
        public override bool Modified
        {
            get { return TrackModified || ArtistModified || DiscModified || AlbumModified; }
            set
            {
                TrackModified = value;
                ArtistModified = value;
                DiscModified = value;
                AlbumModified = value;
            }
        }

        private bool AlbumModified
        {
            get { return Track.Disc.Album.Modified; }
            set { Track.Disc.Album.Modified = value; }
        }

        private bool ArtistModified
        {
            get { return Track.Artist.Modified; }
            set { Track.Artist.Modified = value; }
        }

        private bool DiscModified
        {
            get { return Track.Disc.Modified; }
            set { Track.Disc.Modified = value; }
        }

        private bool TrackModified
        {
            get { return Track.Modified; }
            set { Track.Modified = value; }
        }

        public TrackViewModel Track
        {
            get { return _track; }
            set { Set(ref _track, value); }
        }

        public void Edit(Track track)
        {
            _editedTrack = track;

            var artists = _library.Artists.ToList();
            artists.Insert(0, new Artist { Name = AddNew });

            var tags = artists.SelectMany(a => a.Tracks).SelectMany(t => t.Tags).Distinct().OrderBy(t => t).ToList();

            Track = new TrackViewModel(this, _messenger, _webAlbumInfoService, _imageProcessingService, _webBrowserService, artists, tags, track);
        }

        protected override bool CustomCancel()
        {
            return true;
        }

        protected override bool CustomSave()
        {
            if (!Validate())
                return false;

            var originalArtist = _editedTrack.Artist;
            var originalDisc = _editedTrack.Disc;

            Track.UpdateTrack(_editedTrack);

            _trackUpdateService.SaveTrackChanges(_editedTrack, originalArtist, originalDisc, TrackModified, ArtistModified, DiscModified, AlbumModified);

            return true;
        }

        private void OnLyricsWebSearch()
        {
            _webBrowserService.OpenSearch(Track.Artist.Name, Track.Title, "lyrics");
        }

        private bool Validate()
        {
            var errorMessage = Track.Error; 
            
            if (!string.IsNullOrEmpty(errorMessage))
            {
                errorMessage = string.Format("Please resolve the following validation errors:{0}{0}{1}", Environment.NewLine, errorMessage);
                var messageViewModel = new MessageViewModel(_messenger, "Validation Error", errorMessage);
                _messenger.Send(new ShowDialogMessage(messageViewModel));
                return false;
            }

            return true;
        }
    }
}
