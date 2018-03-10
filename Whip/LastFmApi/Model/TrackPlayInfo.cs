using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastFmApi.Model
{
    public class TrackPlayInfo
    {
        public bool NowPlaying { get; set; }
        public string ArtistName { get; set; }
        public string TrackTitle { get; set; }
        public string AlbumTitle { get; set; }
        public string Url { get; set; }
        public DateTime? TimePlayed { get; set; }
        public string ImageUrl { get; set; }
    }
}
