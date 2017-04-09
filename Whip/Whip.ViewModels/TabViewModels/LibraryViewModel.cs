using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using Whip.Common.Enums;
using Whip.Common.ExtensionMethods;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.TabViewModels
{
    public class LibraryViewModel : ViewModelBase
    {
        private readonly Library _library;
        private readonly IMessenger _messenger;

        private List<Album> _albums;
        private List<Artist> _artists;
        private List<Album> _selectedAlbums;
        private Album _selectedAlbum;
        private Artist _selectedArtist;

        public LibraryViewModel(Library library, IMessenger messenger)
        {
            _library = library;
            _messenger = messenger;

            _library.Updated += OnLibraryUpdated;
            
            Artists = new List<Artist>();
            Albums = new List<Album>();

            ClearSelectedAlbumCommand = new RelayCommand(OnClearSelectedAlbum, CanClearSelectedAlbum);
            PlayAlbumCommand = new RelayCommand<Album>(OnPlayAlbum);
            PlaySelectedAlbumsCommand = new RelayCommand<Track>(OnPlaySelectedAlbums);
            ShuffleArtistCommand = new RelayCommand<Artist>(OnShuffleArtist);
        }

        public List<Artist> Artists
        {
            get { return _artists; }
            private set { Set(ref _artists, value); }
        }

        public List<Album> Albums
        {
            get { return _albums; }
            private set { Set(ref _albums, value); }
        }

        public RelayCommand ClearSelectedAlbumCommand { get; private set; }
        public RelayCommand<Album> PlayAlbumCommand { get; private set; }
        public RelayCommand<Track> PlaySelectedAlbumsCommand { get; private set; }
        public RelayCommand<Artist> ShuffleArtistCommand { get; private set; }
        
        public List<Album> SelectedAlbums
        {
            get { return _selectedAlbums; }
            private set { Set(ref _selectedAlbums, value); }
        }

        public Artist SelectedArtist
        {
            get { return _selectedArtist; }
            set
            {
                Set(ref _selectedArtist, value);
                Albums = _selectedArtist?.GetAlbumsInOrder();
                SelectedAlbum = null;
            }
        }

        public Album SelectedAlbum
        {
            get { return _selectedAlbum; }
            set
            {
                Set(ref _selectedAlbum, value);
                SelectedAlbums = _selectedAlbum == null
                    ? Albums
                    : new List<Album> { _selectedAlbum };
                ClearSelectedAlbumCommand.RaiseCanExecuteChanged();
            }
        }

        private bool CanClearSelectedAlbum()
        {
            return SelectedAlbum != null && Albums.Count > 1;
        }

        private void OnClearSelectedAlbum()
        {
            SelectedAlbum = null;
        }

        private void OnLibraryUpdated()
        {
            Artists = _library.Artists;
        }

        private void OnPlayAlbum(Album album)
        {
            _messenger.Send(new PlayAlbumsMessage(album, SortType.Ordered));
        }

        private void OnShuffleArtist(Artist artist)
        {
            _messenger.Send(new PlayArtistsMessage(artist, SortType.Random));
        }

        private void OnPlaySelectedAlbums(Track startAtTrack)
        {
            _messenger.Send(new PlayAlbumsMessage(SelectedAlbums, SortType.Ordered, startAtTrack));
        }
    }
}
