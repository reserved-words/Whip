using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Model
{
    public class TrackId3Data
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string AlbumTitle { get; set; }
        public string AlbumArtist { get; set; }
        public int TrackNo { get; set; }
        public int DiscNo { get; set; }
        public string AlbumYear { get; set; }
        public string Genre { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
