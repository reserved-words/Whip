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
            Tags = new List<string>();
        }

        public File File { get; set; }
        public string Title { get; set; }
        public int TrackNo { get; set; }
        public TimeSpan Duration { get; set; }
        public List<string> Tags { get; set; }
        public string Year { get; set; }
        public string Lyrics { get; set; }

        public Artist Artist { get; set; }
        public Disc Disc { get; set; }

        public string TagsDescription => Tags.Any() ? string.Join(", ", Tags) : "-";
        public string TrackNoDescription => string.Format("{0} of {1}", TrackNo, Disc.TrackCount);
        public string DiscNoDescription => string.Format("{0} of {1}", Disc.DiscNo, Disc.Album.DiscCount);

        public override string ToString()
        {
            return $"{Title} by {Artist.Name}";
        }
    }
}
