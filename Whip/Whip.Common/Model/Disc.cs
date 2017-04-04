using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Model
{
    public class Disc
    {
        public Disc()
        {
            Tracks = new List<Track>();
        }

        public Album Album { get; set; }
        public int DiscNo { get; set; }

        public ICollection<Track> Tracks { get; set; }
    }
}
