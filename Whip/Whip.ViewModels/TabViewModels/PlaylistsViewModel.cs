using Whip.Common;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class PlaylistsViewModel : TabViewModelBase
    {
        public PlaylistsViewModel()
            :base(TabType.Playlists, IconType.ListUl, "Current Playlist")
        {

        }
    }
}
