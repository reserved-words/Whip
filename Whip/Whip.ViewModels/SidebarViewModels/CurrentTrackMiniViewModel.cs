using GalaSoft.MvvmLight;
using Whip.Common.Model;

namespace Whip.ViewModels
{
    public class CurrentTrackMiniViewModel : ViewModelBase
    {
        private Track _track;

        public void OnCurrentTrackChanged(Track track)
        {
            Track = track;
        }

        public Track Track
        {
            get { return _track; }
            set { Set(ref _track, value); }
        }
    }
}
