using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Model
{
    public class AllPlaylists
    {
        public List<CriteriaPlaylist> CriteriaPlaylists { get; set; }
        public List<OrderedPlaylist> OrderedPlaylists { get; set; }
    }
}
