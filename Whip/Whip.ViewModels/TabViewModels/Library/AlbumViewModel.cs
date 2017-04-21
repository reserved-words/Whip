using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.Model;
using GalaSoft.MvvmLight;

namespace Whip.ViewModels.TabViewModels.Library
{
    public class AlbumViewModel : ViewModelBase
    {
        private readonly ArtistViewModel _artist;
        private Track _selectedTrack;

        public AlbumViewModel(Album album, ArtistViewModel artist)
        {
            _artist = artist;

            Album = album;
            ByArtist = album.Artist == artist.Artist;

            EditTrackCommand = new RelayCommand(OnEditTrack);
            PlayCommand = new RelayCommand(OnPlay);
            PlayArtistCommand = new RelayCommand(OnPlayArtist);

            PopulateTracks();
        }

        public RelayCommand EditTrackCommand { get; private set; }
        public RelayCommand PlayArtistCommand { get; private set; }
        public RelayCommand PlayCommand { get; private set; }

        public bool IsMultiDisc => Album.MultiDisc;
        public bool ByArtist { get; private set; }
        public Album Album { get; private set; }
        public List<Track> Tracks { get; private set; }

        public Track SelectedTrack
        {
            get { return _selectedTrack; }
            set { Set(ref _selectedTrack, value); }
        }

        private void OnEditTrack()
        {
            _artist.OnEditTrack(SelectedTrack);
        }

        private void OnPlay()
        {
            _artist.OnPlayAlbum(this);
        }

        private void OnPlayArtist()
        {
            _artist.OnPlay(SelectedTrack);
        }

        private void PopulateTracks()
        {
            if (ByArtist)
            {
                Tracks = Album.Discs.SelectMany(d => d.Tracks).ToList();
            }
            else
            {
                Tracks = Album.Discs.SelectMany(d => d.Tracks).Where(t => t.Artist == _artist.Artist).ToList();
            }
        }
    }
}
