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

            return true;

            // Move this stuff to a service
            // TransferData(_track);

            // Save to file using ID3 Tag Service
        }

        protected override bool CustomCancel()
        {
            return true;
        }

        public void TransferData(Track track)
        {
            track.Title = TrackViewModel.Title;
            track.Year = TrackViewModel.Year;
            // track.Tags = TrackViewModel.Tags;
            track.TrackNo = TrackViewModel.TrackNo.Value;
            track.Lyrics = TrackViewModel.Lyrics;

            SetArtist(track);
        }

        private void SetArtist(Track track)
        {
            if (track.Artist.Name != TrackViewModel.ArtistName)
            {
                track.Artist.Tracks.Remove(track);

                var artist = _library.Artists.SingleOrDefault(a => a.Name == TrackViewModel.ArtistName);

                if (artist == null)
                {
                    artist = new Artist { Name = TrackViewModel.ArtistName };
                    _library.Artists.Add(artist);
                }

                track.Artist = artist;
                artist.Tracks.Add(track);
            }

            track.Artist.Grouping = TrackViewModel.ArtistGrouping;
            track.Artist.Genre = TrackViewModel.ArtistGenre;
            track.Artist.City = new City(TrackViewModel.ArtistCity, TrackViewModel.ArtistState, TrackViewModel.ArtistCountry);
            track.Artist.Website = TrackViewModel.ArtistWebsite;
            track.Artist.Facebook = TrackViewModel.ArtistFacebook;
            track.Artist.Twitter = TrackViewModel.ArtistTwitter;
            track.Artist.Wikipedia = TrackViewModel.ArtistWikipedia;
            track.Artist.LastFm = TrackViewModel.ArtistLastFm;
        }
    }
}
