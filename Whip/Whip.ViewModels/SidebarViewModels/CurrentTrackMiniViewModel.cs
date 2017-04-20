using GalaSoft.MvvmLight;
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

            library.TrackUpdated += Library_TrackUpdated;
        }

        private void Library_TrackUpdated(Track track)
        {
            if (Track == track)
            {
                RaisePropertyChanged(nameof(Track));
            }
        }

        public async void OnNewTrackStarted(Track track)
        {
            try
            {
                Loved = await _trackLoveService.IsLovedAsync(Track);
            }
            catch (Exception ex)
            {
                // Should be logged by method interceptor, for now just ignore
            }
        }

        public void OnCurrentTrackChanged(Track track)
        {
            Loved = false;
            Track = track;
        }

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
    }
}
