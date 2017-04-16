using Whip.Common;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class CurrentPlaylistViewModel : TabViewModelBase
    {
        public CurrentPlaylistViewModel()
            :base(TabType.CurrentPlaylist, IconType.ListOl, "Current Playlist")
        {

        }
    }
}
