using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.ViewModels.Utilities;
using Whip.ViewModels.Validation;
using static Whip.Resources.Resources;

namespace Whip.ViewModels.TabViewModels.EditTrack
{
    public class DiscViewModel : EditableViewModelBase
    {
        private TrackViewModel _track;
        private string _trackCount;
        private string _discNo;

        public DiscViewModel(TrackViewModel trackViewModel, IMessenger messenger, IAlbumInfoService albumInfoService, IImageProcessingService imageProcessingService,
            IFileDialogService fileDialogService, List<Artist> artists, Track track)
        {
            _track = trackViewModel;

            Album = new AlbumViewModel(this, messenger, albumInfoService, imageProcessingService, fileDialogService, artists, track);

            Populate(track.Disc);

            Modified = false;
        }

        public AlbumViewModel Album { get; private set; }

        [Required(ErrorMessageResourceName = nameof(RequiredErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [TrackCount]
        [Display(Name = "Track Count")]
        public string TrackCount
        {
            get { return _trackCount; }
            set
            {
                SetModified(nameof(TrackCount), ref _trackCount, value);

                _track.ValidateTrackNo();
            }
        }

        [Required(ErrorMessageResourceName = nameof(RequiredErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [DiscNo]
        [Display(Name = "Disc No")]
        public string DiscNo
        {
            get { return _discNo; }
            set
            {
                SetModified(nameof(DiscNo), ref _discNo, value);
                PopulateDiscDetails();
            }
        }

        public void PopulateDiscDetails()
        {
            var album = Album?.Album?.Title == AddNew ? null : Album?.Album;

            var disc = album?.Discs.SingleOrDefault(d => d.DiscNo.ToString() == DiscNo);

            if (disc != null)
            {
                TrackCount = disc.TrackCount.ToString();
            }
        }

        public void SyncArtists(bool sync)
        {
            _track.SyncArtists(sync);
        }

        public void UpdateDisc(Track track)
        {
            var albumArtist = Album.ArtistName == track.Artist.Name
                ? track.Artist
                : Album.ExistingAlbumArtistSelected
                ? Album.Artist
                : new Artist();

            var album = Album.ExistingAlbumSelected
                ? Album.Album
                : new Album { Artist = albumArtist };

            var disc = album.Discs.SingleOrDefault(d => d.DiscNo.ToString() == DiscNo) ?? new Disc { Album = album };

            disc.DiscNo = Convert.ToInt16(DiscNo);
            disc.TrackCount = Convert.ToInt16(TrackCount);

            Album.UpdateAlbum(track.Disc.Album);

            track.Disc = disc;
        }

        public void ValidateDiscNo()
        {
            var discNo = DiscNo;
            DiscNo = null;
            DiscNo = discNo;
        }

        private void Populate(Disc disc)
        {
            DiscNo = disc.DiscNo.ToString();
            TrackCount = disc.TrackCount.ToString();
        }
    }
}
