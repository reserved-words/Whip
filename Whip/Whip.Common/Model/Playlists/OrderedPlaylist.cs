using System.Collections.Generic;
using Whip.Common.Enums;

namespace Whip.Common.Model
{
    public class OrderedPlaylist : Playlist
    {
        public OrderedPlaylist(int id, string title, bool favourite)
            :base(PlaylistType.Ordered, id, title, favourite)
        {
            Tracks = new List<string>();
        }

        public List<string> Tracks { get; set; }
    }
}
