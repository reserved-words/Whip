using Whip.Common.Model;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class CurrentTrackViewModel : TabViewModelBase
    {
        private Track _track;

        public CurrentTrackViewModel()
            :base(TabType.CurrentTrack)
        {

        }

        public Track Track
        {
            get { return _track; }
            set { Set(ref _track, value); }
        }

        public override void OnCurrentTrackChanged(Track track)
        {
            if (Track == track)
                return;

            Track = track;
        }
    }
}
