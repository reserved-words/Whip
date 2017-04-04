using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Model
{
    public class Track
    {
        public Track()
        {

        }

        public string RelativeFilepath { get; set; }
        public string Title { get; set; }
        public int TrackNo { get; set; }

        public Artist Artist { get; set; }
        public Disc Disc { get; set; }
    }
}
