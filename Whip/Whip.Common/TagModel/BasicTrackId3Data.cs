using System;

namespace Whip.Common.TagModel
{
    public class BasicTrackId3Data
    {
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        public string ArtistName { get; set; }
        public string AlbumTitle { get; set; }
        public string AlbumArtistName { get; set; }
        public int TrackNo { get; set; }
        public int DiscNo { get; set; }
        public string Year { get; set; }
    }
}
