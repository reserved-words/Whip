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
        
        private ArtistViewModel _artistViewModel;
        private AlbumViewModel _albumViewModel;
        private TrackViewModel _trackViewModel;

        private Track _track;
        private bool _syncingArtistNames;

        public EditTrackViewModel(Common.Singletons.Library library, IMessenger messenger, IWebAlbumInfoService webAlbumInfoService)
            : base(TabType.EditTrack, IconType.Edit, "Edit Track", false)
        {
            _library = library;
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
            var errorMessage = TrackViewModel.Error;

            if (!string.IsNullOrEmpty(errorMessage))
            {
                var messageViewModel = new MessageViewModel(_messenger, "Validation Error", errorMessage);
                _messenger.Send(new ShowDialogMessage(messageViewModel));
                return false;
            }

            UpdateTrack(_track);

            // Save to file(s) using ID3 Tag Service

            return true;
        }

        protected override bool CustomCancel()
        {
            return true;
        }

        public void UpdateTrack(Track track)
        {
            track.Title = TrackViewModel.Title;
            track.Year = TrackViewModel.Year;
            track.Tags = TrackViewModel.Tags.ToList();
            track.TrackNo = AlbumViewModel.TrackNo.Value;
            track.Lyrics = TrackViewModel.Lyrics;

            UpdateArtist(track);
            UpdateAlbum(track);

            _library.OnTrackUpdated(track);
        }

        private void UpdateArtist(Track track)
        {
            Artist artist;

            if (ArtistViewModel.ExistingArtistSelected)
            {
                artist = ArtistViewModel.Artist;
            }
            else
            {
                artist = new Artist();
                _library.Artists.Add(artist);
            }

            var originalArtist = track.Artist;

            if (originalArtist != artist)
            {
                originalArtist.Tracks.Remove(track);
                track.Artist = artist;
                artist.Tracks.Add(track);
            }

            track.Artist.Name = ArtistViewModel.Name;
            track.Artist.Grouping = ArtistViewModel.Grouping;
            track.Artist.Genre = ArtistViewModel.Genre;
            track.Artist.City = new City(ArtistViewModel.City, ArtistViewModel.State, ArtistViewModel.Country);
            track.Artist.Website = ArtistViewModel.Website;
            track.Artist.Facebook = ArtistViewModel.Facebook;
            track.Artist.Twitter = ArtistViewModel.Twitter;
            track.Artist.Wikipedia = ArtistViewModel.Wikipedia;
            track.Artist.LastFm = ArtistViewModel.LastFm;

            if (!originalArtist.Tracks.Any() && !originalArtist.Albums.Any())
            {
                _library.Artists.Remove(originalArtist);
            }
        }

        private void UpdateAlbum(Track track)
        {
            Artist albumArtist = null;
            Album album = null;

            if (AlbumViewModel.ExistingAlbumArtistSelected)
            {
                albumArtist = AlbumViewModel.Artist;

                if (AlbumViewModel.ExistingAlbumSelected)
                {
                    album = AlbumViewModel.Album;
                }
            }
            else
            {
                // Could be in the library already if added as the track artist
                albumArtist = _library.Artists.SingleOrDefault(a => a.Name == AlbumViewModel.ArtistName);
            }

            if (albumArtist == null)
            {
                albumArtist = new Artist();
                _library.Artists.Add(albumArtist);
            }

            if (album == null)
            {
                album = new Album { Artist = albumArtist };
                albumArtist.Albums.Add(album);
            }

            var originalDisc = track.Disc;

            var originalAlbum = originalDisc.Album;

            if (originalAlbum != album || originalDisc.DiscNo != AlbumViewModel.DiscNo)
            {
                originalDisc.Tracks.Remove(track);

                track.Disc = album.Discs.SingleOrDefault(d => d.DiscNo == AlbumViewModel.DiscNo);

                if (track.Disc == null)
                {
                    track.Disc = new Disc { DiscNo = AlbumViewModel.DiscNo.Value };
                    album.Discs.Add(track.Disc);
                    track.Disc.Album = album;
                }

                track.Disc.Tracks.Add(track);
            }

            track.Disc.TrackCount = AlbumViewModel.TrackCount.Value;

            track.Disc.Album.Title = AlbumViewModel.Title;
            track.Disc.Album.Year = AlbumViewModel.Year;
            track.Disc.Album.ReleaseType = AlbumViewModel.ReleaseType;
            track.Disc.Album.DiscCount = AlbumViewModel.DiscCount.Value;

            if (!originalDisc.Tracks.Any())
            {
                originalDisc.Album.Discs.Remove(originalDisc);
            }

            if (!originalAlbum.Discs.Any())
            {
                originalAlbum.Artist.Albums.Remove(originalAlbum);
            }

            if (!originalAlbum.Artist.Albums.Any() && !originalAlbum.Artist.Tracks.Any())
            {
                _library.Artists.Remove(originalAlbum.Artist);
            }
        }
    }
}
