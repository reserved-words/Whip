using GalaSoft.MvvmLight;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.LastFm;
using Whip.Services.Interfaces;

namespace Whip.ViewModels
{
    public class CurrentTrackMiniViewModel : ViewModelBase
    {
        private readonly ITrackLoveService _trackLoveService;

        private bool _loved;
        private Track _track;

        public CurrentTrackMiniViewModel(ITrackLoveService trackLoveService)
        {
            _trackLoveService = trackLoveService;
        }

        public async void OnNewTrackStarted(Track track)
        {
            Loved = await _trackLoveService.IsLovedAsync(Track);
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
