using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Linq;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.TabViewModels.Library
{
    public class ArtistViewModel : ViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly ITrackFilterService _trackFilterService;
        private readonly IWebArtistInfoService _webArtistInfoService;

        public ArtistViewModel(ITrackFilterService trackFilterService, IMessenger messenger, IWebArtistInfoService webArtistInfoService)
        {
            _messenger = messenger;
            _trackFilterService = trackFilterService;
            _webArtistInfoService = webArtistInfoService;
        }

        private Artist _artist;
        private List<AlbumViewModel> _albums;

        public Artist Artist
        {
            get { return _artist; }
            set
            {
                Set(ref _artist, value);
                PopulateAlbums();
                PopulateImage();
                RaisePropertyChanged(nameof(NumberOfAlbums));
                RaisePropertyChanged(nameof(NumberOfTracks));
            }
        }

        private async void PopulateImage()
        {
            if (Artist == null)
                return;

            Artist.WebInfo = await _webArtistInfoService.PopulateArtistImages(Artist);
            RaisePropertyChanged(nameof(ImageUrl));
        }

        public List<AlbumViewModel> Albums
        {
            get { return _albums; }
            set { Set(ref _albums, value); }
        }

        public string NumberOfAlbums => Format(Artist?.Albums.Count, "album");
        public string NumberOfTracks => Format(Artist?.Tracks.Count, "track");

        public string ImageUrl => Artist?.WebInfo.LargeImageUrl;

        private string Format(int? count, string description)
        {
            return !count.HasValue
                ? string.Empty
                : count == 1
                ? string.Format("1 {0}", description)
                : string.Format("{0} {1}s", count, description);
        }

        public void OnEditTrack(Track track)
        {
            _messenger.Send(new EditTrackMessage(track));
        }

        public void OnPlay(Track startAt)
        {
            _messenger.Send(new PlayArtistMessage(Artist, SortType.Ordered, startAt));
        }

        public void OnPlayAlbum(AlbumViewModel album)
        {
            _messenger.Send(new PlayAlbumMessage(album.Album, SortType.Ordered));
        }

        private void PopulateAlbums()
        {
            if (Artist == null)
            {
                Albums = new List<AlbumViewModel>();
                return;
            }

            var tracks = _trackFilterService.GetTracksByArtist(Artist);

            Albums = tracks.Select(t => t.Disc)
                .Select(d => d.Album)
                .Distinct()
                .Select(a => new AlbumViewModel(a, this))
                .ToList();
        }
    }
}
