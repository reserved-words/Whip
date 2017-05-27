using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class CurrentArtistViewModel : TabViewModelBase
    {
        private readonly Common.Singletons.Library _library;

        private bool _showingCurrentArtist = true;
        private Track _currentTrack;
        private Artist _artist;
        private Library.ArtistViewModel _subViewModel;

        public CurrentArtistViewModel(Common.Singletons.Library library, IMessenger messenger, ITrackFilterService trackFilterService,
            ILibrarySortingService librarySortingService, IWebArtistInfoService webArtistInfoService, IImageProcessingService imageProcessingService)
            :base(TabType.CurrentArtist, IconType.Users, "Artist")
        {
            _library = library;

            LibraryArtistViewModel = new Library.ArtistViewModel(trackFilterService, messenger, webArtistInfoService, imageProcessingService, null);

            ShowCurrentArtistCommand = new RelayCommand(ShowCurrentArtist);
        }

        private void ShowCurrentArtist()
        {
            Artist = _currentTrack.Artist;
            _showingCurrentArtist = true;
        }

        public RelayCommand ShowCurrentArtistCommand { get; private set; }

        public List<Artist> Artists => _library.Artists;

        public bool ShowingCurrentArtist
        {
            get { return _showingCurrentArtist; }
            set
            {
                Set(ref _showingCurrentArtist, value);
                RaisePropertyChanged(nameof(NotShowingCurrentArtist));
                if (_showingCurrentArtist)
                {
                    ShowCurrentArtist();
                }
            }
        }

        public bool NotShowingCurrentArtist => !ShowingCurrentArtist;

        public Artist Artist
        {
            get { return _artist; }
            set
            {
                Set(ref _artist, value);
                LibraryArtistViewModel.Artist = _artist;
            }
        }

        public Library.ArtistViewModel LibraryArtistViewModel
        {
            get { return _subViewModel; }
            set { Set(ref _subViewModel, value); }
        }

        public override void OnCurrentTrackChanged(Track track)
        {
            _currentTrack = track;

            if (!_showingCurrentArtist)
                return;

            if (track == null)
            {
                Artist = null;
                return;
            }

            if (Artist == track.Artist)
                return;

            Artist = track.Artist;
            _showingCurrentArtist = true;
        }

        public override void OnShow(Track currentTrack)
        {
            RaisePropertyChanged(nameof(Artists));
            OnCurrentTrackChanged(currentTrack);
        }
    }
}
