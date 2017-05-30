
using System;
using System.Collections.Generic;

namespace Whip.Common.Model
{
    public class ArtistWebInfo
    {
        public DateTime Updated { get; set; }
        public string Wiki { get; set; }
        public string SmallImageUrl { get; set; }
        public string MediumImageUrl { get; set; }
        public string LargeImageUrl { get; set; }
        public string ExtraLargeImageUrl { get; set; }
        public List<ArtistWebSimilarArtist> SimilarArtists { get; set; } = new List<ArtistWebSimilarArtist>();
    }
}
