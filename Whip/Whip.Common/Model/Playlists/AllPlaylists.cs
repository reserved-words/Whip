using System.Collections.Generic;

namespace Whip.Common.Model
{
    public class AllPlaylists
    {
        public List<CriteriaPlaylist> CriteriaPlaylists { get; set; }
        public List<OrderedPlaylist> OrderedPlaylists { get; set; }
        public List<QuickPlaylist> FavouriteQuickPlaylists { get; set; }
    }
}
