using GalaSoft.MvvmLight.Messaging;
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
        private readonly IMessenger _messenger;
        private readonly IWebAlbumInfoService _webAlbumInfoService;
        private readonly ILibraryDataOrganiserService _libraryOrganiserService;

        private ArtistViewModel _artistViewModel;
        private AlbumViewModel _albumViewModel;
        private TrackViewModel _trackViewModel;

        private Track _track;
        private bool _syncingArtistNames;

        public EditTrackViewModel(Common.Singletons.Library library, IMessenger messenger, IWebAlbumInfoService webAlbumInfoService,
            ILibraryDataOrganiserService libraryOrganiserService)
            : base(TabType.EditTrack, IconType.Edit, "Edit Track", false)
        {
            _library = library;
            _libraryOrganiserService = libraryOrganiserService;
            _messenger = messenger;
            _webAlbumInfoService = webAlbumInfoService;
        }

        public override bool Modified
        {
            get { return TrackViewModel.Modified || ArtistViewModel.Modified || AlbumViewModel.Modified; }
            set
            {
                TrackViewModel.Modified = value;
                ArtistViewModel.Modified = value;
                AlbumViewModel.Modified = value;
            }
        }

        public TrackViewModel TrackViewModel
        {
            get { return _trackViewModel; }
            set { Set(ref _trackViewModel, value); }
        }

        public ArtistViewModel ArtistViewModel
        {
            get { return _artistViewModel; }
            set { Set(ref _artistViewModel, value); }
        }

        public AlbumViewModel AlbumViewModel
        {
            get { return _albumViewModel; }
            set { Set(ref _albumViewModel, value); }
        }

        public void Edit(Track track)
        {
            _track = track;

            var artists = _library.Artists.ToList();
            artists.Insert(0, new Artist { Name = AddNew });

            var tags = artists.SelectMany(a => a.Tracks).SelectMany(t => t.Tags).Distinct().OrderBy(t => t).ToList();

            ArtistViewModel = new ArtistViewModel(this, artists, track);
            AlbumViewModel = new AlbumViewModel(this, _messenger, _webAlbumInfoService, artists, track);
            TrackViewModel = new TrackViewModel(track, tags);
        }

        public void SyncArtistNames(string latestName)
        {
            if (ArtistViewModel == null || AlbumViewModel == null)
                return;

            if (!_syncingArtistNames && ArtistViewModel.Artist == AlbumViewModel.Artist && ArtistViewModel.Name != AlbumViewModel.ArtistName)
            {
                _syncingArtistNames = true;
                ArtistViewModel.Name = latestName;
                AlbumViewModel.ArtistName = latestName;
                _syncingArtistNames = false;
            }
        }

        protected override bool CustomSave()
        {
            if (!Validate())
                return false;

            UpdateLibraryTrack();
            
            // Save to file(s) using ID3 Tag Service

            return true;
        }

        private void UpdateLibraryTrack()
        {
            var originalArtist = _track.Artist;
            var originalDisc = _track.Disc;

            TrackViewModel.UpdateTrack(_track);
            ArtistViewModel.UpdateTrack(_track);
            AlbumViewModel.UpdateTrack(_track);

            _libraryOrganiserService.ReorganiseOnTrackChange(_track, originalArtist, originalDisc);
        }

        private bool Validate()
        {
            var errorMessage = TrackViewModel.Error;

            if (!string.IsNullOrEmpty(errorMessage))
            {
                var messageViewModel = new MessageViewModel(_messenger, "Validation Error", errorMessage);
                _messenger.Send(new ShowDialogMessage(messageViewModel));
                return false;
            }

            return true;
        }

        protected override bool CustomCancel()
        {
            return true;
        }
    }
}
