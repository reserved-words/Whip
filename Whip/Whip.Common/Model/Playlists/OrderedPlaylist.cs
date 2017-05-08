using System.Collections.Generic;

namespace Whip.Common.Model
{
    public class OrderedPlaylist : PlaylistBase
    {
        public OrderedPlaylist()
        {
            Tracks = new List<string>();
        }

        public List<string> Tracks { get; set; }
    }
}
