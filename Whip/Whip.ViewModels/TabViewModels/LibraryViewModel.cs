using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.Model;

namespace Whip.ViewModels.TabViewModels
{
    public class LibraryViewModel : ViewModelBase
    {
        private List<Album> _albums;
        private List<Artist> _artists;
        private Library _library;
        private List<Album> _selectedAlbums;
        private Album _selectedAlbum;
        private Artist _selectedArtist;

        public LibraryViewModel()
        {
            Artists = new List<Artist>();
            Albums = new List<Album>();

            ClearSelectedAlbum = new RelayCommand(OnClearSelectedAlbum, CanClearSelectedAlbum);
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

        public RelayCommand ClearSelectedAlbum { get; private set; }

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
                Albums = _selectedArtist.Albums.OrderBy(a => a.Year).ToList();
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
                ClearSelectedAlbum.RaiseCanExecuteChanged();
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

        public void OnLibraryUpdated(Library library)
        {
            _library = library;

            Artists = _library.Artists;
        }
    }
}
