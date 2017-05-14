using System.Collections.Generic;

namespace Whip.Common.Model.Playlists.Criteria
{
    public class CriteriaGroup
    {
        public CriteriaGroup()
        {
            TrackCriteria = new List<Criteria<Track>>();
            DiscCriteria = new List<Criteria<Disc>>();
            AlbumCriteria = new List<Criteria<Album>>();
            ArtistCriteria = new List<Criteria<Artist>>();
        }

        public List<Criteria<Track>> TrackCriteria { get; set; }
        public List<Criteria<Disc>> DiscCriteria { get; set; }
        public List<Criteria<Album>> AlbumCriteria { get; set; }
        public List<Criteria<Artist>> ArtistCriteria { get; set; }
    }
}
