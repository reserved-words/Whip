using Whip.Common;
using Whip.Common.Model;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class CurrentArtistViewModel : TabViewModelBase
    {
        private Artist _artist;

        public CurrentArtistViewModel()
            :base(TabType.CurrentArtist, IconType.Users, "Current Artist")
        {

        }

        public Artist Artist
        {
            get { return _artist; }
            set { Set(ref _artist, value); }
        }

        public override void OnCurrentTrackChanged(Track track)
        {
            if (Artist == track.Artist)
                return;

            Artist = track.Artist;
        }
    }
}
