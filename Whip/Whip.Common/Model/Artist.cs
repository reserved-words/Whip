using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Model
{
    public class Artist
    {
        public Artist()
        {
            Albums = new List<Album>();
            Tracks = new List<Track>();
        }

        public string Name { get; set; }

        public ICollection<Album> Albums { get; set; }
        public ICollection<Track> Tracks { get; set; }
    }
}
