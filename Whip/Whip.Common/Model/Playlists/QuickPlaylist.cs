using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Enums;

namespace Whip.Common.Model
{
    public class QuickPlaylist : Playlist
    {
        public QuickPlaylist(int id, string title, bool favourite, FilterType filterType, params string[] filterValues)
            :base(PlaylistType.Quick, id, title, favourite)
        {
            FilterType = filterType;
            FilterValues = filterValues;
        }

        public FilterType FilterType { get; set; }
        public string[] FilterValues { get; set; }
    }
}
