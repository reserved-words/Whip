using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.Interfaces;
using Whip.Common.Model;

namespace Whip.Common.TrackSorters
{
    public class RandomTrackSorter : IRandomTrackSorter
    {
        public IEnumerable<Track> Sort(IEnumerable<Track> tracks)
        {
            var random = new Random();

            return tracks
                .Select(track => new { track, random = random.Next() })
                .OrderBy(randomTrack => randomTrack.random)
                .Select(randomTrack => randomTrack.track);
        }
    }
}
