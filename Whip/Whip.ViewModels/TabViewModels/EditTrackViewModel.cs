using GalaSoft.MvvmLight.Messaging;
using System.Linq;
using Whip.Common;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.ViewModels.Messages;
using Whip.ViewModels.Utilities;
using Whip.ViewModels.Windows;

namespace Whip.ViewModels.TabViewModels
{
    public class EditTrackViewModel : EditableTabViewModelBase
    {
        private readonly Library _library;
        private readonly IMessenger _messenger;
 
        private Track _track;
        
        public EditTrackViewModel(TrackViewModel trackViewModel, Library library, IMessenger messenger)
            : base(TabType.EditTrack, IconType.Edit, "Edit Track", false)
        {
            _library = library;
            _messenger = messenger;

            TrackViewModel = trackViewModel;
        }

        public override bool Modified
        {
            get { return TrackViewModel.Modified; }
            set { TrackViewModel.Modified = value; }
        }

        public TrackViewModel TrackViewModel { get; private set; }

        public void Edit(Track track)
        {
            _track = track;

            TrackViewModel.Populate(track);
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

            TransferData(_track);

            // Save to file(s) using ID3 Tag Service

            return true;
        }

        protected override bool CustomCancel()
        {
            return true;
        }

        public void TransferData(Track track)
        {
            track.Title = TrackViewModel.Title;
            track.Year = TrackViewModel.Year;
            track.Tags = TrackViewModel.Tags.ToList();
            track.TrackNo = TrackViewModel.TrackNo.Value;
            track.Lyrics = TrackViewModel.Lyrics;

            SetArtist(track);
            SetAlbum(track);

            _library.OnTrackUpdated(track);
        }

        private void SetArtist(Track track)
        {
            Artist artist;

            if (TrackViewModel.ExistingArtistSelected)
            {
                artist = TrackViewModel.Artist;
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
            
            track.Artist.Name = TrackViewModel.ArtistName;
            track.Artist.Grouping = TrackViewModel.ArtistGrouping;
            track.Artist.Genre = TrackViewModel.ArtistGenre;
            track.Artist.City = new City(TrackViewModel.ArtistCity, TrackViewModel.ArtistState, TrackViewModel.ArtistCountry);
            track.Artist.Website = TrackViewModel.ArtistWebsite;
            track.Artist.Facebook = TrackViewModel.ArtistFacebook;
            track.Artist.Twitter = TrackViewModel.ArtistTwitter;
            track.Artist.Wikipedia = TrackViewModel.ArtistWikipedia;
            track.Artist.LastFm = TrackViewModel.ArtistLastFm;

            if (!originalArtist.Tracks.Any() && !originalArtist.Albums.Any())
            {
                _library.Artists.Remove(originalArtist);
            }
        }

        private void SetAlbum(Track track)
        {
            Artist albumArtist = null;
            Album album = null;

            if (TrackViewModel.ExistingAlbumArtistSelected)
            {
                albumArtist = TrackViewModel.AlbumArtist;

                if (TrackViewModel.ExistingAlbumSelected)
                {
                    album = TrackViewModel.Album;
                }
            }
            else
            {
                // Could be in the library already if added as the track artist
                albumArtist = _library.Artists.SingleOrDefault(a => a.Name == TrackViewModel.AlbumArtistName);   
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

            if (originalAlbum != album || originalDisc.DiscNo != TrackViewModel.DiscNo)
            {
                originalDisc.Tracks.Remove(track);

                track.Disc = album.Discs.SingleOrDefault(d => d.DiscNo == TrackViewModel.DiscNo);

                if (track.Disc == null)
                {
                    track.Disc = new Disc { DiscNo = TrackViewModel.DiscNo.Value };
                    album.Discs.Add(track.Disc);
                    track.Disc.Album = album;
                }

                track.Disc.Tracks.Add(track);
            }
            
            track.Disc.TrackCount = TrackViewModel.TrackCount.Value;

            track.Disc.Album.Title = TrackViewModel.AlbumTitle;
            track.Disc.Album.Year = TrackViewModel.AlbumYear;
            track.Disc.Album.ReleaseType = TrackViewModel.AlbumReleaseType;
            track.Disc.Album.DiscCount = TrackViewModel.DiscCount.Value;

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
