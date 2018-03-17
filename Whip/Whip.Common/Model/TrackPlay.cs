using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Model
{
    public class TrackPlay
    {
        public bool NowPlaying { get; set; }
        public string Track { get; set; }
        public string Url { get; set; }
        public string TimePlayed { get; set; }
        public string ImageUrl { get; set; }
    }
}
