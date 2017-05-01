using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;

namespace Whip.ViewModels
{
    public class CurrentTrackMiniViewModel : ViewModelBase
    {
        private readonly ITrackLoveService _trackLoveService;
        
        private bool _loved;
        private Track _track;

        public CurrentTrackMiniViewModel(ITrackLoveService trackLoveService, Library library)
        {
            _trackLoveService = trackLoveService;

            library.Updated += OnLibraryUpdated;

            LoveTrackCommand = new RelayCommand(OnLoveTrack);
            UnloveTrackCommand = new RelayCommand(OnUnloveTrack);
        }

        public RelayCommand LoveTrackCommand { get; private set; }
        public RelayCommand UnloveTrackCommand { get; private set; }

        public Track Track
        {
            get { return _track; }
            set { Set(ref _track, value); }
        }

        public bool Loved
        {
            get { return _loved; }
            set { Set(ref _loved, value); }
        }
        private void OnLibraryUpdated(Track track)
        {
            if (track != null && Track == track)
            {
                RaisePropertyChanged(nameof(Track));
            }
        }

        public void OnCurrentTrackChanged(Track track)
        {
            Loved = false;
            Track = track;
        }

        public async void OnNewTrackStarted(Track track)
        {
            Loved = await _trackLoveService.IsLovedAsync(Track);
        }

        private async void OnLoveTrack()
        {
            Loved = true;
            await _trackLoveService.LoveTrackAsync(Track);
        }

        private async void OnUnloveTrack()
        {
            Loved = false;
            await _trackLoveService.UnloveTrackAsync(Track);
        }
    }
}
