using System.Collections.Generic;

namespace LastFmApi
{
    public class ArtistInfo
    {
        public string Name { get; set; }
        public string Wiki { get; set; }
        public string SmallImageUrl { get; set; }
        public string MediumImageUrl { get; set; }
        public string LargeImageUrl { get; set; }
        public string ExtraLargeImageUrl { get; set; }
        public List<ArtistInfo> SimilarArtists { get; set; }
        public string Url { get; set; }
    }
}
