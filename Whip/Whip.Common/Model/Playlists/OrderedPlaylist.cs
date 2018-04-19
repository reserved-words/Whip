using System.Collections.Generic;

namespace Whip.Common.Model
{
    public class OrderedPlaylist : PlaylistBase
    {
        public OrderedPlaylist(int id, string title, bool favourite)
            :base(id, title, favourite)
        {
            Tracks = new List<string>();
        }

        public List<string> Tracks { get; set; }
    }
}
