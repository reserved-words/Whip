using System.Collections.Generic;
using System.Linq;
using Whip.Common.Interfaces;
using Whip.Common.Model;

namespace Whip.Common.TrackSorters
{
    public class DefaultTrackSorter : ITrackSorter
    {
        public IEnumerable<Track> Sort(IEnumerable<Track> tracks)
        {
            return tracks.OrderBy(t => t.Disc.Album.Artist.Name)
                .ThenBy(t => t.Disc.Album.ReleaseType)
                .ThenBy(t => t.Disc.Album.Year)
                .ThenBy(t => t.Disc.DiscNo)
                .ThenBy(t => t.TrackNo);
        }
    }
}
