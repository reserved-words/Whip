using System.Collections.Generic;

namespace Whip.Common.Model
{
    public class Album
    {
        public Album()
        {
            Discs = new List<Disc>();
        }

        public string Title { get; set; }
        public string Year { get; set; }
        public int DiscCount { get; set; }
        public ReleaseType ReleaseType { get; set; }
        public ReleaseTypeGrouping Grouping { get; set; }

        public byte[] Artwork { get; set; }

        public Artist Artist { get; set; }

        public List<Disc> Discs { get; set; }
        
        public bool MultiDisc => Discs.Count > 1;
        public bool IsAlbum => ReleaseType == ReleaseType.Album;

        public override string ToString()
        {
            return string.Format("{0} by {1}", Title, Artist.Name);
        }
    }
}
